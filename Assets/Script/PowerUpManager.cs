using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PowerUpManagerFactory
{
    static PowerUpManager _powerupManager;
    static PowerUpManagerFactory() { _powerupManager = new PowerUpManager(); }
    public static PowerUpManager PowerUpManager => _powerupManager;

    public static void NewClass()
    {
        _powerupManager = new PowerUpManager();
    }
}

public class PowerUpManager 
{
   
    private Dictionary<string, List<PowerUp>> availablePowerUps = new Dictionary<string, List<PowerUp>>();

    private string[] categories = new[] { "Bullet", "Player" , "Misc" };

    public PowerUpManager()
    {
        availablePowerUps.Add("Bullet",
            new List<PowerUp> {
                new BulletSpeed(),
                new BulletDamage(),
                new BulletDistance(),
                new BulletCD(),
                new BulletCriticalDamage(),
                new BulletCriticalDamageChance()
            });

        availablePowerUps.Add("Player",
            new List<PowerUp> {
                new PlayerHealth(),
                new PlayerSpeed(),
                new PlayerDashCD(),
                new PlayerDashSpeed(),
                new PlayerHealthRecharge(),
                new PlayerHealthRechargeCD()
            });

        availablePowerUps.Add("Misc",
            new List<PowerUp> {
                new CoinPickupRange()
            });
    }


    public List<PowerUp> GetPowerUps()
    {
        List<PowerUp> powerUpsRet = new List<PowerUp>();
        foreach (var category in categories)
        {
            var powerUps = availablePowerUps[category];
            powerUpsRet.Add(GetPowerUpsForCategory(powerUps));
        }

        return powerUpsRet;
    }


    private int GetWeightedRandomPowerUp(List<PowerUp> powerUps)
    {
        int[] weights = powerUps.Select(x=>x.Weight).ToArray();
        int randomWeight = UnityEngine.Random.Range(0, weights.Sum());
        for (int i = 0; i < weights.Length; ++i)
        {
            randomWeight -= weights[i];
            if (randomWeight < 0)
            {
                Debug.Log($"found {i} - {powerUps[i].ID}");
                return i;
            }
        }

        Debug.Log($"none found - using default");
        return Random.Range(0, powerUps.Count);
    }

    private PowerUp GetPowerUpsForCategory(List<PowerUp> powerUps)
    {
        int index = GetWeightedRandomPowerUp(powerUps);
        return powerUps[index];
    }

    private float BulletV(string item)
    {
        return availablePowerUps["Bullet"].FirstOrDefault(x => x.ID == item).Value;
    }
    private float PlayerV(string item)
    {
        return availablePowerUps["Player"].FirstOrDefault(x => x.ID == item).Value;
    }

    public float BulletSpeed => BulletV("BulletSpeed");
    public float BulletDamage => BulletV("BulletDamage");
    public float BulletCriticalDamage => BulletV("BulletCriticalDamage");
    public float BulletDistance => BulletV("BulletDistance");
    public float BulletCD => BulletV("BulletCD");
    public float BulletCriticalDamageChance => BulletV("BulletCriticalDamageChance");

    public float PlayerSpeed => PlayerV("PlayerSpeed");
    public float PlayerHealth => PlayerV("PlayerHealth");
    public float PlayerDashCD => PlayerV("PlayerDashCD");
    public float PlayerDashSpeed => PlayerV("PlayerDashSpeed");
    public float PlayerHealthRecharge => PlayerV("PlayerHealthRecharge");
    public float PlayerHealthRechargeCD => PlayerV("PlayerHealthRechargeCD");

    public float CoinPickupRange => availablePowerUps["Misc"].FirstOrDefault(x => x.ID == "CoinPickupRange").Value;

}


public abstract class PowerUp
{
    public string ID { get; set; }
    public float Value { get; set; }
    public string Text { get; protected set; }
    public bool IsSelected { get; private set; }
    public int Weight { get; protected set; } = 10;

    protected bool _decrease;

    public virtual void Selected()
    {
        IsSelected = true;
        Weight += Weight;

        if (_decrease)
        {
            Value -= Value * 0.1f;
            if (Value < 0) Value = 0;
            return;
        }
        if (Value == 0) Value = 0.1f; else Value += Value * 0.1f;
    }

    public abstract void Init();

    public PowerUp()
    {
        Init();
    }

}

public class BulletSpeed : PowerUp { public override void Init() { ID = "BulletSpeed";  Value = 10;  Text = "Increase bullet speed"; } }

public class BulletDamage : PowerUp { public override void Init() { ID = "BulletDamage"; Value = 1;  Text = "Increase bullet damage"; } }

public class BulletCriticalDamage : PowerUp { public override void Init() { ID = "BulletCriticalDamage"; Value = 3; Text = "Increase bullet critical damage"; } }

public class BulletDistance : PowerUp { public override void Init() { ID = "BulletDistance"; Value = 0.5f; Text = "Increase bullet distance"; } }

public class BulletCD : PowerUp { public override void Init() { ID = "BulletCD"; Value = 2f; Text = "Decrease bullet cooldown"; _decrease = true; } }

public class BulletCriticalDamageChance : PowerUp {
    public override void Init() { ID = "BulletCriticalDamageChance"; Value = 20f; Text = "Increase bullet critical damage chance"; }
    public override void Selected() { Value = Value + 3; }
}


public class PlayerHealth : PowerUp { public override void Init() { ID = "PlayerHealth"; Value = 20;  Text = "Increase player health"; } }

public class PlayerSpeed : PowerUp { public override void Init() { ID = "PlayerSpeed"; Value = 2.5f; Text = "Increase player speed"; } }

public class PlayerDashCD : PowerUp { public override void Init() { ID = "PlayerDashCD"; Value = 3f; Text = "Decrease dash cooldown"; _decrease = true; } }

public class PlayerDashSpeed : PowerUp { public override void Init() { ID = "PlayerDashSpeed"; Value = 30f; Text = "Increase dash speed"; } }

public class PlayerHealthRecharge : PowerUp { public override void Init() { ID = "PlayerHealthRecharge"; Value = 0.0f; Text = "Increase health recharge"; } }

public class PlayerHealthRechargeCD : PowerUp { public override void Init() { ID = "PlayerHealthRechargeCD"; Value = 3f; Text = "Decrease health recharge cd"; _decrease = true; } }


public class CoinPickupRange : PowerUp { public override void Init() { ID = "CoinPickupRange"; Value = 2.1f; Text = "Increase coin pickup range"; } }




