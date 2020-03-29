using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class Drill : MonoBehaviour {

    CameraShake shake;
    public LedgeSpawner reset;

    public Transform drillButt;
    Toggle togg;
    Image upBar;
    Image pBar;

    public float rechargeTime;
    public float drillTime;
    bool startDrill = false;
    float clickTime;
    float endTime = 0;

    AudioSource drill;
    AudioSource rock;
    AudioSource rape;

    FishObtain nig;
    Transform bubble;

    int fishCount = 0;

    public GameObject puff;

    void Start()
    {
        drill = GetComponents<AudioSource>()[0];
        rock = GetComponents<AudioSource>()[1];
        rape = GetComponents<AudioSource>()[2];

        togg = drillButt.GetChild(0).GetComponent<Toggle>();
        upBar = drillButt.GetChild(0).GetChild(1).GetComponent<Image>();
        pBar = drillButt.GetChild(1).GetChild(1).GetComponent<Image>();
        togg.interactable = false;

        if (PlayerPrefs.GetInt("Drill") != 3)
        {
            togg.gameObject.SetActive(false);
        }

        shake = GetComponent<CameraShake>();

        bubble = transform.parent;
        nig = bubble.GetComponent<FishObtain>();
    }

    // Update is called once per frame
    void Update () {
        if (upBar.fillAmount == 1f)
        {
            togg.interactable = true;
        }

        if (startDrill)
        {
            transform.parent.transform.rotation = Quaternion.Euler(Vector3.zero);
            pBar.fillAmount = 1f - ((Time.time - clickTime) / drillTime);

            foreach (GameObject nigbob in nig.bubblefish)
            {
                nigbob.transform.position = bubble.transform.position;
            }

            if (Time.time > clickTime + drillTime)
            {
                StartDrill(false);
            }
        }
        else if (!startDrill && upBar.fillAmount < 1f)
        {
            upBar.fillAmount = (Time.time - endTime) / rechargeTime;
        }
    }

    public void StartDrill (bool val)
    {
        if ((val && upBar.fillAmount == 1f) || (!val && pBar.fillAmount == 0f))
        {
            upBar.transform.parent.gameObject.SetActive(!val);
            pBar.transform.parent.gameObject.SetActive(val);
            Manager.instance.drill = val;
            startDrill = val;
            gameObject.GetComponent<SpriteRenderer>().enabled = val;
            gameObject.GetComponent<Collider2D>().enabled = val;

            if (val)
            {
                clickTime = Time.time;

                shake.StartShake(drillTime);
                togg.interactable = false;
                drill.Play();
            }
            else
            {
                upBar.fillAmount = 0.99f;
                fishCount = 0;

                endTime = Time.time;

                togg.interactable = false;
                togg.isOn = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject col = collision.gameObject;
        if (!col.CompareTag("InBubble") && !col.CompareTag("Untagged") && !col.CompareTag("Bottom"))
        {
            Destroy(col);

            if (col.CompareTag("Fish"))
            {
                fishCount++;
                if (fishCount == 7)
                    Manager.instance.Achievement("Not a Drill", "Kill 7 fish with 1 drill");
            }

            if (col.CompareTag("Ledge"))
            {
                reset.Reset(col.transform.position.y, collision.transform);
                rock.Play();
            }
            else
            {
                Vector3 colPos = collision.GetContact(0).point;
                GameObject nigPuff = Instantiate(puff, colPos, Quaternion.identity);
                int scale;
                switch (col.name)
                {
                    case "Minnow(Clone)":
                        scale = 8;
                        break;
                    case "Trout(Clone)":
                        scale = 10;
                        break;
                    case "Pike(Clone)":
                        scale = 12;
                        break;
                    case "Bass(Clone)":
                        scale = 14;
                        break;
                    case "Tuna(Clone)":
                        scale = 16;
                        break;
                    case "Kingfish(Clone)":
                        scale = 18;
                        break;
                    case "Grouper(Clone)":
                        scale = 20;
                        break;
                    case "Shark(Clone)":
                        scale = 22;
                        break;
                    case "Whale(Clone)":
                        scale = 24;
                        break;
                    case "Golden Fish(Clone)":
                        scale = 26;
                        break;
                    case "Swordfish(Clone)":
                        scale = 10;
                        break;
                    case "Lionfish(Clone)":
                        scale = 20;
                        break;
                    default:
                        scale = 8;
                        break;
                }
                nigPuff.transform.localScale = new Vector3(scale, scale);
                rape.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ledge"))
        {
            reset.Reset(collision.gameObject.transform.position.y, collision.transform);
            rock.Play();
        }
    }
}
