using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {

    public bool isPaused;
    public GameObject boins;
    public GameObject buttons;
    Toggle music;
    Toggle SFX;
    Animator boinsAnim;
    public Animator pauseAnim;

    public AudioSource theme;
    AudioSource pauseClick;

    private void Start()
    {
        pauseClick = GetComponent<AudioSource>();
        music = buttons.transform.GetChild(0).GetComponent<Toggle>();
        SFX = buttons.transform.GetChild(1).GetComponent<Toggle>();

        int bs = PlayerPrefs.GetInt("Boins", 0);
        boins.GetComponent<Text>().text = bs.ToString("D4");

        boinsAnim = boins.GetComponent<Animator>();
        
        theme.ignoreListenerPause = true;
        theme.volume = PlayerPrefs.GetInt("Music", 1);
        
        if (PlayerPrefs.GetInt("Music") == 0)
            music.isOn = true;
        else
            music.isOn = false;

        bool boo = PlayerPrefs.GetInt("SFX Muted") == 1;
        SFX.isOn = boo;
        AudioListener.pause = boo;
        pauseClick.ignoreListenerPause = !boo;
    }

    public void muteMusic(bool dank)
    {
        theme.volume = dank ? 0 : 1;
        PlayerPrefs.SetInt("Music", dank ? 0 : 1);
        PlayerPrefs.Save();
    }

    public void muteSFX(bool diccl)
    {
        pauseClick.ignoreListenerPause = !diccl;
        PlayerPrefs.SetInt("SFX Muted", diccl ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnPause(bool paused)
    {
        boinsAnim.Play("Boins" + paused.ToString());
        pauseAnim.Play("Pause" + paused.ToString());

        if (paused)
        {
            isPaused = true;
            Time.timeScale = 0f;
            AudioListener.pause = true;
            pauseClick.Stop();
            StopAllCoroutines();
        }
        else StartCoroutine(UnPause());
    }

    IEnumerator UnPause()
    {
        if (PlayerPrefs.GetInt("Skip Countdown", 0) == 0)
        {
            pauseClick.PlayOneShot(pauseClick.clip);
            yield return new WaitForSecondsRealtime(3.3f);
        }
        else
            pauseAnim.SetTrigger("Skip");

        AudioListener.pause = false;
        if (PlayerPrefs.GetInt("SFX Muted") == 1)
            AudioListener.pause = true;
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ToEnd()
    {
        Manager.instance.weight = Manager.instance.capacity;
        Manager.instance.depth = 9980f;
    }
}
