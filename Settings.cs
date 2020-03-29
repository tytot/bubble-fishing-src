using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

    public AudioSource titleMusic;
    public AudioSource creditsMusic;
    public Toggle music;
    public Toggle SFX;
    public Toggle tilts;

    // Use this for initialization
    void Start () {
        if (!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetInt("Music", 1);
        if (!PlayerPrefs.HasKey("SFX Muted"))
            PlayerPrefs.SetInt("SFX Muted", 0);
        if (!PlayerPrefs.HasKey("Tilt Controls"))
            PlayerPrefs.SetInt("Tilt Controls", 0);
        if (!PlayerPrefs.HasKey("Skip Countdown"))
            PlayerPrefs.SetInt("Skip Countdown", 0);
        PlayerPrefs.Save();

        titleMusic.ignoreListenerPause = true;
        titleMusic.volume = PlayerPrefs.GetInt("Music", 1);
        music.isOn = PlayerPrefs.GetInt("Music") == 0;
        SFX.isOn = PlayerPrefs.GetInt("SFX Muted") == 1;
        AudioListener.pause = SFX.isOn;
        tilts.isOn = PlayerPrefs.GetInt("Skip Countdown") == 0;
    }
	
	public void muteMusic(bool dank)
    {
        titleMusic.volume = dank ? 0 : 1;
        creditsMusic.volume = dank ? 0 : 1;
        PlayerPrefs.SetInt("Music", dank ? 0 : 1);
        PlayerPrefs.Save();
    }

    public void muteSFX(bool diccl)
    {
        AudioListener.pause = diccl;
        PlayerPrefs.SetInt("SFX Muted", diccl ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void enableCountdown(bool nigbert)
    {
        PlayerPrefs.SetInt("Skip Countdown", nigbert ? 0 : 1);
        PlayerPrefs.Save();
    }
}
