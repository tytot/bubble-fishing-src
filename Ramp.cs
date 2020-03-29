using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class Ramp : MonoBehaviour {

    private Rigidbody2D rb;
    public float sinkSpeed;

    private Vector3 topPos;
    private bool splash = false;

    public Text signText;
    public Light lt;

    private AudioSource[] sounds;
    public GameObject soundPlayer;

    public HingeJoint2D hinge;

    private int launcher;
    public GameObject[] launchers;

    public GameObject deLight;
    public GameObject drill;

    public Animator fishNigbobs;
    public SpriteRenderer[] introFish;
    public Sprite[] nigberts;
    public PostProcessVolume pplmao;

    bool tutorial;
    int depth;

    IEnumerator WaitLMAO()
    {
        rb.isKinematic = true;
        yield return new WaitForSeconds(1f);
        rb.isKinematic = false; ;

    }
    // Use this for initialization
    void Start()
    {
        MotionBlur blur;
        pplmao.profile.TryGetSettings(out blur);

        rb = GetComponent<Rigidbody2D>();

        sounds = soundPlayer.GetComponents<AudioSource>();

        tutorial = PlayerPrefs.GetInt("Tutorial") == 0;
        if (tutorial)
        {
            transform.position = new Vector3(0.1f, 150f, -0.5f);
            Camera.main.transform.position = new Vector3(0.1f, 150f, -10f);
            blur.shutterAngle.value = 50f;
            topPos = new Vector3(0f, 120f, -0.5f);
            StartCoroutine(WaitLMAO());
        }
        else
        {
            launcher = PlayerPrefs.GetInt("Launcher", 1);
            launchers[launcher - 1].SetActive(true);
            if (launcher == 1)
            {
                blur.shutterAngle.value = 75f;
            }
            if (launcher == 2)
            {
                introFish[0].sprite = nigberts[2];
                introFish[1].sprite = nigberts[2];
                introFish[2].sprite = nigberts[3];
                blur.shutterAngle.value = 150f;
            }
            if (launcher == 3)
            {
                introFish[0].sprite = nigberts[4];
                introFish[1].sprite = nigberts[4];
                introFish[2].sprite = nigberts[5];
                blur.shutterAngle.value = 225f;
            }
            if (launcher == 4)
            {
                introFish[0].sprite = nigberts[6];
                introFish[1].sprite = nigberts[6];
                introFish[2].sprite = nigberts[7];
                launchers[4].SetActive(true);
                blur.shutterAngle.value = 300f;
            }
        }
        
        if (PlayerPrefs.GetInt("Light Type") == 0)
        {
            deLight.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Drill") != 3)
        {
            drill.SetActive(false);
        }
        
        if (tutorial)
            depth = 50;
        else if (launcher == 1)
            depth = 100;
        else if (launcher == 2)
            depth = 200;
        else if (launcher == 3)
            depth = 500;
        else
            depth = 1000;
    }

    void Configure(Animator l, float min, string animName, float time, Vector3 force, Vector2 position)
    {
        AnimatorStateInfo info = l.GetCurrentAnimatorStateInfo(0);

        if (transform.position.y < min)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;

            l.SetTrigger("placed");

            if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetButtonDown("Fire1"))
            {
                l.SetTrigger("click");
                l.GetComponent<AudioSource>().enabled = true;
            }
        }
        
        if (info.IsName(animName) && info.normalizedTime > time)
        {
            splash = true;

            rb.constraints = RigidbodyConstraints2D.None;
            transform.position = position;
            rb.AddForce(force);
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void Update()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetButtonDown("Fire1"))
        {
            if (launcher == 3 && transform.position.y < 128.95)
            {
                rb.AddForce(new Vector3(1f, 2f).normalized * 144);
                JointMotor2D motor = hinge.motor;
                motor.motorSpeed = 600;
                hinge.motor = motor;

                launchers[2].GetComponents<AudioSource>()[0].enabled = true;
                splash = true;
            }
        }

        if (tutorial && transform.position.y <= 120f)
        {
            splash = true;
            tutorial = false;
        }

        if (splash)
        {
            if (transform.position.x > 0f)
            {
                Destroy(rb);
                Destroy(GetComponent<CircleCollider2D>());

                transform.position = new Vector3(0f, transform.position.y, -0.5f);
                topPos = transform.position;

                sounds[1].Play();
                sounds[2].Play();

                fishNigbobs.Play("Intro", 0);
            }

            if (transform.position.x < 0f)
            {
                if (launcher > 1)
                    transform.Rotate(Vector3.back * 8f);
            }
            if (transform.position.x == 0f)
            {
                Quaternion target = Quaternion.Euler(Vector3.zero);
                transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, new Vector3(0f, 0f, -0.5f), sinkSpeed * Time.deltaTime);

                float introMeters = (-depth / 120f) * transform.position.y + depth;
                signText.text = Mathf.Round(introMeters) + " m";
                lt.intensity = -0.001f * introMeters + 1f;

                if (Mathf.Round(introMeters) == depth)
                    SceneManager.LoadScene("Main");
            }
        }
        else
        {
            if (launcher == 2)
                Configure(launchers[1].GetComponent<Animator>(), 130.95f, "slingshotlaunch", .99f, new Vector3(2f, 3f).normalized * 132f, transform.position);
            else if (launcher == 4)
                Configure(launchers[3].GetComponent<Animator>(), 131.5f, "cannonshoot", .5f, new Vector3(1f, 3f).normalized * 160f, new Vector2(-44f, 132f));
        }
    }

    void LateUpdate()
    {
        float z;
        float max = (22f / (Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad) * Camera.main.aspect)) / (3.75f - 0.75f * PlayerPrefs.GetInt("Scope", 1));

        if (transform.position.x == 0f && transform.position.y <= 120f)
            z = ((max - 14) / topPos.y) * transform.position.y - max;
        else
            z = Mathf.Lerp(Camera.main.transform.position.z, -14f, 2f * Time.deltaTime);
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (launcher == 1)
            rb.AddForce(new Vector3(1f, 1f).normalized * 15);
        splash = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (launcher == 1 && !sounds[0].isPlaying)
            sounds[0].Play(0);

        if (launcher == 3)
            launchers[2].GetComponents<AudioSource>()[1].enabled = true;
    }

    public void Skip()
    {
        SceneManager.LoadScene("Main");
        if (!sounds[2].isPlaying)
        {
            sounds[0].Stop();
            sounds[1].Stop();
            sounds[2].Play();
        }
    }
}
