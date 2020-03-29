using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Death : MonoBehaviour {

    private Rigidbody2D rb;
    private Animator anim;
    private Animator nine;
    public Animator disappear;

    public GameObject sky;
    public GameObject[] LBoys;
    public GameObject tryAgain;
    public AudioSource music;
    public AudioSource death;

    public float zoomSpeed;
    
    private int highScore;

    public bool die;
    private bool hit;

    public CameraShake zap;
    public GameObject go;
    private AudioSource shock;

    private FishObtain fishObtain;

    public Toggle drill;

    public TheEnd heh;
    Interstitial sti;

    AudioSource obtain;
    public AudioClip getBoin;
    int boinCount;
    
    public GameObject tut;

    bool retried = false;
    int succ = -1;

    int startDepth;

    private SpriteRenderer bub;
    private SpriteRenderer shield;
    float cooldown = 3.0f;
    bool decrease = false;

    private Leaderboard l;

    public IEnumerator L()
    {
        if (shield.enabled)
        {
            if (Manager.instance.depth > 0f)
            {
                GetComponent<PowerUpObtain>().stopShield = true;
                shield.enabled = false;
                decrease = true;
                bub.color = new Color(1f, 190f / 255f, 190f / 255f);
            }
        }

        if (decrease)
            yield break;

        int done = PlayerPrefs.GetInt("Tutorial");

        tut.SetActive(false);
        Vector3 startPos = Camera.main.transform.position;
        BeginDeath(false, Vector3.zero);

        yield return new WaitForSeconds(0.5f);

        bub.enabled = false;

        yield return new WaitForSeconds(2f);

        if (!retried && done != 0)
        {
            LBoys[13].SetActive(true);
            do
            {
                yield return null;
            } while (succ == -1);
            if (succ == 1)
            {
                yield return new WaitForSeconds(0.3f);
                LateralMovement lm = GetComponent<LateralMovement>();
                retried = true;
                BeginDeath(true, startPos);
                AudioSource restart = LBoys[10].GetComponent<AudioSource>();
                restart.GetComponent<Toggle>().interactable = false;
                restart.GetComponent<AudioSource>().PlayOneShot(restart.clip);
                Manager.instance.weight = Manager.instance.capacity / 2;

                Manager.instance.freeze = true;
                lm.enabled = false;
                disappear.Play("Countdown");
                
                yield return new WaitForSeconds(3f);
                restart.GetComponent<Toggle>().interactable = true;
                Manager.instance.freeze = false;
                Manager.instance.weight = 0;
                lm.enabled = true;
                GetComponents<AudioSource>()[0].Play();
                yield break;
            }
        }

        LBoys[1].SetActive(true);

        if (done == 0)
        {
            tryAgain.SetActive(true);
            Manager.instance.weight = 1;
            Manager.instance.capacity = 2;
            yield break;
        }
        else
        {
            disappear.Play("Death");
            LBoys[8].GetComponent<Animator>().Play("BoinsTrue");

            yield return new WaitForSeconds(2f);

            if (done == 2)
                LBoys[2].SetActive(true);
            LBoys[3].SetActive(true);
            if (done == 1)
            {
                RectTransform rt = LBoys[3].GetComponent<RectTransform>();
                rt.position = new Vector2(Screen.width / 2f, rt.position.y);
            }

            LBoys[14].GetComponent<Image>().enabled = true;

            l.PostScore();
            if ((int)Manager.instance.depth > highScore)
            {
                PlayerPrefs.SetInt("High Score", (int)Manager.instance.depth);
                PlayerPrefs.Save();
                LBoys[6].SetActive(true);
            }
            else
                LBoys[7].SetActive(true);

            death.mute = true;

            int counter = PlayerPrefs.GetInt("Ad Counter") + 1;
            PlayerPrefs.SetInt("Ad Counter", counter);
            if (counter == 3)
            {
                PlayerPrefs.SetInt("Ad Counter", 0);
                sti.ShowRewardedVideo();
                Time.timeScale = 0f;
            }

            int boinsToAdd = 0;
            if (done == 1)
            {
                if (Manager.instance.depth >= 500f)
                    boinsToAdd = 2 * (int)(Manager.instance.depth / 100);
                else
                    boinsToAdd = 10;
            }
            else if (Manager.instance.depth >= startDepth)
            {
                boinsToAdd = 2 * PlayerPrefs.GetInt("Launcher", 1) * ((int)((Manager.instance.depth - startDepth) / 100f) + 1);
            }

            if (boinsToAdd != 0)
            {
                LBoys[9].SetActive(true);
                LBoys[9].GetComponent<Text>().text = "+" + boinsToAdd.ToString();
                boinCount = PlayerPrefs.GetInt("Boins");
                PlayerPrefs.SetInt("Boins", boinCount + boinsToAdd);
            }

            yield break;
        }
    }

    void BeginDeath(bool reversed, Vector3 ogPos)
    {
        drill.enabled = reversed;
        LBoys[10].SetActive(reversed);
        transform.GetChild(0).gameObject.SetActive(reversed);
        Manager.instance.ledge = !reversed;

        die = !reversed;
        Camera.main.GetComponent<CameraMovement>().enabled = reversed;
        if (!reversed)
        {
            anim.SetTrigger("Pop");

            rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            death.Play();
        }
        else
        {
            Camera.main.transform.position = ogPos;

            bub.enabled = true;
            anim.SetTrigger("Respawn");

            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            death.Stop();
        }
        
        LBoys[0].SetActive(!reversed);
        music.enabled = reversed;

        GetComponent<PolygonCollider2D>().enabled = reversed;
    }

    void Start()
    {
        l = FindObjectOfType<Leaderboard>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        nine = LBoys[9].GetComponent<Animator>();
        bub = GetComponent<SpriteRenderer>();
        shield = transform.GetChild(1).GetComponent<SpriteRenderer>();

        fishObtain = GetComponent<FishObtain>();

        AudioSource[] sounds = go.GetComponents<AudioSource>();
        shock = sounds[0];

        highScore = PlayerPrefs.GetInt("High Score", 0);

        obtain = LBoys[8].GetComponent<AudioSource>();

        switch (PlayerPrefs.GetInt("Launcher", 1))
        {
            case 1:
                startDepth = 100;
                break;
            case 2:
                startDepth = 200;
                break;
            case 3:
                startDepth = 500;
                break;
            case 4: startDepth = 1000;
                break;
        }

        sti = FindObjectOfType<Interstitial>();
    }

    void Update()
    {
        if (LBoys[9].activeSelf && nine.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            if (boinCount != PlayerPrefs.GetInt("Boins"))
            {
                boinCount += 1;
                LBoys[8].GetComponent<Text>().text = boinCount.ToString("D4");
                obtain.PlayOneShot(getBoin);
            }
        }

        if (Manager.instance.depth < (0.2f / Time.deltaTime))
        {
            if (!sky.activeSelf)
            {
                sky.SetActive(true);
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), sky.GetComponents<BoxCollider2D>()[1]);
            }

            if (hit == false && Manager.instance.freeze == false && Manager.instance.ledge == false)
            {
                float speed = (Manager.instance.percentage - 50f) / 5f;
                Vector3 movement = new Vector3(0, speed);

                movement *= Time.deltaTime;
                sky.transform.Translate(movement);
            }
        }
        else
        {
            if (sky.activeSelf)
                sky.SetActive(false);
        }

        if (decrease)
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0.0f)
            {
                decrease = false;
                cooldown = 3.0f;
                bub.color = Color.white;
            }
        }
    }

    void LateUpdate()
    {
        if (die)
        {
            Vector3 zoom = new Vector3(transform.position.x, transform.position.y, -8f * transform.localScale.x);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, zoom, zoomSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!Manager.instance.drill)
        {
            if (col.gameObject.tag == "Sky")
            {
                hit = true;
                StartCoroutine(L());
            }

            if (col.gameObject.name.Contains("Swordfish"))
            {
                StartCoroutine(L());
            }

            if (col.gameObject.name.Contains("Eel") || col.gameObject.name.Contains("Stingray"))
            {
                foreach (GameObject peasant in fishObtain.bubblefish)
                    Destroy(peasant);

                zap.StartShake(0.5f);
                shock.Play(0);

                fishObtain.Disable(col.gameObject);

                Manager.instance.weight = 0;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject collider = col.gameObject;

        if (!Manager.instance.drill)
        {
            if (collider.name.Contains("Lionfish") || collider.name.Contains("Pufferfish"))
            {
                StartCoroutine(L());
            }
        }

        if (collider.tag == "Bottom")
        {
            heh.Finale();
        }
    }

    public void setSucc(int val)
    {
        succ = val;
    }
}
