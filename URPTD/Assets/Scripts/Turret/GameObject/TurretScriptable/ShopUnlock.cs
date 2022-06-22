using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopUnlock", menuName = "ShopUnlock", order = 1)]
public class ShopUnlock : ScriptableObject
{
    [SerializeField] Sprite icon;
    [SerializeField] float iconScale;
    [SerializeField] string unlockName;
    [SerializeField] int baseCostInGame, upgradeCostInGame;
    [SerializeField] GameObject turretPrefab;
    [SerializeField] int unlockCost, reduceCostInGame, reduceCostLeft;
    [SerializeField] bool canBePurchased = false, isUnlocked = false;
    public bool CanBePurchased => canBePurchased;
    public bool IsUnlocked => isUnlocked;
    public int UpgradeCostInGame => upgradeCostInGame;
    public Sprite Icon => icon;
    public int BaseCostInGame => baseCostInGame;
    public GameObject TurretPrefab => turretPrefab;
    public string UnlockName => unlockName;
    public float IconScale => iconScale;
    public int UnlockCost => unlockCost;
    public void Unlock() => isUnlocked = true;
    public int ReduceCost => reduceCostInGame;
    public int ReduceCostLeft => reduceCostLeft;
    public void ReduceCostHandle()
    {
        reduceCostLeft -= 1;
        reduceCostInGame *= 2;
        upgradeCostInGame /= 2;
    }

    public void SetStatsFromSave(ShopUnlockSerializable shopUnlock)
    {
        this.baseCostInGame = shopUnlock.BaseCost;
        this.upgradeCostInGame = shopUnlock.UpgradeCost;
        this.unlockCost = shopUnlock.UnlockCost;
        this.reduceCostInGame = shopUnlock.ReduceCost;
        this.reduceCostLeft = shopUnlock.ReduceCostLeft;
        this.canBePurchased = shopUnlock.CanBePurchased;
        this.isUnlocked = shopUnlock.IsUnlocked;
    }
}
