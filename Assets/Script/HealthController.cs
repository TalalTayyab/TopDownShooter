using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maximumHealth;
    [SerializeField] private GameObject _damagePopupPrefab;

    public UnityEvent OnDied;
    public UnityEvent OnDamage;
    public UnityEvent OnHealthChange;

    public float RemainingHealthPercentage
    {
        get
        {
            float v = _currentHealth / ((float)_maximumHealth);
            return v;
        }
    }

    public int CurrentHealth => _currentHealth;

    public bool IsInvicible { get; set; }

    public void TakeDamage(int damageAmount, bool isCriticalHit)
    {
        if (_currentHealth < 0)
        {
            return;
        }

        if (IsInvicible)
        {
            return;
        }

        _currentHealth -= damageAmount; 
        OnHealthChange.Invoke();

        DamagePopUp(transform.position, damageAmount, isCriticalHit);

        if (_currentHealth <0)
        {
            _currentHealth = 0;
        }

        if (_currentHealth == 0)
        {
            OnDied.Invoke();
        }
        else
        {
            OnDamage.Invoke();
        }
    }

    private void DamagePopUp(Vector3 position, int damageAmount, bool isCriticalHit)
    {
        var dp = Instantiate(_damagePopupPrefab, position, Quaternion.identity);
        dp.GetComponent<DamagePopUpScript>().Setup(damageAmount, isCriticalHit);
    }

    public void AddHealth(int amountToAdd)
    {
        if (_currentHealth == _maximumHealth)
        {
            return;
        }

        _currentHealth += amountToAdd;
        OnHealthChange.Invoke();

        if (_currentHealth > _maximumHealth)
        {
            _currentHealth = _maximumHealth;
        }
    }

    public void SetMaxHealth(int maxHealth)
    {
        _currentHealth = _maximumHealth = maxHealth;
    }
}
