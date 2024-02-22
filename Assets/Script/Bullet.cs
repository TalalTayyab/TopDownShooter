using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   // [SerializeField] private float _autoDestoryTime;
    [SerializeField] private GameObject _damagePrefab;
    private Camera _camera;
    
    private void Awake()
    {
        _camera = Camera.main;
        Destroy(gameObject, PowerUpManagerFactory.PowerUpManager.BulletDistance);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))

        {
            var healthController = collision.GetComponent<HealthController>();
            var damage = PowerUpManagerFactory.PowerUpManager.BulletDamage;
            var isCriticalHit = IsCriticalHit(ref damage);
            healthController.TakeDamage(damage, isCriticalHit);
            Destroy(gameObject);

        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Trees"))
        {
            Destroy(gameObject);
        }
    }

    private bool IsCriticalHit(ref float damage)
    {
        var randomValue = Random.Range(1,101);
        var critp = PowerUpManagerFactory.PowerUpManager.BulletCriticalDamageChance;
        if (randomValue < critp)
        {
            var modifier = Random.Range(PowerUpManagerFactory.PowerUpManager.BulletDamage, PowerUpManagerFactory.PowerUpManager.BulletCriticalDamage);
            damage = damage + modifier;
            return true;
        }

        return false;
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
