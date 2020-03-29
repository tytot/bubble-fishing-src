using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheEnd : MonoBehaviour
{
    bool done;
    bool finaleBegun;
    public GameObject bottom;

    public Animator credits;
    public GameObject bubble;

    public AudioSource music;
    AudioSource endAudio;

    public GameObject spawner;

    public AudioLowPassFilter lpf;

    public GameObject pause;
    public Animator boins;
    public Text boinsToAdd;
    int boinCount;
    bool add;
    AudioSource obtain;
    public AudioClip getBoin;

    IEnumerator Vait()
    {
        yield return new WaitForSeconds(21f);
        boins.Play("BoinsTrue");
        boinsToAdd.gameObject.SetActive(true);
        boinsToAdd.text = "+400";
        yield return new WaitForSeconds(1f);
        add = true;
        yield break;
    }

    void Start()
    {
        endAudio = GetComponent<AudioSource>();
        obtain = boins.GetComponent<AudioSource>();
        obtain.ignoreListenerPause = true;
        boins.GetComponent<Text>().text = PlayerPrefs.GetInt("Boins").ToString("D4");
    }

    // Update is called once per frame
    void Update()
    {
        if ((10000f - Manager.instance.depth) < (0.2f / Time.deltaTime))
        {
            if (!bottom.activeSelf)
                bottom.SetActive(true);

            if (done == false && Manager.instance.ledge == false)
            {
                float speed;
                if (!Manager.instance.freeze)
                    speed = (Manager.instance.percentage - 50f) / 5f;
                else
                    speed = (Manager.instance.percentage - 50f) / 20f;
                Vector3 movement = new Vector3(0, speed);

                movement *= Time.deltaTime;
                bottom.transform.Translate(movement);
            }
        }
        else {
            if (bottom.activeSelf)
                bottom.SetActive(false);
        }
        
        if ((int)Manager.instance.depth == 9980)
        {
            if (Manager.instance.percentage > 50f)
                spawner.SetActive(false);
            else
                spawner.SetActive(true);
        }
        else if ((int)Manager.instance.depth > 9990f)
        {
            music.volume = Mathf.Lerp(0f, 1f, (9999f - Manager.instance.depth) / 9f);
        }

        if (add)
        {
            boinCount += 1;
            boins.GetComponent<Text>().text = boinCount.ToString("D4");
            obtain.PlayOneShot(getBoin);

            if (boinCount == PlayerPrefs.GetInt("Boins"))
                add = false;
        }
    }

    public void StartCredits()
    {
        credits.Play("RollCredits");
        StartCoroutine(Vait());
    }

    public void Finale()
    {
        if (!finaleBegun)
        {
            pause.SetActive(false);

            finaleBegun = true;

            PlayerPrefs.SetInt("High Score", 10000);
            boinCount = PlayerPrefs.GetInt("Boins");
            PlayerPrefs.SetInt("Boins", boinCount + 400);
            PlayerPrefs.Save();
            Manager.instance.depth = 10000;
            Manager.instance.weight = Manager.instance.capacity / 2;
            Manager.instance.freeze = true;
            done = true;

            float offset = -0.9f * bubble.transform.localScale.x + Camera.main.transform.position.y;
            Vector3 offsetVec = new Vector3(0f, offset - 5.8f);
            transform.position += offsetVec;
            bottom.transform.position -= (offsetVec + new Vector3(0f, 0.2f));

            GetComponent<Animator>().Play("End");

            endAudio.ignoreListenerPause = true;
            lpf.enabled = false;
        }
    }

    public void adjustAudio()
    {
        AudioListener.pause = true;
        bubble.SetActive(false);
    }
}
