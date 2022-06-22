using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TurretStatsUI : MonoBehaviour
{

    [SerializeField] TurretBehaviour turretBehaviour;
    [SerializeField] TMP_Text health, healthRegen, damage, fireRate, fireRange;

    void Update()
    {



        if (Input.GetKeyDown(KeyCode.Escape))
            gameObject.SetActive(false);

        health.text = Mathf.Round(turretBehaviour.turret.Health*10f).ToString() + "/" + Mathf.Round(turretBehaviour.turret.MaxHealth*10f).ToString();
        healthRegen.text = Mathf.Round(turretBehaviour.turret.HealthRegen*10f).ToString();
        damage.text = Mathf.Round(turretBehaviour.turret.Damage*10f).ToString();
        fireRate.text = Mathf.Round(turretBehaviour.turret.FireRate*10f).ToString();
        fireRange.text = Mathf.Round(turretBehaviour.turret.FireRange*10f).ToString();
    }




}
