using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    [SerializeField] Slider hpBar;
    [SerializeField] Image hpBarColor;

    void Start()
    {
        hpBar.value = 1f;
    }


    void Update()
    {
        gameObject.transform.LookAt(Camera.main.transform);
    }


    public void HealthRegenHpBar(float health, float maxHealth)
    {
        hpBar.value = health / maxHealth;

        if(hpBar.value >= 0.9f)
            hpBarColor.color = new Color32(255, 0, 0, 0);      
    }


    public void ChangeHpBar(float health, float maxHealth)
    {
        hpBar.value = health / maxHealth;
        hpBarColor.color = new Color32(255, 0, 0, 255);
    }





}
