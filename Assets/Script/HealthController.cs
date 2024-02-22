using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maximumHealth;
    [SerializeField] private GameObject _damagePopupPrefab;

    public UnityEvent OnDied;
    public UnityEvent OnDamage;
    public UnityEvent OnHealthChange;

    public bool isPlayerHealth;
    private float _prevMaxHealth;


    private void Awake()
    {
        _prevMaxHealth = _currentHealth = _maximumHealth;
    }

    public float RemainingHealthPercentage
    {
        get
        {
            float v = _currentHealth / _maximumHealth;
            return v;
        }
    }

    public float CurrentHealth => _currentHealth;
    public float MaximumHealth => _maximumHealth;

    public bool IsInvicible { get; set; }

    public void TakeDamage(float damageAmount, bool isCriticalHit)
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

        DamagePopUp(transform.position, damageAmount, isCriticalHit, false);

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

    private void DamagePopUp(Vector3 position, float amount, bool isCriticalHit, bool isHealthIncrease)
    {
        var dp = Instantiate(_damagePopupPrefab, position, Quaternion.identity);
        dp.GetComponent<DamagePopUpScript>().Setup(amount, isCriticalHit, isHealthIncrease);
    }

    public void AddHealth(float amountToAdd)
    {
       // Debug.Log($"c={_currentHealth},m={_maximumHealth},a={amountToAdd}");
        if (_currentHealth == _maximumHealth)
        {
            return;
        }

        if (amountToAdd <= 0) return;

        _currentHealth += amountToAdd;
        OnHealthChange.Invoke();

        if (_currentHealth > _maximumHealth)
        {
            _currentHealth = _maximumHealth;
        }

        if (isPlayerHealth)
            DamagePopUp(transform.position, amountToAdd, false, true);
    }

    public void SetMaxHealth(float maxHealth)
    {
        _maximumHealth = maxHealth;

        var diff = _maximumHealth - _prevMaxHealth;

        _prevMaxHealth = _maximumHealth;

        AddHealth(diff);
    }
}
