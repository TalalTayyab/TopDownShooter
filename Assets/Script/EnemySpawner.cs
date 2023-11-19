using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;
    [SerializeField] private GameObject _misslePrefab;
    [SerializeField] private int Location;
   
    private Transform _playerTransform;
    private float _timeUntilSpawn;
    


    // Start is called before the first frame update
    void Start()
    {
        SetTimeUntilSpawn();
        _playerTransform = FindObjectOfType<Player>().transform;
        //  _delta = transform.position - _playerTransform.transform.position;
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
            transform.position = new Vector3(0, pos.y+1, transform.position.z);
        }
        if (Location == 3) //bottom-mid
        {
            var pos = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, transform.position.z));
            transform.position = new Vector3(0, (pos.y*-1)- 1, transform.position.z);
        }

    }

    //private void FixedUpdate()
    //{
    // move with player
    //    transform.position = new Vector3(_playerTransform.position.x + _delta.x, _playerTransform.position.y + _delta.y, transform.position.z);

    //}

    // Update is called once per frame
    void Update()
    {
        _timeUntilSpawn -= Time.deltaTime;

        if (_timeUntilSpawn <= 0)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        var position = transform.position;
        var prefab = _enemyPrefab;
        prefab.GetComponent<EnemyShooterScript>().enabled = false;
        prefab.GetComponent<EnemyMovement>().enabled = true;
        prefab.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        prefab.GetComponent<HealthController>().SetMaxHealth(10);

        var rnd = Random.Range(0, 8);
        if (rnd == 3)
        {
            prefab.GetComponent<EnemyMovement>().enabled = false;
            prefab.GetComponent<EnemyShooterScript>().enabled = true;
            prefab.GetComponentInChildren<SpriteRenderer>().color = Color.red;
            prefab.GetComponent<HealthController>().SetMaxHealth(Random.Range(20, 60));
        }

        if (rnd == 5)
        {
            prefab = _misslePrefab;
            position = _playerTransform.transform.position;
        }

        Instantiate(prefab, position, Quaternion.identity);

        SetTimeUntilSpawn();
    }

    private void SetTimeUntilSpawn()
    {
        _timeUntilSpawn = Random.Range(_minSpawnTime, _maxSpawnTime);
    }
}
