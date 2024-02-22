using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int Location;
    private Vector3 _delta;
    private Transform _playerTransform;
   

    // Start is called before the first frame update
    void Start()
    {
       
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

}
