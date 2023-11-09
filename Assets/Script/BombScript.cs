using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class BombScript : MonoBehaviour
{
    [SerializeField] private float _autoDestoryTime;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private float _explosionRadius;
    private Camera _camera;
    private float _time;
    private bool _destroyed;

    // Start is called before the first frame update
    void Start()
    {
        //var direction = transform.right + Vector3.up;
        //GetComponent<Rigidbody2D>().AddForce(direction * _speed, ForceMode2D.Impulse);
        _camera = Camera.main;
        _time = _autoDestoryTime;
        _destroyed = false;
    }

    private void Destroy()
    {
        var explosion = Instantiate(_explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
        _destroyed = true;
        Destroy(explosion, 1);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<EnemyMovement>();
        if (enemy != null)
        {
            Explode();
        }

    }

    private void Explode()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<EnemyMovement>())
            {
                var healthController = collider.GetComponent<HealthController>();
                healthController.TakeDamage(30);
            }
        }
        Destroy();
    }

    private void FixedUpdate()
    {
        DestoryWhenOffScreen();
        _time -= Time.deltaTime;


        if (_time <= 0 && _destroyed == false)
        {
            Explode();
        }


    }

    private void DestoryWhenOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);
        if (screenPosition.x < 0 || screenPosition.x > _camera.pixelWidth || screenPosition.y < 0 || screenPosition.y > _camera.pixelHeight)
        {
            Explode();
        }
    }
}
