using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private int _damageAmount;

    public UnityEvent OnEnterCloseAttack;
    public UnityEvent OnLeaveCloseAttack;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            OnEnterCloseAttack?.Invoke();

            var healthController = collision.gameObject.GetComponent<HealthController>();
            healthController.TakeDamage(_damageAmount, false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        OnLeaveCloseAttack?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            OnEnterCloseAttack?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnLeaveCloseAttack?.Invoke();
    }
}
