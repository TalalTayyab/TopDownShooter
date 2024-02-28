using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _slimePrefab;
    [SerializeField] private GameObject _dogPrefab;
    [SerializeField] private GameObject _frogPrefab;
    [SerializeField] private GameObject _eyePrefab;
    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;
    [SerializeField] private Transform _left;
    [SerializeField] private Transform _top;
    [SerializeField] private Transform _right;
    [SerializeField] private Transform _bottom;
    [SerializeField] private CoinUI _coins;
    [SerializeField] private PowerUpUI _powerUpUI;
    [SerializeField] private int _coinsRequiredForPowerUp;
    [SerializeField] private ExperienceUI _experienceTracker;
    [SerializeField] private AudioSource _audioZombieMoan;
    [SerializeField] private AudioSource _audioLevel1;
    [SerializeField] private GameObject _player;

    private Transform _playerTransform;
    private float _timeUntilSpawn;
    private GameTimeScript _gameTimeScript;
    private int wave = 1;
    public int _level = 1;
    private Camera _camera;
    public int CoinsRequiredForLevelling
    {
        get
        {
            return _coinsRequiredForPowerUp + (int) (_coinsRequiredForPowerUp * 0.3 * _level ); 
        }
    }

    public PowerUpManager PowerUpManager => PowerUpManagerFactory.PowerUpManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameTimeScript = FindObjectOfType<GameTimeScript>();
        _camera = Camera.main;

        StartCoroutine(PlayZombieSound());
    }

    void PowerUpHandler()
    {
        _experienceTracker.UpdateExperience(_coins.LevelCoins, CoinsRequiredForLevelling);
        if (_coins.LevelCoins % CoinsRequiredForLevelling == 0 && _coins.LevelCoins > 0)
        {
            _powerUpUI.Show(PowerUpManager.GetPowerUpsFromEachCategory());
            _level++;
            _coins.NewLevel();
        }
    }

    // Update is called once per frame
    void Update()
    {
        _timeUntilSpawn -= Time.deltaTime;
        PowerUpHandler();

        if (_timeUntilSpawn <= 0)
        {
            //   Spawn();
        }

        Waves();
    }

    IEnumerator PlayZombieSound()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3,20));
            _audioZombieMoan.Play();
        }
    }

    private void Waves()
    {
        if (_gameTimeScript.Seconds >= 1)
        {
            StartCoroutine(S1());
        }
    }

    private List<GameObject> RemoveDestroyed(GameObject[] enemies)
    {
        return enemies.Where(x => x != null).ToList();
    }

    void MoveEnemiesIntoCircle(GameObject[] enemies)
    {
        
        int delta = 360 / enemies.Length;
        int start = 0;
        var radius = 4f;
        Vector2 anchor = _player.transform.position;

        for (var i = 0; i < enemies.Length; i++)
        {
            var angle = Mathf.Deg2Rad * start;
            var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
            var em = enemies[i].GetComponent<EnemyMovement>();
            em.UseFixedPositionForMovement = true;
            em.ReachedPosition = false;
            em.FixedPositionForTarget = anchor + offset;
            start += delta;

        }
    }

    IEnumerator SpawnEnemyInCircle(int enemies, float speed, float health, int wait1, int wait2, float speed2)
    {
        enemies = Mathf.Clamp(enemies, 1, 5);

        var l = new List<GameObject>();

        l.AddRange(Spawn(enemies, speed, health, "top", false));
        l.Reverse();
        l.AddRange(Spawn(enemies, speed, health, "right", false));
        l.AddRange(Spawn(enemies, speed, health, "bottom", false));
        l.AddRange(Spawn(enemies, speed, health, "left", false));

        var cnt = 0;

        while (cnt<= wait1)
        {
            // remove destroyed objects
            l = RemoveDestroyed(l.ToArray());
            MoveEnemiesIntoCircle(l.ToArray());
            yield return new WaitForSeconds(0.1f);
            var p = l.Count(x => x!=null && x.GetComponent<EnemyMovement>().ReachedPosition == false);
            if (p == 0) break; // all enemies reached
            cnt++;
        }

        yield return new WaitForSeconds(0.4f);

        l.ForEach(x => {
            if (x == null) return;
            var em = x.GetComponent<EnemyMovement>();
            em.UseFixedPositionForMovement = false;
            em.SetSpeed(0);
            em.DontUseTargetCD = true;
        });

        yield return new WaitForSeconds(1f);

        cnt = 0;

        while (cnt <= wait2)
        {
            l.ForEach(x => {
                if (x == null) return;
                var em = x.GetComponent<EnemyMovement>();
                em.SetSpeed(speed2);
            });
            yield return new WaitForSeconds(0.1f);
            cnt++;
        }

        l.ForEach(x => {
            if (x == null) return;
            var em = x.GetComponent<EnemyMovement>();
            em.SetSpeed(speed);
        });
    }

    IEnumerator SpawnSwarm(int enemies, float speed, float health, float wait)
    {
        for (var i = 1; i < enemies -1; i++)
        {
            float x, y;
            Vector3 pos;
            var hm = Teleport();

             if (hm.left <= 0.4)
            {
                x = _left.position.x;
                y = i % 2 == 0 ? _left.position.y - (i / 2) : _left.position.y + (i / 2);
                pos = new Vector3(x, y, 0);
                Spawn(10, speed, health, "left", true, false, false, pos);
            }

            if (hm.up <= 0.4)
            {
                x = i % 2 == 0 ? _top.position.x - (i / 2) : _top.position.x + (i / 2);
                y = _top.position.y;
                pos = new Vector3(x, y, 0);
                Spawn(10, speed, health, "top", true, false, false, pos);
            }

            if (hm.right <= 0.4)
            {
                x = _right.position.x;
                y = i % 2 == 0 ? _left.position.y + (i / 2) : _left.position.y - (i / 2);
                pos = new Vector3(x, y, 0);
                Spawn(10, speed, health, "right", true, false, false, pos);
            }

            if (hm.down <= 0.4)
            {
                x = i % 2 == 0 ? _top.position.x + (i / 2) : _top.position.x - (i / 2);
                y = _bottom.position.y;
                pos = new Vector3(x, y, 0);
                Spawn(10, speed, health, "bottom", true, false, false, pos);
            }

            yield return new WaitForSeconds(wait);
        }
    }


    private IEnumerator SmartWait(string tag, float waitForSeconds)
    {
        float totalWaited = 0f;
        float waitFor = 0.3f;
        waitFor = waitFor > waitForSeconds ? waitForSeconds : waitFor;
        while (true)
        {
            var count = GameObject.FindGameObjectsWithTag(tag).Length;
            if (totalWaited >= waitForSeconds || count ==0) break;
            yield return new WaitForSeconds(waitFor);
            totalWaited = totalWaited + waitFor;
            Teleport();
        } 
    }

    private (GameObject[] enemies, GameObject[] left, GameObject[] right, GameObject[] up, GameObject[] down) EnemeyHeatMap()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float cnt = enemies.Count();
        var t = _player.transform.position;


        var left = enemies.Where(e =>
        {
            var r = (t.x - e.transform.position.x);
            //Debug.Log($"left={r}");
            return r >= 3f;
        });
        var right = enemies.Where(e => (t.x - e.transform.position.x) <= -3f);
        //var up = enemies.Where(e => (t.y - e.transform.position.y) <= -3f);
        var up = enemies.Where(e =>
        {
            var r = (t.y - e.transform.position.y);
           // Debug.Log($"up={r}");
            return r <= -3f;
        });
        var down = enemies.Where(e => (t.y - e.transform.position.y) >= 3f);

        return (enemies.ToArray(), left.ToArray(), right.ToArray(), up.ToArray(), down.ToArray());
    }
   

    private (float left, float right, float up, float down) EnemyHeatMapRatios()
    {
        var hm = EnemeyHeatMap();
        float cnt = hm.enemies.Count();

        if (cnt == 0) return (0, 0, 0, 0);

        var lr = hm.left.Count() / cnt;
        var rr = hm.right.Count() / cnt;
        var ur = hm.up.Count() / cnt;
        var dr = hm.down.Count() / cnt;

        // Debug.Log($"left = {lr}, right = {rr}, up = {ur}, down = {dr}");

        return (lr, rr, ur, dr);

    }

    (float left, float right, float up, float down) Teleport()
    {
        var hm = EnemeyHeatMap();
        float cnt = hm.enemies.Count();

        if (cnt == 0) return (0, 0, 0, 0);

        var lr = hm.left.Count() / cnt;
        var rr = hm.right.Count() / cnt;
        var ur = hm.up.Count() / cnt;
        var dr = hm.down.Count() / cnt;

       //Debug.Log($"left = {lr}, right = {rr}, up = {ur}, down = {dr}");

        var teleported = new List<GameObject>();

        var order = new Dictionary<string, float>();
        order.Add("lr", lr);
        order.Add("rr", rr);
        order.Add("ur", ur);
        order.Add("dr", dr);
        var sorted = from entry in order orderby entry.Value ascending select entry;

        foreach (var s in sorted)
        {
            switch (s.Key)
            {
                case "lr":
                    TeleportLeftToRight(hm, lr, teleported);
                    break;

                case "rr":
                    TeleportRightToLeft(hm, rr, teleported);
                    break;

                case "ur":
                    TeleportUpToDown(hm, ur, teleported);
                    break;

                case "dr":
                    TeleportDownToUp(hm, dr, teleported);
                    break;
            }
        }

        return (lr, rr, ur, dr);
    }

    private void TeleportDownToUp((GameObject[] enemies, GameObject[] left, GameObject[] right, GameObject[] up, GameObject[] down) hm, float dr, List<GameObject> teleported)
    {
        if (dr > 0.4)
        {
            foreach (var e in hm.down)
            {
                if (teleported.Exists(x => x.GetInstanceID() == e.GetInstanceID())) continue;
                var visible = e.GetComponent<EnemyMovement>().IsVisible;
                if (!visible)//(pos.x > 1 || pos.y > 1)
                {
                    Debug.Log($"down to up + {e.transform.position} to {GetPosition("top", 0)}");
                    e.transform.position = GetPosition("top", 0);
                    teleported.Add(e);
                }
            }
        }
    }

    private void TeleportUpToDown((GameObject[] enemies, GameObject[] left, GameObject[] right, GameObject[] up, GameObject[] down) hm, float ur, List<GameObject> teleported)
    {
        if (ur > 0.4)
        {
            foreach (var e in hm.up)
            {
                if (teleported.Exists(x => x.GetInstanceID() == e.GetInstanceID())) continue;
                var visible = e.GetComponent<EnemyMovement>().IsVisible;
                if (!visible)//(pos.x > 1 || pos.y > 1)
                {
                    Debug.Log($"up to down + {e.transform.position} to {GetPosition("bottom", 0)}");
                    e.transform.position = GetPosition("bottom", 0);
                    teleported.Add(e);
                }
            }
        }
    }

    private void TeleportRightToLeft((GameObject[] enemies, GameObject[] left, GameObject[] right, GameObject[] up, GameObject[] down) hm, float rr, List<GameObject> teleported)
    {
        if (rr > 0.4)
        {
            foreach (var e in hm.right)
            {
                if (teleported.Exists(x => x.GetInstanceID() == e.GetInstanceID())) continue;
                var visible = e.GetComponent<EnemyMovement>().IsVisible;
                if (!visible)//(pos.x > 1 || pos.y > 1)
                {
                    //Debug.Log($"right to left + {e.transform.position} to {GetPosition("left", 0)}");
                    e.transform.position = GetPosition("left", 0);
                    teleported.Add(e);
                }
            }
        }
    }

    private void TeleportLeftToRight((GameObject[] enemies, GameObject[] left, GameObject[] right, GameObject[] up, GameObject[] down) hm, float lr, List<GameObject> teleported)
    {
       // Debug.Log($"teleport left to right {lr} - {hm.left.Count()}");

        if (lr > 0.4)
        {
            foreach (var e in hm.left)
            {
                if (teleported.Exists(x => x.GetInstanceID() == e.GetInstanceID())) continue;

                //var pos = _camera.WorldToViewportPoint(e.transform.position);
                //Debug.Log($"left to right + {e.transform.position} to {GetPosition("right", 0)} - visible = {visible}");
                var visible = e.GetComponent<EnemyMovement>().IsVisible;
                if (!visible)//(pos.x > 1 || pos.y > 1)
                {
                    e.transform.position = GetPosition("right", 0);
                    teleported.Add(e);
                }
            }
        }
    }

    IEnumerator S1()
    {
        if (wave >1) yield break;

        wave++;

        _audioLevel1.loop = true;
        _audioLevel1.Play();

        for (var i = 0; i < 20; i++)
        {
            var speed = 0.4f + (i / 3f);
            var health = 1f;
            var shooterhealth = 4;
            var circlehealth = 6;
            

            Debug.Log($"wave start: {wave}-{i} , speed={speed}, health={health}");

            //yield return SpawnSwarm(10 + (i*2), speed, health, 2f);

            yield return Spawn(1, 1, 100, "top");


            yield return SmartWait("Enemy",500f);

            if (i > 4)
            {
                Spawn(1, speed, shooterhealth, "top", false, true, false);
            }

            if (i > 8)
            {
                Spawn(1, speed, shooterhealth, "bottom", false, true, false);
            }

            if (i > 12)
            {
                Spawn(1, speed, shooterhealth, "left", false, true, false);
            }

            if (i > 16)
            {
                Spawn(1, speed, shooterhealth, "right", false, true, false);
            }

            if (i % 4 == 0 && i > 0)
            {
                yield return SpawnEnemyInCircle(i/6, speed, circlehealth, 300, 100, 0.5f);
            }

            Debug.Log($"wave end: {wave}-{i}");

            //yield return SmartWait("Enemy", 20f);
        }

        _audioLevel1.Stop();

    }

    private List<GameObject> Spawn(int enemies, float speed, float health, string loc,
        bool varspeed = true, bool shoot = false, bool shootMissle = false, Vector3? pos = null)
    {
        var l = new List<GameObject>();
        for (float i = 0; i < enemies; i++)
        {
            
            float newspeed = speed;

            if (pos == null)
            {
                pos = GetPosition(loc, i);
            }

            if (varspeed)
            {
                newspeed += Random.Range(-1.5f, 1.5f);
                newspeed = Mathf.Clamp(newspeed, 1f, 6f);
            }

            l.Add(SpawnEnemy(pos.Value, newspeed, health, shoot, shootMissle));
        }

        return l;
    }

    private Vector3 GetPosition(string loc, float i)
    {
        Vector3 pos = Vector3.zero;

        if (loc == "top") pos = new Vector3(_top.position.x - i, _top.position.y, _top.position.z);
        if (loc == "left") pos = new Vector3(_left.position.x, _left.position.y + i, _left.position.z);
        if (loc == "right") pos = new Vector3(_right.position.x, _right.position.y - i, _right.position.z);
        if (loc == "bottom") pos = new Vector3(_bottom.position.x - i, _bottom.position.y, _bottom.position.z);

        return pos;
    }
   


    GameObject SpawnEnemy(Vector3 position, float speed, float health, bool shoot = false, bool shootMissle = false)
    {
        var prefab = _enemyPrefab;
        prefab.GetComponent<EnemyMovement>().enabled = true;
        var em = prefab.GetComponent<EnemyMovement>();
        var hc = prefab.GetComponent<HealthController>();
        em.SetSpeed(speed);
        em.SetShoot(shoot, shootMissle);
        hc.SetMaxHealth(health);

        
        prefab.GetComponentInChildren<SpriteRenderer>().color = Color.white;

        return Instantiate(prefab, position, Quaternion.identity);
    }


    void SpawnEnemy()
    {
        var prefab = _enemyPrefab;
        prefab.GetComponent<EnemyShooterScript>().enabled = false;
        prefab.GetComponent<EnemyMovement>().enabled = true;
        prefab.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        prefab.GetComponent<HealthController>().SetMaxHealth(10);

        Instantiate(prefab, transform.position, Quaternion.identity);
    }

    void SpawnEye()
    {
        var prefab = _eyePrefab;
        Instantiate(prefab, transform.position, Quaternion.identity);
    }



    void SpawnDog()
    {
        var prefab = _dogPrefab;
        prefab.GetComponent<EnemyMovement>().enabled = true;
        prefab.GetComponent<EnemyMovement>().SetSpeed(4.5f);
        prefab.GetComponent<HealthController>().SetMaxHealth(10);
        Instantiate(prefab, transform.position, Quaternion.identity);
    }

    void SpawnFrog()
    {
        var prefab = _frogPrefab;
        var pp = _playerTransform.position;
        var position = new Vector3(pp.x + Random.Range(-10f, +10f), pp.y + Random.Range(-10f, 10f), pp.z);
        prefab.GetComponent<HealthController>().SetMaxHealth(20);
        Instantiate(prefab, position, Quaternion.identity);
    }

    void SpawnSlime()
    {
        var prefab = _slimePrefab;
        Instantiate(prefab, transform.position, Quaternion.identity);
    }

    private void SetTimeUntilSpawn()
    {
        _timeUntilSpawn = Random.Range(_minSpawnTime, _maxSpawnTime);
    }

    void Spawn()
    {
        var rnd = Random.Range(0, 8);
        switch (rnd)
        {
            case 1:
                SpawnSlime();
                break;

            case 2:
                SpawnFrog();
                break;

            case 3:
                SpawnDog();
                break;

            case 7:
                SpawnEye();
                break;

            default:
                SpawnEnemy();
                break;
        }

        SetTimeUntilSpawn();
    }
}
