using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;



public class MainManuUI : MonoBehaviour
{

    //v________________________________________________________________________________________________________________________________________________________
    [SerializeField] RawImage sliderImage;
    [SerializeField] Material glowingMaterial;
    [SerializeField] Slider energySlider;
    [SerializeField] Color glowingColor;
    [SerializeField] TMP_Text energyText;

    [SerializeField] List<float> visualValues;
    [SerializeField] List<GameObject> visualPercentage;

    bool rectCooldown;


    [Header("GAME STATS OBJECT")]
    [SerializeField] GameStats gameStats;

    void Start() => rectCooldown = false;
    void Update()
    {
        SliderHandle();
    }




    
    async void SliderHandle()
    {
        energySlider.value = gameStats.Energy / 100f;
        energyText.text = $"{gameStats.Energy}%";

        for (int i = 0; i < visualPercentage.Count; i++)
        {
            if (visualValues[i] > energySlider.value)
                visualPercentage[i].SetActive(false);

            else
                visualPercentage[i].SetActive(true);
        }

        if (rectCooldown)
            return;

        rectCooldown = true;
        await Task.Delay(30);
        glowingMaterial.color = glowingColor * Random.Range(0.80f, 1.1f);
        if (sliderImage == null)
            return;
        sliderImage.uvRect = new Rect(Random.Range(.2f, 0.9f), 0f, 0.45f, 0.35f);

        rectCooldown = false;
    }











}
