using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpObtain : MonoBehaviour {
    
    public BubbleSpawner bubbleSpawner;
    private FishObtain fishObtain;
    public CameraShake shock;
    public Freeze freeze;

    private string PUN;
    private float timerStart;
    public bool powerUpEnabled;
    Toggle togg;

    public Image PU;
    public Image PUBox;
    private Image bar;
    public Sprite timeFreeze;
    public Sprite bubblePotion;
    public Sprite fastFoward;
    public Sprite magnet;
    public Sprite zap;

    private GameObject shield;
    public bool stopShield = false;
    public GameObject overlay;
    private AudioSource[] sounds;
    public AudioLowPassFilter lpf;
    
    public Transform fish;

    public AudioSource obtain;

    void Start()
    {
        PU.enabled = false;
        powerUpEnabled = false;
        togg = PU.transform.parent.GetComponent<Toggle>();

        fishObtain = GetComponent<FishObtain>();

        sounds = overlay.GetComponents<AudioSource>();

        bar = PUBox.transform.GetChild(2).GetComponent<Image>();
        shield = transform.GetChild(1).gameObject;
    }

    void Update()
    {
        if (powerUpEnabled)
        {
            switch (PUN)
            {
                case "clocksm":
                    bar.fillAmount = 1f - ((Time.time - timerStart) / 8f);
                    if (Time.time > timerStart + 8f)
                    { 
                        Manager.instance.freeze = false;
                        powerUpEnabled = false;
                        freeze.freeze = false;
                    }
                    else if (lpf.cutoffFrequency > 2500f)
                        lpf.cutoffFrequency = 2500f;
                    break;
                case "bps":
                    bar.fillAmount = 1f - ((Time.time - timerStart) / 3f);
                    if (Time.time > timerStart + 3f)
                    {
                        bubbleSpawner.changed = false;
                        powerUpEnabled = false;
                    }
                    break;
                case "zapsm":
                    for (int i = 0; i < fish.childCount; i++)
                        fishObtain.Disable(fish.GetChild(i).gameObject);

                    string[] tagsToDelete = { "Bubble", "PowerUp" };
                    foreach (string tag in tagsToDelete)
                    {
                        GameObject[] pitiful = GameObject.FindGameObjectsWithTag(tag);
                        foreach (GameObject pities in pitiful)
                            Destroy(pities);
                    }

                    shock.StartShake(0.5f);
                    
                    sounds[0].Play(0);

                    powerUpEnabled = false;
                    break;
                case "ffsm":
                    bar.fillAmount = 1f - ((Time.time - timerStart) / 15f);
                    if (Time.time > timerStart + 15f || stopShield)
                    {
                        shield.GetComponent<Animator>().SetTrigger("Pop");
                        powerUpEnabled = false;
                        stopShield = false;
                        sounds[4].Stop();
                        bar.fillAmount = 0f;
                    }
                    break;
                case "magnetsm":
                    bar.fillAmount = 1f - ((Time.time - timerStart) / 5f);
                    if (Time.time > timerStart + 5f)
                    {
                        Manager.instance.magnet = false;
                        powerUpEnabled = false;
                    }
                    break;
            }
        }

        if (bar.fillAmount == 0f)
        {
            if (!PUBox.enabled)
            {
                PUBox.enabled = true;
                PUBox.transform.GetChild(0).gameObject.SetActive(false);
                PUBox.GetComponent<Toggle>().isOn = false;
                togg.interactable = true;
            }
            else
                PU.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 2f);
        }
    }

    public void TriggerPowerUp(bool val)
    {
        if (val)
        {
            if (!powerUpEnabled && PU.enabled)
            {
                powerUpEnabled = true;
                PU.enabled = false;
                PU.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -1.5f);
                timerStart = Time.time;
                PUN = PU.sprite.name;
                togg.interactable = false;

                PUBox.enabled = false;
                PUBox.transform.GetChild(0).gameObject.SetActive(true);

                if (PUN == "clocksm")
                {
                    Manager.instance.freeze = true;
                    freeze.freeze = true;
                    sounds[2].Play();
                }
                if (PUN == "bps")
                {
                    bubbleSpawner.changed = true;
                    sounds[1].Play();
                }
                if (PUN == "magnetsm")
                {
                    Manager.instance.magnet = true;
                    sounds[3].Play();
                }
                if (PUN == "ffsm")
                {
                    Animator a = shield.GetComponent<Animator>();
                    shield.GetComponent<SpriteRenderer>().enabled = true;
                    a.Play("Shield");
                    if (transform.localScale.x >= 4f)
                        a.SetTrigger("Grow");
                    sounds[4].Play();
                    stopShield = false;
                }
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Contains("PowerUp") && !Manager.instance.drill)
        {
            PU.enabled = true;

            var powerUp = col.gameObject;

            if (powerUp.name.Contains("Time Freeze"))
            {
                PU.sprite = timeFreeze;
            }
            else if (powerUp.name.Contains("Bubble Potion"))
            {
                PU.sprite = bubblePotion;
            }
            else if (powerUp.name.Contains("Zap"))
            {
                PU.sprite = zap;
            }
            else if (powerUp.name.Contains("Fast Forward"))
            {
                PU.sprite = fastFoward;
            }
            else if (powerUp.name.Contains("Magnet"))
            {
                PU.sprite = magnet;
            }
            
            PU.preserveAspect = true;
            obtain.Play();

            Destroy(powerUp);
        }
    }
}
