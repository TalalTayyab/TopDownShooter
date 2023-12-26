using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image _image;
    [SerializeField] private GameObject _healthTextGameObject;
    [SerializeField] private HealthController _healthController;
    private TextMeshProUGUI _healthTextPro;

    private void Awake()
    {
        _healthTextPro = _healthTextGameObject.GetComponent<TextMeshProUGUI>();
        _healthTextPro.SetText(_healthController.CurrentHealth.ToString());
    }
    public void UpdateHealthBar(HealthController healthController)
    {
        _image.fillAmount = healthController.RemainingHealthPercentage;
        _healthTextPro.SetText(healthController.CurrentHealth.ToString());
    }
}
