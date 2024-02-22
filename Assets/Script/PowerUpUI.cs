using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUpUI : MonoBehaviour
{
    [SerializeField] TMP_Text powerUp1;
    [SerializeField] TMP_Text powerUp2;
    [SerializeField] TMP_Text powerUp3;

    private List<PowerUp> _powerUps;

    public bool IsShowing { get; set; }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(List<PowerUp> powerUps)
    {
        if (IsShowing) return;

        IsShowing = true;
        Time.timeScale = 0;
        _powerUps = powerUps;

        powerUp1.text= powerUps[0].Text;
        powerUp2.text = powerUps[1].Text;
        powerUp3.text = powerUps[2].Text;

        gameObject.SetActive(true);
    }

    public void PowerUp1Clicked()
    {
        PowerUpSelected(_powerUps[0]);
    }

    public void PowerUp2Clicked()
    {
        PowerUpSelected(_powerUps[1]);
    }

    public void PowerUp3Clicked()
    {
        PowerUpSelected(_powerUps[2]);
    }

    private void PowerUpSelected(PowerUp powerUp)
    {
        powerUp.Selected();
        Hide();
    }
    

    public void Hide()
    {
        if (!IsShowing) return;

        IsShowing = false;
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
