using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FishObtain : MonoBehaviour {

    public List<GameObject> bubblefish;

    private Death death;

    float startTime;
    bool set;

    AudioSource molest;
    public AudioClip squish;

    Tutorial tut;

    PowerUpObtain PUO;
    int counter;

    private void Start()
    {
        death = GetComponent<Death>();
        molest = GetComponents<AudioSource>()[3];
        GetComponents<AudioSource>()[0].Play();

        tut = FindObjectOfType<Tutorial>();
        PUO = FindObjectOfType<PowerUpObtain>();
    }

    private void Update()
    {
        if (set)
        {
            if (Manager.instance.weight != Manager.instance.capacity)
            {
                set = false;
            }
            else
            {
                if (Time.time - startTime >= 30f )
                {
                    set = false;
                    if (!death.die)
                        Manager.instance.Achievement("Daredevil", "Sink at the max weight for 30 seconds straight");
                }
            }
        }

        if (counter != 0)
        {
            if (!Manager.instance.magnet)
                counter = 0;
        }
    }

    public void Disable(GameObject go)
    {
        go.GetComponent<Wander2>().enabled = false;
        go.GetComponent<Wander2Unit>().enabled = false;

        InBubble ib = go.GetComponent<InBubble>();
        if (ib)
            ib.enabled = true;

        go.GetComponent<Rigidbody2D>().gravityScale = 1f;
        go.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    }

    int AddWeight(string fishname, GameObject item)
    {
        int toAdd;
        
        switch (fishname)
        {
            case "Minnow(Clone)":
                toAdd = 2;
                break;
            case "Trout(Clone)":
                toAdd = 4;
                break;
            case "Pike(Clone)":
                toAdd = 6;
                break;
            case "Bass(Clone)":
                toAdd = 8;
                break;
            case "Tuna(Clone)":
                toAdd = 10;
                break;
            case "Kingfish(Clone)":
                toAdd = 12;
                break;
            case "Grouper(Clone)":
                toAdd = 14;
                break;
            case "Shark(Clone)":
                toAdd = 16;
                break;
            case "Whale(Clone)":
                toAdd = 20;
                break;
            case "Golden Fish(Clone)":
                toAdd = 2;
                break;
            default:
                toAdd = 0;
                break;
        }
        return toAdd;
    }

    void OnCollisionEnter2D (Collision2D col)
    {
        if (col.gameObject.tag == "Fish" && !Manager.instance.drill)
        {

            if (160f * transform.localScale.x > col.gameObject.GetComponent<SpriteRenderer>().sprite.rect.xMax)
            {
                if (PlayerPrefs.GetInt("Tutorial") == 0)
                {
                    tut.obtained++;
                }

                var fish = col.gameObject;
                Manager.instance.weight += AddWeight(fish.name, fish);

                if (Manager.instance.weight <= Manager.instance.capacity)
                {
                    molest.PlayOneShot(squish);

                    fish.transform.position = new Vector3(transform.position.x, transform.position.y, -0.25f);

                    Disable(fish);
                    fish.tag = "InBubble";

                    bubblefish.Add(fish);

                    if (PlayerPrefs.GetInt("Lucky Number 6", 0) == 0)
                    {
                        List<GameObject> unique = bubblefish.GroupBy(x => x.name).Select(x => x.First()).ToList();
                        if (unique.Count() == 6)
                            Manager.instance.Achievement("Lucky Number 6", "Acquire 6 different fish in your bubble");
                    }

                    if (fish.name.Contains("Golden Fish"))
                    {
                        Manager.instance.Achievement("Ecstacy of Gold", "Catch the Golden Fish");
                    }

                    if (Manager.instance.weight == Manager.instance.capacity && PlayerPrefs.GetInt("Daredevil", 0) == 0)
                    {
                        startTime = Time.time;
                        set = true;
                    }

                    if (Manager.instance.magnet && PlayerPrefs.GetInt("Sucker", 0) == 0)
                    {
                        counter++;
                        if (counter == 5)
                            Manager.instance.Achievement("Sucker", "Suck 5 fish with the magnet");
                    }

                    fish.GetComponent<Rigidbody2D>().mass = 0f;
                }
                else
                {
                    StartCoroutine(death.L());
                    Manager.instance.weight = Manager.instance.capacity;
                }
            }
            else
                StartCoroutine(death.L());
        }
    }
}
