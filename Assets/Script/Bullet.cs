using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _autoDestoryTime;
    private Camera _camera;
    
    private void Awake()
    {
        _camera = Camera.main;
        Destroy(gameObject, _autoDestoryTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyMovement>())
        {
            var healthController = collision.GetComponent<HealthController>();
            healthController.TakeDamage(5);
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Trees"))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        
        DestoryWhenOffScreen();
    }

    private void DestoryWhenOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);
        if (screenPosition.x <0 || screenPosition.x > _camera.pixelWidth || screenPosition.y < 0 || screenPosition.y > _camera.pixelHeight)
        {
            Destroy(gameObject);
        }
    }
}
