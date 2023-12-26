using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _slimePrefab;
    [SerializeField] private GameObject _dogPrefab;
    [SerializeField] private GameObject _frogPrefab;
    [SerializeField] private GameObject _eyePrefab;
    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;
    [SerializeField] private GameObject _misslePrefab;
    [SerializeField] private int Location;

    private Transform _playerTransform;
    private float _timeUntilSpawn;
    private Vector3 _delta;



    // Start is called before the first frame update
    void Start()
    {
        SetTimeUntilSpawn();
        _playerTransform = FindObjectOfType<Player>().transform;
        if (Location == 0) //left-mid
        {
            var pos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, transform.position.z));
            transform.position = new Vector3(pos.x - 1, 0, transform.position.z);
        }
        if (Location == 1) //right-mid
        {
            var pos = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, transform.position.z));
            transform.position = new Vector3(pos.x + 1, 0, transform.position.z);
        }
        if (Location == 2) //top-mid
        {
            var pos = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, transform.position.z));
            transform.position = new Vector3(0, pos.y + 1, transform.position.z);
        }
        if (Location == 3) //bottom-mid
        {
            var pos = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, transform.position.z));
            transform.position = new Vector3(0, (pos.y * -1) - 1, transform.position.z);
        }
        _delta = transform.position - _playerTransform.transform.position;

    }

    private void FixedUpdate()
    {
        // move with player
        transform.position = new Vector3(_playerTransform.position.x + _delta.x, _playerTransform.position.y + _delta.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        _timeUntilSpawn -= Time.deltaTime;

        if (_timeUntilSpawn <= 0)
        {
            Spawn();
        }
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

    void SpawnMissle()
    {
        var prefab = _misslePrefab;
        var position = _playerTransform.transform.position;
        Instantiate(prefab, position, Quaternion.identity);
    }

    void SpawnDog()
    {
        var prefab = _dogPrefab;
        prefab.GetComponent<EnemyMovement>().enabled = true;
        prefab.GetComponent<EnemyMovement>()._speed = 4.5f;
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

            case 5:
                SpawnMissle();
                break;

            default:
                SpawnEnemy();
                break;
        }

        SetTimeUntilSpawn();
    }

    private void SetTimeUntilSpawn()
    {
        _timeUntilSpawn = Random.Range(_minSpawnTime, _maxSpawnTime);
    }
}
