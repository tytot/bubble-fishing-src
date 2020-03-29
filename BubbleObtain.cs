using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleObtain : MonoBehaviour {

    public int NumEdges;
    public float SpeedOfGrowth;
    private int AmountAdded;
    public Vector3 endPos;
    float yPos = 0f;
    
    private SpriteRenderer thissprite;
    public Sprite sprite1;
    public Sprite sprite2;

    private Animator anim;

    AudioSource glug;
    public AudioClip petitGlug;
    public AudioClip smallGlug;
    public AudioClip mediumGlug;

    Tutorial tut;
    int counter;

    private Animator shield;

    void Start()
    {
        thissprite = GetComponent<SpriteRenderer>();
        endPos = transform.localScale;

        anim = GetComponent<Animator>();

        glug = GetComponents<AudioSource>()[2];

        tut = FindObjectOfType<Tutorial>();
        shield = transform.GetChild(1).GetComponent<Animator>();
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, endPos, SpeedOfGrowth * Time.deltaTime);

        if (Manager.instance.ledge || Manager.instance.freeze)
        {
            if (Manager.instance.percentage < 50f)
            {
                Vector3 mover = new Vector3(transform.position.x, yPos);
                transform.position = Vector3.Lerp(transform.position, mover, SpeedOfGrowth * Time.deltaTime);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 0f), SpeedOfGrowth * Time.deltaTime);
            yPos = transform.position.y;
        }

        if (Manager.instance.drill && transform.position.y != yPos)
        {
            transform.position = new Vector3(transform.position.x, yPos);
        }

        if (counter != 0)
        {
            if (!Manager.instance.freeze)
                counter = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (transform.localScale.x >= 4f)
        {
            anim.SetTrigger("Grow");
            shield.SetTrigger("Grow");
        }

        if (col.gameObject.tag.Contains("Bubble") && !Manager.instance.drill)
        {
            if (PlayerPrefs.GetInt("Tutorial") == 0)
                tut.obtained++;
            var bubble = col.gameObject;

            if (bubble.name.Contains("Petite"))
            {
                AmountAdded = 1;
                glug.PlayOneShot(petitGlug);
            }

            if (bubble.name.Contains("Small"))
            {
                AmountAdded = 2;
                glug.PlayOneShot(smallGlug);
            }

            if (bubble.name.Contains("Medium"))
            {
                AmountAdded = 4;
                glug.PlayOneShot(mediumGlug);
            }

            if (Manager.instance.freeze && PlayerPrefs.GetInt("Microwave", 0) == 0)
            {
                counter++;
                if (counter == 3)
                    Manager.instance.Achievement("Microwave", "Get 3 bubbles with the freeze power-up");
            }

            Manager.instance.capacity += AmountAdded;
            float scaleFactor = Mathf.Sqrt(Manager.instance.capacity / 4f);
            endPos = new Vector3(scaleFactor, scaleFactor);
            if (Manager.instance.ledge && Manager.instance.percentage < 50f)
                yPos -= 0.9f * (scaleFactor - Mathf.Sqrt((Manager.instance.capacity - AmountAdded)/ 4f));
            Destroy(bubble);
        }
    }
}
