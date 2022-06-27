using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using System.Threading.Tasks;
using UnityEngine.Advertisements;



public class MainManuBtnHandle : MonoBehaviour
{
    public static string ANDROID_GAME_ID = "4779839";
    [SerializeField] GameObject mainMenu, options, levelLoading, shop, musicCredits;
    [SerializeField] GameStats gameStats;
    [SerializeField] TMP_Text newRun;
    [SerializeField] List<Image> newRunLights;
    [SerializeField] Color32 availableColor;
    [SerializeField] Color32 notAvailableColor;
    [SerializeField] Slider soundVolumeSlider, musicVolumeSlider;
    [SerializeField] TMP_Text soundVolume, musicVolume;
    [SerializeField] List<GameObject> listOfQualSettings;
    [SerializeField] Slider loadingSlider;
    [SerializeField] TMP_Text loadingText;
    [SerializeField] Button adButton;
    GameObject musicGameObject;
    AudioSource musicSource;
    public GameStats GS { get => gameStats; set => gameStats = value; }

    

    void Start()
    {
        Time.timeScale = 1f;
        SaveSys.LOAD_INTO(ref gameStats);
        
        EnergyHandle();

        ChangeQual(gameStats.QualIndex);
        musicGameObject = GameObject.FindGameObjectWithTag("Music");
        musicSource = musicGameObject.GetComponent<AudioSource>();
        musicSource.volume = gameStats.MusicVolume;
        EnergyHandle();
    }




    public void EnergyHandle()
    {
        if (gameStats.Energy >= 25)
        {
            newRun.text = "NEW RUN";
            foreach (Image i in newRunLights)
                i.color = availableColor;

            return;
        }

        newRun.text = "ENERGY LOW";
        foreach (Image i in newRunLights)
            i.color = notAvailableColor;
    }

    #region volumes
    public void SetSoundValue()
    {
        gameStats.SoundVolume = soundVolumeSlider.value;
        soundVolume.text = ((int)(gameStats.SoundVolume * 100f)).ToString() + "%";
    }

    public void SetMusicValue()
    {
        gameStats.MusicVolume = musicVolumeSlider.value;
        musicVolume.text = ((int)(gameStats.MusicVolume * 100f)).ToString() + "%";
        musicSource.volume = gameStats.MusicVolume;
    }


    void ChangeQual(int index)
    {
        for (int i = 0; i < listOfQualSettings.Count; i++)
        {
            if (i == index)
            {
                listOfQualSettings[i].SetActive(true);
                gameStats.QualIndex = i;
                continue;
            }
            listOfQualSettings[i].SetActive(false);
        }
    }
    #endregion

    public void StartBtn()
    {

        if (gameStats.Energy < 25)
            return;

        LoadLevel();
    }


    async void LoadLevel()
    {
        levelLoading.SetActive(true);
        mainMenu.SetActive(false);
        gameStats.RemoveEnergy(25);
        SaveSys.SAVE(gameStats);
        AsyncOperation operation = SceneManager.LoadSceneAsync("Main");
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;
            loadingText.text = (progress * 100f).ToString() + "%";
            await Task.Yield();
        }
    }

    async void RotateLoadingCircle()
    {
        await Task.Yield();
    }


    public void OptionsBtn()
    {
        mainMenu.SetActive(false);
        options.SetActive(true);
        soundVolumeSlider.value = gameStats.SoundVolume;
        musicVolumeSlider.value = gameStats.MusicVolume;
    }

    public void BackBtn()
    {
        mainMenu.SetActive(true);
        options.SetActive(false);
        shop.SetActive(false);
        SaveSys.SAVE(gameStats);
    }


    public void QuitBtn() => Application.Quit();


    public void ShopBtn()
    {
        mainMenu.SetActive(false);
        shop.SetActive(true);
    }

    public void CreditsBtn()
    {
        if (musicCredits.activeSelf)
            musicCredits.SetActive(false);
        else
            musicCredits.SetActive(true);
    }


    public void WatchAddBtn()
    {

        gameStats.AddEnergy(20);



    }


    public void Qual0Btn()
    {
        QualitySettings.SetQualityLevel(0);
        ChangeQual(0);
    }

    public void Qual1Btn()
    {
        QualitySettings.SetQualityLevel(1);
        ChangeQual(1);
    }


    public void Qual2Btn()
    {
        QualitySettings.SetQualityLevel(2);
        ChangeQual(2);
    }

}
