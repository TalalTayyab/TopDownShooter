using System.Collections;
using System.Collections.Generic;
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

    private Transform _playerTransform;
    private float _timeUntilSpawn;
    private GameTimeScript _gameTimeScript;
    private int wave = 1;
    public int _level = 1;
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
         
    }

    void PowerUpHandler()
    {
        _experienceTracker.UpdateExperience(_coins.LevelCoins, CoinsRequiredForLevelling);
        if (_coins.LevelCoins % CoinsRequiredForLevelling == 0 && _coins.LevelCoins > 0)
        {
            _powerUpUI.Show(PowerUpManager.GetPowerUps());
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

    private void Waves()
    {
        if (_gameTimeScript.Seconds >= 1)
        {
            StartCoroutine(S1());
        }
    }


    IEnumerator S1()
    {
        if (wave >1) yield break;

        wave++;

        for (var i = 0; i < 10; i++)
        {
            Spawn(1, 2, 3, "top", false, true, false);

            Spawn(1, 2+(i/4), 1+i, "left");

            yield return new WaitForSeconds(3f);

            Spawn(1, 2 + (i / 4), 1+i, "top");

            yield return new WaitForSeconds(3f);

            Spawn(4, 2 + (i / 4), 1+i, "bottom");

            yield return new WaitForSeconds(3f);

            Spawn(2, 3 + (i / 4), 1+i, "right");

            yield return new WaitForSeconds(10f);


        }
        
    }

    private void Spawn(int enemies, float speed, int health, string loc, bool varspeed = true, bool shoot = false, bool shootMissle = false)
    {
        for (float i = 0; i < enemies; i++)
        {
            var pos = new Vector3();
            float newspeed = speed;

            if (loc=="top") pos = new Vector3(_top.position.x -i, _top.position.y, _top.position.z);
            if (loc=="left") pos = new Vector3(_left.position.x, _left.position.y +i, _left.position.z);
            if (loc=="right") pos = new Vector3(_right.position.x, _right.position.y-i, _right.position.z);
            if (loc=="bottom") pos = new Vector3(_bottom.position.x -i , _bottom.position.y, _bottom.position.z);

            if (varspeed)
            {
                newspeed += i  / enemies ;
            }

            SpawnEnemy(pos, newspeed, health, shoot, shootMissle);
        }
    }
   


    void SpawnEnemy(Vector3 position, float speed, int health, bool shoot = false, bool shootMissle = false)
    {
        var prefab = _enemyPrefab;
        prefab.GetComponent<EnemyMovement>().enabled = true;
        var em = prefab.GetComponent<EnemyMovement>();
        var hc = prefab.GetComponent<HealthController>();
        em.SetSpeed(speed);
        em.SetShoot(shoot, shootMissle);
        hc.SetMaxHealth(health);

        
        prefab.GetComponentInChildren<SpriteRenderer>().color = Color.white;

        Instantiate(prefab, position, Quaternion.identity);
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
