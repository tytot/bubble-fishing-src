using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleHue : MonoBehaviour {

    private Image title;
    public float changeSpeed;
    public GameObject tutorial;

    void Start()
    {
        if (!PlayerPrefs.HasKey("Boins"))
            PlayerPrefs.SetInt("Boins", 0); //TO CHANGE
        title = GetComponent<Image>();
        
        if (PlayerPrefs.GetInt("Tutorial") == 0)
        {
            tutorial.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("Tutorial") == 1)
        {
            tutorial.SetActive(true);
            tutorial.GetComponent<Animator>().Play("Tutorial7");
        }

    }

    // Update is called once per frame
    void Update () {
        float hue = (Mathf.Sin(Time.time * changeSpeed) + 1) / 2f;
        title.color = Color.HSVToRGB(hue, 0.25f, 1f);
	}

    public void Timescale(float val)
    {
        Time.timeScale = val;
    }

    public void SkipTutorial()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("Tutorial", 2);
        PlayerPrefs.SetInt("Launcher", 1);
        PlayerPrefs.Save();
    }
}
