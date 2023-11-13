using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserChargeScript : MonoBehaviour
{
    [SerializeField] Slider _laserChargeSlider;
    public void UpdateCharge(Shoot shoot)
    {
        _laserChargeSlider.value = shoot.RemainingCharge;
    }

}
