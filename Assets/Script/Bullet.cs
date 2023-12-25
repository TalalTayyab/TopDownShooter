using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _autoDestoryTime;
    [SerializeField] private GameObject _damagePrefab;
    private Camera _camera;
    
    private void Awake()
    {
        _camera = Camera.main;
        Destroy(gameObject, _autoDestoryTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))

        {
            var healthController = collision.GetComponent<HealthController>();
            var damage = Random.Range(3, 10);
            var isCriticalHit = IsCriticalHit(ref damage);
            healthController.TakeDamage(damage);
            Destroy(gameObject);

            DamagePopUp(collision.transform.position, damage, isCriticalHit);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Trees"))
        {
            Destroy(gameObject);
        }
    }

    private bool IsCriticalHit(ref int damage)
    {
        var range = Random.Range(1, 6);
        if (range == 3)
        {
            var modifier = Random.Range(3, 10);
            damage = damage + modifier;
            return true;
        }

        return false;
    }

    private void DamagePopUp(Vector3 position, int damageAmount, bool isCriticalHit)
    {
        var dp = Instantiate(_damagePrefab, position, Quaternion.identity);
        dp.GetComponent<DamagePopUpScript>().Setup(damageAmount, isCriticalHit);
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
