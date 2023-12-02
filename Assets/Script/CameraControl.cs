using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] Transform _playerPosition;
    [SerializeField] Transform _bg1;
    [SerializeField] Transform _bg2;
    [SerializeField] Transform _bg3;
    [SerializeField] Transform _bg4;
    private Vector2 _size;
    // Start is called before the first frame update
    void Start()
    {

        //_size = _bg1.GetComponent<BoxCollider2D>().size;
        //_size *= _bg1.GetComponent<Transform>().localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var targetPosition = new Vector3(_playerPosition.position.x, _playerPosition.position.y, transform.position.z);
        transform.position = targetPosition;// Vector3.Lerp(transform.position, targetPosition, 0.2f);
        
        //if (transform.position.y >= _bg2.position.y )
        //{
        //    _bg1.position = new Vector3(_bg1.position.x, _bg2.position.y + _size.y, _bg1.position.z);
        //    Swap(ref _bg1,ref _bg2);

        //    _bg3.position = new Vector3(_bg3.position.x, _bg1.position.y + _size.y, _bg3.position.z);
        //    Swap(ref _bg3, ref _bg4);

        //}

        //if (transform.position.y < _bg1.position.y)
        //{
        //    _bg2.position = new Vector3(_bg2.position.x, _bg1.position.y - _size.y, _bg2.position.z);
        //    Swap(ref _bg1, ref _bg2);

        //    _bg4.position = new Vector3(_bg4.position.x, _bg2.position.y - _size.y, _bg4.position.z);
        //    Swap(ref _bg3, ref _bg4);
        //}

        //if (transform.position.x >= _bg3.position.x)
        //{
        //    _bg1.position = new Vector3(_bg3.position.x + _size.x, _bg1.position.y, _bg1.position.z);
        //    Swap(ref _bg1, ref _bg3);

        //    _bg2.position = new Vector3(_bg1.position.x + _size.x, _bg2.position.y, _bg2.position.z);
        //    Swap(ref _bg2, ref _bg4);
        //}

        //if (transform.position.x < _bg1.position.x)
        //{
        //    _bg3.position = new Vector3(_bg1.position.x - _size.x, _bg3.position.y, _bg3.position.z);
        //    Swap(ref _bg1, ref _bg3);

        //    _bg4.position = new Vector3(_bg3.position.x - _size.x, _bg4.position.y, _bg4.position.z);
        //    Swap(ref _bg4, ref _bg2);
        //}
    }

    void Swap(ref Transform a, ref Transform b)
    {
        var temp = a;
        a = b;
        b = temp;
    }
}
