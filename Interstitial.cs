using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Interstitial : MonoBehaviour, IUnityAdsListener
{
    #if UNITY_IOS
        private string gameId = "3208302";
    #elif UNITY_ANDROID
        private string gameId = "3208303";
    #endif
    string placementId = "interstitial";

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Ad Counter"))
            PlayerPrefs.SetInt("Ad Counter", 0);
        
        Advertisement.Initialize(gameId, true);
        Advertisement.AddListener(this);
    }

    public void ShowRewardedVideo()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show(placementId);
            AudioListener.pause = true;
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady(string pId)
    {
        // If the ready Placement is rewarded, activate the button: 
    }

    public void OnUnityAdsDidFinish(string pId, ShowResult showResult)
    {
        if (pId == placementId)
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
            Advertisement.RemoveListener(this);
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string pId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}

