using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleLight : MonoBehaviour {

    Transform unPressed;
    Image upBar;
    Transform pressed;
    Image pBar;
    Toggle togg;

    public Light bubbleLight;

    public float rechargeTime;
    public float lightTime;
    bool startLight = false;
    float clickTime;
    float endTime = 0;

    AudioSource lightSwitch;
    AudioSource buzz;
    public AudioClip on;
    public AudioClip off;

    int lightType;

    Sprite overlaydef;
    public Sprite overlaylight;
    public SpriteRenderer overlay;

	// Use this for initialization
	void Start () {
        overlaydef = overlay.sprite;
        lightType = PlayerPrefs.GetInt("Light Type");
        if (lightType == 0)
            gameObject.SetActive(false);

        unPressed = transform.GetChild(0);
        upBar = unPressed.GetChild(1).GetComponent<Image>();
        pressed = transform.GetChild(1);
        pBar = pressed.GetChild(1).GetComponent<Image>();

        togg = transform.GetChild(0).GetComponent<Toggle>();
        togg.interactable = false;

        lightSwitch = GetComponents<AudioSource>()[0];
        buzz = GetComponents<AudioSource>()[1];

        if (lightType == 2)
        {
            upBar.fillAmount = 1f;
            StartLight(true);
            buzz.Stop();
            togg.interactable = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (lightType == 1)
        {
            if (upBar.fillAmount == 1f)
            {
                togg.interactable = true;
            }

            if (startLight)
            {
                pBar.fillAmount = 1f - ((Time.time - clickTime) / lightTime);
                overlay.transform.position = new Vector3(bubbleLight.transform.position.x, -0.4f, -1f);

                if (Time.time > clickTime + lightTime)
                {
                    StartLight(false);
                }
            }
            else if (!startLight && upBar.fillAmount < 1f)
            {
                upBar.fillAmount = (Time.time - endTime) / rechargeTime;
            }
        }
	}

    public void StartLight (bool val)
    {
        if ((val && upBar.fillAmount == 1f) || !val)
        {
            unPressed.gameObject.SetActive(!val);
            pressed.gameObject.SetActive(val);
            bubbleLight.enabled = val;
            startLight = val;

            if (val)
            {
                overlay.sprite = overlaylight;
                clickTime = Time.time;
                
                lightSwitch.PlayOneShot(on);
                buzz.Play();
            }
            else
            {
                overlay.sprite = overlaydef;
                overlay.transform.position = new Vector3(0f, -0.4f, -1f);
                endTime = Time.time;
                upBar.fillAmount = 0.99f;

                togg.interactable = false;
                togg.isOn = false;
                lightSwitch.PlayOneShot(off);

                if (buzz.isPlaying)
                {
                    buzz.Stop();
                }
            }
        }
    }
}
