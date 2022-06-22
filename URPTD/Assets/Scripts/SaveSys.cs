using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System;

public class SaveSys
{
    [SerializeField] GameStats gameStats;
    public const string FILE_NAME = "saveData.td";

    public static string PATH = Application.persistentDataPath + FILE_NAME;
    public static void SAVE(GameStats gameStats)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fs = new FileStream(PATH, FileMode.Create))
        {
            formatter.Serialize(fs, new Save(gameStats));
        }
    }



    public static void LOAD_INTO(ref GameStats gameStats)
    {   
        if (!File.Exists(PATH))
            return;

        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fs = new FileStream(PATH, FileMode.Open))
        {
            Save save = formatter.Deserialize(fs) as Save;
            gameStats.SetStatsFromSave(save);
        }

    }
}



#region PAIN PEKO

[Serializable]
public class Save
{
    public List<ShopUnlockSerializable> ListOfUnlocks;
    public int Energy, CCoinCount, HighestScore;
    public long LastLogged;
    public float SoundVolume, MusicVolume;
    public int QualIndex;

    public Save(GameStats gameStats)
    {
        ListOfUnlocks = new List<ShopUnlockSerializable>();

        this.Energy = gameStats.Energy;
        this.CCoinCount = gameStats.CCoinCount;
        this.HighestScore = gameStats.HighestScore;
        this.LastLogged = gameStats.LastLogged;
        this.SoundVolume = gameStats.SoundVolume;
        this.MusicVolume = gameStats.MusicVolume;
        this.QualIndex = gameStats.QualIndex;

        foreach (ShopUnlock shopUnlock in gameStats.ListOfUnlocks)
        {
            ShopUnlockSerializable x = new ShopUnlockSerializable(shopUnlock);
            ListOfUnlocks.Add(x);
        }
    }

}

[Serializable]
public struct ShopUnlockSerializable
{
    public int BaseCost, UnlockCost, ReduceCost, ReduceCostLeft, UpgradeCost;
    public bool IsUnlocked, CanBePurchased;
    
    public ShopUnlockSerializable(ShopUnlock shopUnlock)
    {
        this.BaseCost = shopUnlock.BaseCostInGame;
        this.UpgradeCost = shopUnlock.UpgradeCostInGame;
        this.UnlockCost = shopUnlock.UnlockCost;
        this.ReduceCost = shopUnlock.ReduceCost;
        this.ReduceCostLeft = shopUnlock.ReduceCostLeft;
        this.IsUnlocked = shopUnlock.IsUnlocked;
        this.CanBePurchased = shopUnlock.CanBePurchased;
    }
}

#endregion