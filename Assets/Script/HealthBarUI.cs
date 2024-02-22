using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image _image;
    [SerializeField] private HealthController _healthController;
    private TextMeshProUGUI _healthTextPro;

    private void Awake()
    {
        _healthTextPro = GetComponent<TextMeshProUGUI>();
        _healthTextPro.SetText($"Health:{Mathf.Round(_healthController.CurrentHealth)}/{Mathf.Round(_healthController.MaximumHealth)}");
    }
    public void UpdateHealthBar(HealthController healthController)
    {
        
    }

    private void FixedUpdate()
    {
        //_image.fillAmount = _healthController.RemainingHealthPercentage;
        _healthTextPro.SetText($"Health:{Mathf.Round(_healthController.CurrentHealth)}/{Mathf.Round(_healthController.MaximumHealth)}");
    }
}
