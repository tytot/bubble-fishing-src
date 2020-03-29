using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    public static Manager instance = null;

    [Range(0, 256)]
    public int weight;

    [Range(4, 256)]
    public int capacity;

    [Range(0, 100)]
    public float percentage;

    bool sinking = false;

    [Range(0f, 10000f)]
    public float depth;

    public bool ledge;

    public bool freeze;

    public bool magnet;

    public bool drill;

    public Pause pause;

    public RectTransform plank;
    Animator anim;

    AudioSource blip;

    bool realQuick = false;
    
    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        weight = 0;
        capacity = 4;

        switch (PlayerPrefs.GetInt("Launcher"))
        {
            case 1:
                depth = 100;
                break;
            case 2:
                depth = 200;
                break;
            case 3:
                depth = 500;
                break;
            case 4:
                depth = 1000;
                break;
            default:
                depth = 100;
                break;
        }
        if (PlayerPrefs.GetInt("Tutorial") == 0)
            depth = 50;

        blip = GetComponent<AudioSource>();
        blip.ignoreListenerPause = true;

        anim = plank.GetComponent<Animator>();
    }
    void Update()
    {
        if (!drill)
            percentage = 100 * weight / capacity;
        else
        {
            percentage = 150;
            ledge = false;
        }

        if (!ledge && !pause.isPaused)
        {
            if (!freeze)
                depth += (percentage - 50f) / 1000f;
            else
                depth += (percentage - 50f) / 4000f;
        }

        if (PlayerPrefs.GetInt("Tutorial") > 0 && ((sinking && percentage < 50f) || (!sinking && percentage > 50f)))
        {
            ledge = false;
            sinking = !sinking;
        }

        switch ((int)depth)
        {
            case 1:
                realQuick = true;
                break;
            case 100:
                if (realQuick)
                    Achievement("Real Quick", "Go from 1 m to 100 m");
                break;
            case 1000:
                Achievement("One Grand", "Reach 1,000 meters");
                break;
            case 5000:
                Achievement("Halfway There", "Reach 5,000 meters");
                break;
            case 10000:
                Achievement("Maritime Maestro", "Reach 10,000 meters");
                break;
            default:
                break;
        }
    }

    public void Achievement(string title, string message)
    {
        if (PlayerPrefs.GetInt(title) == 0)
        {
            bool played = false;
            PlayerPrefs.SetInt(title, 1);
            PlayerPrefs.Save();
            if (!played)
            {
                blip.Play();
                played = true;
            }
            plank.GetComponentInChildren<Text>().text = "<b>" + title + "</b>\n" + message;
            anim.Play("Achievement");
        }
    }
}
