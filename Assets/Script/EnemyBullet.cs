using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private int _damageAmount;
    [SerializeField] private GameObject _rectangle;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float StretchY;
    private Camera _camera;
    private Vector3 _target;
    private bool _warning;
    private float _bulletSpeed;

    public void Setup(Vector3 target, float bulletSpeed)
    {
        _target = target;
        _bulletSpeed = bulletSpeed;

    }

    private void Start()
    {
        _camera = Camera.main;
    }

   
    private IEnumerator ShowWarning()
    {
        if (_warning) yield break;
        _bullet.SetActive(false);
        _warning = true;
        var currentPosition = transform.position;
        while (currentPosition != _target)
        {
            currentPosition = Vector3.MoveTowards(currentPosition, _target, 30*Time.deltaTime);
            Strech(_rectangle, transform.position, currentPosition);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        _bullet.SetActive(true);
        Destroy(_rectangle);
        var ridigBody2d = GetComponent<Rigidbody2D>();
        ridigBody2d.velocity = transform.up * _bulletSpeed;
    }

    
    public void Strech(GameObject _sprite, Vector3 _initialPosition, Vector3 _finalPosition)
    {
        Vector3 centerPos = (_initialPosition + _finalPosition) / 2f;
        _sprite.transform.position = centerPos;
        Vector3 direction = _finalPosition - _initialPosition;
        direction = Vector3.Normalize(direction);
        _sprite.transform.right = direction;
        Vector3 scale = new Vector3(1, 20, 0);
        scale.x = Vector3.Distance(_initialPosition, _finalPosition) * 11;
        _sprite.transform.localScale = scale ;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            var healthController = collision.GetComponent<HealthController>();
            healthController.TakeDamage(_damageAmount, false);
            Destroy(gameObject);
        }
    }

    private void DestoryWhenOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);
        if (screenPosition.x < 0 || screenPosition.x > _camera.pixelWidth || screenPosition.y < 0 || screenPosition.y > _camera.pixelHeight)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        StartCoroutine(ShowWarning());
        DestoryWhenOffScreen();
    }

}
