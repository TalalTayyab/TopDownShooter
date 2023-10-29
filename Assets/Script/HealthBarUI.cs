using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image _image;
    public void UpdateHealthBar(HealthController healthController)
    {
        _image.fillAmount = healthController.RemainingHealthPercentage;
    }
}
