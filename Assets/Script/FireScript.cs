using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    [SerializeField] private int _damageAmount;
    private bool _insideFire;
    private GameObject _gameObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            _gameObject = collision.gameObject;
            _insideFire = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            _insideFire = false;
        }
    }

    private void FixedUpdate()
    {
        if (_insideFire && _gameObject.GetComponent<Player>())
        {
            var healthController = _gameObject.GetComponent<HealthController>();
            healthController.TakeDamage(_damageAmount, false);
        }
    }
}
