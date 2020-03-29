using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class Ad4Boins : MonoBehaviour, IUnityAdsListener
{
    #if UNITY_IOS
    private string gameId = "3208302";
    #elif UNITY_ANDROID
        private string gameId = "3208303";
    #endif
    string placementId = "forBoins";

    Button thisButt;

    public Button yeos;
    public GameObject addedBoins;
    public Text boins;

    bool add;
    int added = 0;

    public AudioClip getBoin;
    public AudioSource obtain;

    Text timer;
    bool timerStarted = false;

    public int secsRequired = 3600;

    public void OpenTwitter()
    {
        Application.OpenURL("https://twitter.com/bubble_fishing/");
    }

    IEnumerator AddBoins()
    {
        addedBoins.SetActive(true);
        yield return new WaitForSeconds(1f);
        add = true;
        addedBoins.SetActive(false);
    }

    int UnixTimestamp()
    {
        return (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Last Watch"))
            PlayerPrefs.SetInt("Last Watch", 0);
        boins.text = PlayerPrefs.GetInt("Boins").ToString("D4");

        yeos.interactable = Advertisement.IsReady(placementId);
        Advertisement.Initialize(gameId, true);
        Advertisement.AddListener(this);

        timer = GetComponentInChildren<Text>();
        thisButt = GetComponent<Button>();

        if (UnixTimestamp() - PlayerPrefs.GetInt("Last Watch") < secsRequired)
        {
            timerStarted = true;
            thisButt.interactable = false;
        }
    }

    void Update()
    {
        if (add)
        {
            added++;
            int start = PlayerPrefs.GetInt("Boins");
            boins.text = (start + added).ToString("D4");
            obtain.PlayOneShot(getBoin);

            if (added == 10)
            {
                PlayerPrefs.SetInt("Boins", start + added);
                add = false;
                added = 0;
                PlayerPrefs.Save();
            }
        }

        if (timerStarted)
        {
            int last = PlayerPrefs.GetInt("Last Watch");
            int diff = secsRequired - (UnixTimestamp() - last);
            timer.text = (diff / 60).ToString("D2") + ":" + (diff % 60).ToString("D2");
            if (diff == 0)
            {
                timerStarted = false;
                thisButt.interactable = true;
            }
        }
    }

    public void ShowRewardedVideo()
    {
        Advertisement.Show(placementId);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady(string pId)
    {
        // If the ready Placement is rewarded, activate the button: 
        if (pId == placementId)
        {
            yeos.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish(string pId, ShowResult showResult)
    {
        if (pId == placementId)
        {
            // Define conditional logic for each ad completion status:
            if (showResult == ShowResult.Finished)
            {
                StartCoroutine(AddBoins());
                PlayerPrefs.SetInt("Last Watch", UnixTimestamp());
                timerStarted = true;
                thisButt.interactable = false;
            }
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

    public void ClearListener()
    {
        Advertisement.RemoveListener(this);
    }
}
