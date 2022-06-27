using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class InGameAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
{

    public readonly static string ANDROID_KEY = "4791287", IOS_KEY = "4791286";


    void Start()
    {

        Advertisement.Initialize(ANDROID_KEY, false, this); //initialize ads module
        Advertisement.Load("Rewarded_Android", this);
    }


    public void OnUnityAdsAdLoaded(string adUnitId)
    {

    }

    public void ShowAd()
    {

        Advertisement.Show("Rewarded_Android", this);
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals("Rewarded_Android") && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            if (GameManager.Instance.GameUIManager.BoostPopUP.activeSelf)
            {
                GameManager.Instance.GameUIManager.GetAdBoost();
                Advertisement.Load("Rewarded_Android", this);
                return;
            }
            GameManager.Instance.GameUIManager.GetAdReward();
            Advertisement.Load("Rewarded_Android", this);
        }

    }


    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message) { }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message) { }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }


    public void OnInitializationComplete()
    {
        Advertisement.Load("Rewarded_Android", this);
    }


    public void OnInitializationFailed(UnityAdsInitializationError error, string message) { }
}
