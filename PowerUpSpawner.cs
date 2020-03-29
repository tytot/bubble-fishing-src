using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour {

    public GameObject timeFreeze;
    public GameObject bubblePotion;
    public GameObject fastFoward;
    public GameObject magnet;
    public GameObject zap;

    public float spawnRate;
    float nextSpawn = 0.0f;
    Vector3 spawnpoint;

    public Transform parent;

    void Spawn()
    {
        GameObject poweruptospawn;
        float rand = Random.value;
        float y;

        if (Manager.instance.percentage != 50f) {
            if (rand < .2f)
                poweruptospawn = bubblePotion;
            else if (rand < .4f)
                poweruptospawn = timeFreeze;
            else if (rand < .6f)
                poweruptospawn = zap;
            else if (rand < .8f)
                poweruptospawn = fastFoward;
            else
                poweruptospawn = magnet;
            y = 33f * Mathf.Sign(50f - Manager.instance.percentage) + Camera.main.transform.position.y;
        }
        else
        {
            if (rand < 1f / 3f)
                poweruptospawn = bubblePotion;
            else if (rand < 2f / 3f)
                poweruptospawn = zap;
            else
                poweruptospawn = magnet;
            y = Camera.main.transform.position.y;
        }

        Instantiate(poweruptospawn, new Vector3(Random.Range(-20f, 20f), y, -0.5f), poweruptospawn.transform.rotation, parent);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Manager.instance.drill && !Manager.instance.ledge)
        {
            if (Manager.instance.percentage == 50f)
                spawnRate = -0.06f * Mathf.Abs(Manager.instance.percentage - 50f) + 4.5f;
            else
                spawnRate = 4.5f;
        }
        else if (Manager.instance.drill && !Manager.instance.ledge)
            spawnRate = 1f;
        else
            spawnRate = 9f;

        if (Time.time > nextSpawn && Manager.instance.freeze == false)
        {
            nextSpawn = Time.time + spawnRate;

            Spawn();
        }
    }
}

