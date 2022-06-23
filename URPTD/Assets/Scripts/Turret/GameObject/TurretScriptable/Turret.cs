using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Turret", menuName = "Turret", order = 1)]
public class Turret : ScriptableObject
{
    #region Variables


    public Sprite icon;
    public TurretBehaviour TurretBehaviour;
    //v________________________________________________________________________________________________________________________________________________________
    [Header("Base stats")]
    [SerializeField] float damage;
    [SerializeField] float fireRate, fireRange, rotationSpeed, maxHealth, healthRegen, critRate, critDamage;
    float health;
    [SerializeField] TurretType type;
    [SerializeField] TurretTier headType;
    [SerializeField] BulletType bulletType;


    [Header("Base costs")]
    [SerializeField] int damageCost;
    [SerializeField] int fireRateCost, fireRangeCost, rotationSpeedCost, maxHealthCost, healthRegenCost, critRateCost, critDamageCost;

    [Header("Base modifiers")]
    [SerializeField] float modifier;

    [Header("LEVEL SYSTEM")]
    [SerializeField] int level, exp, requiredExp;

    [Header("Base cost modifiers")]
    [SerializeField] float costModifier;


    int upgradeAmount;


    //V________________________________________________________________________________________________________________________________________________________
    [SerializeField] float damageOutput, fireRateOutput;
    public float Damage => damageOutput;
    public float FireRate => fireRateOutput;
    public float FireRange => fireRange;
    public float RotationSpeed => rotationSpeed;
    public float CritRate => critRate;
    public float CritDamage => critDamage;
    public float Health => health;
    public float MaxHealth => maxHealth;
    public float HealthRegen => healthRegen;

    public int DamageCost => damageCost;
    public int FireRateCost => fireRateCost;
    public int FireRangeCost => fireRangeCost;
    public int RotationSpeedCost => rotationSpeedCost;
    public int CritRateCost => critRateCost;
    public int CritDamageCost => critDamageCost;
    public int MaxHealthCost => maxHealthCost;
    public int HealthRegenCost => healthRegenCost;

    public TurretType Type => type;
    public TurretTier HeadType => headType;
    public BulletType BulletType => bulletType;
    public float Modifier => modifier;
    public float CostModifier => costModifier;
    public int Level => level;
    public int Exp => exp;
    public int RequiredExp => requiredExp;

    public int UpgradeAmount => upgradeAmount;

    #endregion
    //F________________________________________________________________________________________________________________________________________________________

    public void TakeDamage(float damage) => health -= damage;

    public void UpdateGameUI()
    {
        GameManager.Instance.GameUIManager.UpdateCashShop();
        GameManager.Instance.GameUIManager.UpdateUpgradeBtn();
    }

    #region CashUpgrades

    public void BuyDamageBoost()
    {
        if (!CheckIfUpgradePossible(2))
            return;
        if (GameManager.Instance.CashAmount < damageCost)
            return;

        damage *= modifier;
        damageOutput *= modifier;
        GameManager.Instance.CashAmount -= damageCost;
        damageCost *= (int)costModifier;
        upgradeAmount++;
        upgradeCount[2]++;
        UpdateGameUI();
    }

    public void BuyFireRateBoost()
    {
        if (!CheckIfUpgradePossible(3))
            return;
        if (GameManager.Instance.CashAmount < fireRateCost)
            return;

        fireRate *= modifier;
        fireRateOutput *= modifier;
        GameManager.Instance.CashAmount -= fireRateCost;
        fireRateCost *= (int)costModifier;
        upgradeAmount++;
        upgradeCount[3]++;
        UpdateGameUI();
    }

    public void BuyFireRangeBoost()
    {
        if (!CheckIfUpgradePossible(4))
            return;
        if (GameManager.Instance.CashAmount < fireRangeCost)
            return;
        fireRange += 0.8f;
        GameManager.Instance.CashAmount -= fireRangeCost;
        fireRangeCost *= (int)costModifier;
        upgradeAmount++;
        upgradeCount[4]++;

        if (type == TurretType.orbital)
            TurretBehaviour.AddRangeOrbital();
        UpdateGameUI();
    }

    public void BuyRotationSpeedBoost()
    {
        if (!CheckIfUpgradePossible(7))
            return;
        if (GameManager.Instance.CashAmount < rotationSpeedCost)
            return;

        rotationSpeed *= modifier;
        GameManager.Instance.CashAmount -= rotationSpeedCost;
        rotationSpeedCost *= (int)costModifier;
        upgradeAmount++;
        upgradeCount[7]++;
        UpdateGameUI();
    }

    public void BuyCritRateBoost()
    {
        if (!CheckIfUpgradePossible(5))
            return;
        if (GameManager.Instance.CashAmount < critRateCost)
            return;

        critRate *= modifier;
        GameManager.Instance.CashAmount -= critRateCost;
        critRateCost *= (int)costModifier;
        upgradeAmount++;
        upgradeCount[5]++;
        UpdateGameUI();
    }

    public void BuyCritDamageBoost()
    {
        if (!CheckIfUpgradePossible(6))
            return;
        if (GameManager.Instance.CashAmount < critDamageCost)
            return;

        critDamage *= modifier;
        GameManager.Instance.CashAmount -= critDamageCost;
        critDamageCost *= (int)costModifier;
        upgradeAmount++;
        upgradeCount[6]++;
        UpdateGameUI();
    }

    public void BuyMaxHealthBoost()
    {
        if (!CheckIfUpgradePossible(0))
            return;
        if (GameManager.Instance.CashAmount < maxHealthCost)
            return;

        maxHealth *= modifier;
        GameManager.Instance.CashAmount -= maxHealthCost;
        maxHealthCost *= (int)costModifier;
        upgradeAmount++;
        upgradeCount[0]++;
        UpdateGameUI();

    }

    public void BuyHealthRegenBoost()
    {
        if (!CheckIfUpgradePossible(1))
            return;
        if (GameManager.Instance.CashAmount < healthRegenCost)
            return;

        healthRegen *= modifier;
        GameManager.Instance.CashAmount -= healthRegenCost;
        healthRegenCost *= (int)costModifier;
        upgradeAmount++;
        upgradeCount[1]++;
        UpdateGameUI();

    }
    #endregion

    #region LevelUpgrades
    public bool CanBeUpgradedToNext()
    {
        if (level < upgradeLevel)
            return false;
        return true;
    }

    public bool CheckIfUpgradePossible(int i)
    {
        if (upgradeCount[i] >= maxUpgradeCount)
            return false;

        return true;
    }
    #endregion





    #region HealthRegen
    public bool HealthRegenTick_Cooldown { get; private set; }

    public async void HealthRegenTick()
    {
        HealthRegenTick_Cooldown = true;
        await Task.Delay(1000);
        HealthRegenTick_Cooldown = false;
        health += HealthRegen;
        if (health > maxHealth)
            health = maxHealth;
    }
    #endregion





    [SerializeField] int maxUpgradeCount;
    public int MaxUpgradeCount => maxUpgradeCount;
    [SerializeField] int[] upgradeCount;



    public void AddExp(int value)
    {
        while (value > requiredExp)
        {
            value -= (requiredExp - exp);
            LevelUp();
        }

        exp += value;
        if (exp > requiredExp)
        {
            value -= (requiredExp - exp);
            LevelUp();
            exp = value;
        }

    }

    void LevelUp()
    {
        level++;
        health = maxHealth;
        SoundManager.Instance.PlaySound(SoundManager.Instance.LevelUp);
        TurretBehaviour.HealthBarScript.HealthRegenHpBar(health, maxHealth);
        TurretBehaviour.PlayLevelUp();
        requiredExp = (int)((float)requiredExp * 2.5f);
        exp = 0;
        maxUpgradeCount += 3;
    }

    [SerializeField] int upgradeLevel;
    public int UpgradeLevel => upgradeLevel;
    public int[] UpgradeCount => upgradeCount;
    public int[] GetCosts => new int[] { maxHealthCost, healthRegenCost, damageCost, fireRateCost, fireRangeCost, critRateCost, critDamageCost, rotationSpeedCost };
    public float[] GetValues => new float[] { maxHealth, healthRegen, damage, fireRate, fireRange, critRate, critDamage, rotationSpeed };


    public static Turret CreateInstance(Turret turret)
    {
        var clone = ScriptableObject.CreateInstance<Turret>();
        clone.type = turret.Type;
        clone.headType = turret.HeadType;
        clone.bulletType = turret.bulletType;
        clone.upgradeAmount = turret.UpgradeAmount;
        clone.maxUpgradeCount = turret.maxUpgradeCount;
        clone.maxHealth = turret.maxHealth;
        clone.health = clone.maxHealth;
        clone.healthRegen = turret.healthRegen;
        clone.damage = turret.damage;
        clone.fireRate = turret.fireRate;
        clone.fireRange = turret.fireRange;
        clone.critRate = turret.critRate;
        clone.critDamage = turret.critDamage;
        clone.rotationSpeed = turret.rotationSpeed;

        clone.modifier = turret.modifier;
        clone.costModifier = turret.costModifier;

        clone.maxHealthCost = turret.maxHealthCost;
        clone.healthRegenCost = turret.healthRegenCost;
        clone.damageCost = turret.damageCost;
        clone.fireRateCost = turret.fireRateCost;
        clone.fireRangeCost = turret.fireRangeCost;
        clone.critDamageCost = turret.critDamageCost;
        clone.critRateCost = turret.critRateCost;
        clone.rotationSpeedCost = turret.rotationSpeedCost;
        clone.upgradeCount = new int[8];
        clone.exp = turret.exp;
        clone.level = turret.level;
        clone.requiredExp = turret.requiredExp;
        clone.upgradeLevel = turret.upgradeLevel;
        clone.icon = turret.icon;
        clone.damageOutput = clone.damage;
        clone.fireRateOutput = clone.fireRate;

        return clone;
    }

    public void BoostEffect(int i, int strength, int duration)
    {
        switch (i)
        {
            case 0:
                OverloadedBoost(strength,duration);
            break;



            default:
            break;
        }
    }


    public async void OverloadedBoost(float strength, float duration)
    {
        damageOutput = damage * (1 + strength / 100);
        while (duration > 0f)
        {
            duration -= Time.deltaTime;
            await Task.Yield();
        }
        damageOutput = damage;
    }




}

public enum TurretType { projectile, laser, autoProjectile, orbital, autoLaser }

public enum TurretTier { SingleBarrel, DoubleBarrel, MultipleBarrel }


