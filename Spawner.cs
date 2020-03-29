using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    private GameObject Fish;
    public GameObject Bass;
    public GameObject GoldenFish;
    public GameObject Grouper;
    public GameObject Kingfish;
    public GameObject Minnow;
    public GameObject Pike;
    public GameObject Shark;
    public GameObject Trout;
    public GameObject Tuna;
    public GameObject Whale;

    public GameObject[] rockWall;
    
    public float spawnRate = 2f;
    float nextSpawn = 0.0f;
    public Transform parent;

    public Obtainable ob;

    void Spawn()
    {
        float spawnY;

        if (!Manager.instance.drill)
        {
            if (!Manager.instance.ledge && !Manager.instance.freeze)
                spawnY = (Manager.instance.percentage - 50);
            else
                spawnY = 0f;
        }
        else
        {
            spawnY = 69f;
        }

        float rand = Random.value;
        float y = -spawnY + Random.Range(-10f, 10f) + Camera.main.transform.position.y;

        if (PlayerPrefs.GetInt("Tutorial") == 0)
        {
            Fish = Minnow;
        }
        else if (Manager.instance.depth <= 1000)
        {
            if (rand < .75f)
                Fish = Minnow;
            else
                Fish = Trout;
        }
        else if (Manager.instance.depth <= 2000)
        {
            if (rand < .25f)
                Fish = Minnow;
            else if (rand < .75f)
                Fish = Trout;
            else
                Fish = Pike;
        }
        else if (Manager.instance.depth <= 3000)
        {
            if (rand < .25f)
                Fish = Trout;
            else if (rand < .75f)
                Fish = Pike;
            else
                Fish = Bass;
        }
        else if (Manager.instance.depth <= 4000)
        {
            if (rand < .25f)
                Fish = Pike;
            else if (rand < .75f)
                Fish = Bass;
            else
                Fish = Tuna;
        }
        else if (Manager.instance.depth <= 5000)
        {
            if (rand < .25f)
                Fish = Bass;
            else if (rand < .75f)
                Fish = Tuna;
            else
                Fish = Kingfish;
        }
        else if (Manager.instance.depth <= 6000)
        {
            if (rand < .25f)
                Fish = Tuna;
            else if (rand < .75f)
                Fish = Kingfish;
            else
                Fish = Grouper;
        }
        else if (Manager.instance.depth <= 7000)
        {
            if (rand < .25f)
                Fish = Kingfish;
            else if (rand < .75f)
                Fish = Grouper;
            else
                Fish = Shark;
        }
        else if (Manager.instance.depth <= 8000)
        {
            if (rand < .25f)
                Fish = Grouper;
            else if (rand < .75f)
                Fish = Shark;
            else
                Fish = Whale;
        }
        else if (Manager.instance.depth <= 9000)
        {
            if (rand < .25f)
                Fish = Shark;
            else if (rand < .75f)
                Fish = Whale;
            else
                Fish = GoldenFish;
        }
        else if (Manager.instance.depth <= 10000)
        {
            if (rand < .9f)
                Fish = Whale;
            else
                Fish = GoldenFish;
        }

        float LR = Random.value;
        GameObject f;

        if (LR < .5f)
            f = Instantiate(Fish, new Vector3(-25, y, -0.25f), Fish.transform.rotation, parent);
        else
        {
            f = Instantiate(Fish, new Vector3(25, y, -0.25f), Fish.transform.rotation, parent);
            f.transform.localScale = new Vector3(f.transform.localScale.x, f.transform.localScale.y * -1, f.transform.localScale.z);
            f.transform.Rotate(0, 0, 180);
        }

        ob.myDic.Add(f, true);
        ob.check(f);

        foreach (GameObject rw in rockWall)
            Physics2D.IgnoreCollision(f.GetComponent<Collider2D>(), rw.GetComponent<Collider2D>());
    }

    void Update()
    {
        float spawnConstant;
        if (!Manager.instance.drill)
            spawnConstant = -0.01f * Mathf.Abs(Manager.instance.percentage - 50f) + 1f;
        else
            spawnConstant = 0.25f;
        if (Manager.instance.percentage > 75f)
            spawnConstant *= 1.5f;
        else
            spawnConstant *= 0.69f;

        spawnRate = ((1f / 2000f) * Manager.instance.depth + 2f) * spawnConstant;

        if (Time.time > nextSpawn && Manager.instance.freeze == false)
        {
            nextSpawn = Time.time + spawnRate;

            Spawn();
        }

        transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Contains("Fish") || col.gameObject.tag == "Bubble" || col.gameObject.tag == "PowerUp" || col.gameObject.tag == "Enemy")
        {
            Destroy(col.gameObject);
        }
        else if (col.gameObject.tag.Contains("Ledge"))
        {
            Destroy(col.transform.parent.gameObject);
        }
    }
}
