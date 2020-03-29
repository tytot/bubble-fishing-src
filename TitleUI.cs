using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TitleUI : MonoBehaviour {

    Ad4Boins ad;

    private void Start()
    {
        if (PlayerPrefs.GetInt("Boins", 0) == 0 && PlayerPrefs.GetInt("Tutorial") == 1)
            PlayerPrefs.SetInt("Boins", 10);
        ad = FindObjectOfType<Ad4Boins>();
        if (PlayerPrefs.GetInt("Scope", 1) == 1)
            PlayerPrefs.SetInt("Scope", 2);
    }

    // Update is called once per frame
    public void Play () {
        ad.ClearListener();
        SceneManager.LoadScene("Intro");
	}
}
