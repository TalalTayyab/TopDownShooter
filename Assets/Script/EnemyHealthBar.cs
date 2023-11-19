using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] Slider _slider;
    public void UpdateCharge(HealthController healthController)
    {
        _slider.value = healthController.RemainingHealthPercentage;
    }

    public void Destroyed()
    {
        Destroy(gameObject);
    }
}
