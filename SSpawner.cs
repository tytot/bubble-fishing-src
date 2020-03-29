using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSpawner : MonoBehaviour {

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

    public float spawnRate;
    float spawnConstant;
    float nextSpawn = 0.0f;

    public Transform parent;

    void Spawn()
    {
        float rand = Random.value;

        if (Manager.instance.depth <= 1000)
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

        float y = 33f * Mathf.Sign(50f - Manager.instance.percentage) + Camera.main.transform.position.y;

        Instantiate(Fish, new Vector3(Random.Range(-20f, 20f), y, -0.25f), Fish.transform.rotation, parent);

        GameObject right = Instantiate(Fish, new Vector3(Random.Range(-20f, 20f), y, -0.25f), Fish.transform.rotation, parent);
        right.transform.localScale = new Vector3(right.transform.localScale.x, right.transform.localScale.y * -1, right.transform.localScale.z);
        right.transform.Rotate(0, 0, 180);
    }

    void Update()
    {
        if (!Manager.instance.drill && !Manager.instance.ledge)
        {
            spawnConstant = -0.1f * Mathf.Abs(Manager.instance.percentage - 50f) + 6f;
        }
        else if (Manager.instance.drill && !Manager.instance.ledge)
            spawnConstant = 0.69f;
        else
            spawnConstant = 6f;

        spawnRate = Mathf.Lerp(spawnConstant, 2f * spawnConstant, Manager.instance.depth / 10000f);

        if (Time.time > nextSpawn && Manager.instance.freeze == false)
        {
            nextSpawn = Time.time + spawnRate;

            Spawn();
        }
    }
}
