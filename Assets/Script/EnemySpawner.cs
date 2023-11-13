using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;
    [SerializeField] private GameObject _misslePrefab;
    private Transform _playerTransform;
    //private Vector3 _delta;
    private float _timeUntilSpawn;
    // Start is called before the first frame update
    void Start()
    {
        SetTimeUntilSpawn();
        _playerTransform = FindObjectOfType<Player>().transform;
        //  _delta = transform.position - _playerTransform.transform.position;
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
        var position = transform.position;

        if (_timeUntilSpawn <= 0)
        {
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
                prefab.GetComponent<HealthController>().SetMaxHealth(Random.Range(20,60));
            }

            if (rnd == 5)
            {
                prefab = _misslePrefab;
                position = _playerTransform.transform.position;
            }

            Instantiate(prefab, position, Quaternion.identity);
            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        _timeUntilSpawn = Random.Range(_minSpawnTime, _maxSpawnTime);
    }
}
