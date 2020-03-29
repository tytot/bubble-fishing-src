using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class SecondChance : MonoBehaviour, IUnityAdsListener
{
    Image fill;

    #if UNITY_IOS
        private string gameId = "3208302";
    #elif UNITY_ANDROID
        private string gameId = "3208303";
    #endif
    string placementId = "secondChance";

    Button nigButt;
    Death death;
    Animator anim;
    public Transform fish;

    AudioSource ass;
    public AudioSource countdownAss;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;

        fill = transform.GetChild(2).GetComponent<Image>();
        nigButt = transform.GetChild(2).GetComponent<Button>();
        nigButt.interactable = Advertisement.IsReady(placementId);
        Advertisement.Initialize(gameId, true);
        Advertisement.AddListener(this);

        death = FindObjectOfType<Death>();

        AudioListener.pause = true;

        anim = GetComponent<Animator>();
        ass = GetComponent<AudioSource>();

        bool nig = PlayerPrefs.GetInt("SFX Muted") == 0;
        ass.ignoreListenerPause = nig;
        countdownAss.ignoreListenerPause = nig;
    }

    // Update is called once per frame
    void Update()
    {
        if (fill.fillAmount == 0f)
        {
            gameObject.SetActive(false);
            death.setSucc(0);
            Advertisement.RemoveListener(this);
        }
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Advertisement.RemoveListener(this);
    }

    public void ShowRewardedVideo () {
        Advertisement.Show(placementId);
        anim.enabled = false;
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady (string pId) {
        // If the ready Placement is rewarded, activate the button: 
        if (pId == placementId) {        
            nigButt.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish (string pId, ShowResult showResult) {
        if (pId == placementId)
        {
            // Define conditional logic for each ad completion status:
            if (showResult == ShowResult.Finished)
            {
                death.setSucc(1);

                for (int i = fish.childCount - 1; i >= 0; i--)
                {
                    Destroy(fish.GetChild(i).gameObject);
                }
            }
            else
            {
                death.setSucc(0);
            }
            Advertisement.RemoveListener(this);
            gameObject.SetActive(false);
        }
    }

    public void OnUnityAdsDidError (string message) {
        // Log the error.
    }

    public void OnUnityAdsDidStart (string pId) {
        // Optional actions to take when the end-users triggers an ad.
    } 
}