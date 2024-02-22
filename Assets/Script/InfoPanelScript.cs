using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPanelScript : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    private GameManagerScript gameManagerScript;

    private void Awake()
    {
        gameManagerScript = FindObjectOfType<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        var p = PowerUpManagerFactory.PowerUpManager;
        var bullet = $"Bullet - cooldown={p.BulletCD}, critdmg={p.BulletCriticalDamage}, crit%={p.BulletCriticalDamageChance}, dmg={p.BulletDamage}," +
            $" dist={p.BulletDistance}, speed={p.BulletSpeed}";

        var level = $"Level={gameManagerScript._level}, Coins={gameManagerScript.CoinsRequiredForLevelling}, PickupRange={p.CoinPickupRange}";

        var player = $"Player - speed={p.PlayerSpeed}, health={p.PlayerHealth}, dashcd={p.PlayerDashCD}, dashspeed={p.PlayerDashSpeed}, healthrecharge={p.PlayerHealthRecharge}, healthrechargecd={p.PlayerHealthRechargeCD}";

        _text.text = bullet + Environment.NewLine + level + Environment.NewLine + player + Environment.NewLine;
    }
}
