using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class EnergyHandle : MonoBehaviour
{
    [Header("GAME STATS OBJECT")]
    [SerializeField] GameStats gameStats;
    [SerializeField] MainManuBtnHandle btnHandle;

    void Update()
    {
        if (!cooldown)
            CheckAfterOneMinute();
    }




    bool cooldown = false;

    async void CheckAfterOneMinute()
    {
        float timer = 61f;
        cooldown = true;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            await Task.Yield();
        }
        cooldown = false;
        gameStats.CalculateEnergy(false);
        btnHandle.EnergyHandle();
    }


    void OnApplicationFocus(bool active)
    {
        if (firstLaunch)
            return;

        if (active)
        {
            if (gameStats.CalculateEnergy(true))
                return;
            else
                Debug.Log("SERVER ERROR");
        }

        Debug.Log("APP SOFTCLOSED");


    }

    bool firstLaunch = true;
    void Start()
    {
        if (gameStats.CalculateEnergy(true))
        {
            firstLaunch = false;
        }
        else
        {
            Debug.Log("SERVER ERROR");
        }

    }



}
