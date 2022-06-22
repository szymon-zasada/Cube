using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "GameStats", menuName = "GameStats", order = 1)]
[Serializable]
public class GameStats : ScriptableObject
{

    [SerializeField] List<ShopUnlock> listOfUnlocks = new List<ShopUnlock>();
    [SerializeField] int energy, cCoinCount, highestScore;
    [SerializeField] long lastLogged;
    [SerializeField] float soundVolume, musicVolume;
    [SerializeField] int qualIndex;

    public int CCoinCount => cCoinCount;


    public int ChangeBalanceCCoin(int value) => cCoinCount += value;

    public int QualIndex
    {
        get => qualIndex;
        set
        {
            if (value > 2 || value < 0)
                return;
            qualIndex = value;
        }
    }
    public int HighestScore { get => highestScore; set => highestScore = value; }
    public float SoundVolume { get => soundVolume; set => soundVolume = value; }
    public float MusicVolume { get => musicVolume; set => musicVolume = value; }
    public long LastLogged { get => lastLogged; set => lastLogged = value; }
    public int Energy => energy;
    public List<ShopUnlock> ListOfUnlocks => listOfUnlocks;


    public void AddEnergy(int value)
    {
        energy += value;
        if (energy > 100)
            energy = 100;
    }

    public void RemoveEnergy(int value) => energy -= value;

    public bool CalculateEnergy(bool isServer)
    {
        if (isServer)
        {
            long? serverDate = NTP.GetTime().Result;

            if (serverDate == null)
                return false;

            AddEnergy((int)(serverDate - lastLogged) / 60);

            Debug.Log($"SERVER TIME: {serverDate} DIFFERENCE: {(int)(serverDate - lastLogged) / 60}");
            lastLogged = (long)serverDate;
            return true;
        }

        AddEnergy((int)(TIMESTAMP_NOW - lastLogged) / 60);
        lastLogged = TIMESTAMP_NOW;
        return true;
    }



    public static long TIMESTAMP_NOW => new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();

    public void SetStatsFromSave(Save save)
    {
        this.energy = save.Energy;
        this.cCoinCount = save.CCoinCount;
        this.highestScore = save.HighestScore;
        this.lastLogged = save.LastLogged;
        this.soundVolume = save.SoundVolume;
        this.musicVolume = save.MusicVolume;
        this.qualIndex = save.QualIndex;

        for (int i = 0; i < save.ListOfUnlocks.Count; i++)
            this.listOfUnlocks[i].SetStatsFromSave(save.ListOfUnlocks[i]);


    }




}

