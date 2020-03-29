using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    private GameObject Enemy;
    public GameObject Eel;
    public GameObject Lionfish;
    public GameObject Pufferfish;
    public GameObject Stingray;
    public GameObject Swordfish;

    public GameObject[] rockWall;

    public float spawnRate = 8f;
    float nextSpawn = 0.0f;
    public Transform parent;

    void Spawn()
    {
        float spawnY;

        if (!Manager.instance.ledge && !Manager.instance.freeze && !Manager.instance.drill)
            spawnY = (Manager.instance.percentage - 50);
        else if (Manager.instance.drill)
            spawnY = 69f;
        else
            spawnY = 0f;

        float rand = Random.value;
        float y = -spawnY + Random.Range(-10f, 10f) + Camera.main.transform.position.y;

        if (Manager.instance.depth < 2500f)
        {
            if (rand < .3f)
                Enemy = Eel;
            else if (rand < .65f)
                Enemy = Pufferfish;
            else
                Enemy = Swordfish;
        }
        else
        {
            if (rand < .2f)
                Enemy = Eel;
            else if (rand < .4f)
                Enemy = Pufferfish;
            else if (rand < .6f)
                Enemy = Swordfish;
            else if (rand < .8f)
                Enemy = Stingray;
            else
                Enemy = Lionfish;
        }

        bool left = (Random.Range(0, 2) == 0);

        GameObject e;

        if (left)
            e = Instantiate(Enemy, new Vector3(-25, y, -0.5f), Enemy.transform.rotation, parent);
        else
        {
            e = Instantiate(Enemy, new Vector3(25, y, -0.5f), Enemy.transform.rotation, parent);
            e.transform.localScale = new Vector3(e.transform.localScale.x, e.transform.localScale.y * -1, e.transform.localScale.z);
            e.transform.Rotate(0, 0, 180);
        }

        foreach (GameObject rw in rockWall)
            Physics2D.IgnoreCollision(e.GetComponent<Collider2D>(), rw.GetComponent<Collider2D>());
    }

    void Update()
    {
        float spawnConstant;
        if (!Manager.instance.drill)
            spawnConstant = -0.01f * Mathf.Abs(Manager.instance.percentage - 50f) + 1f;
        else
            spawnConstant = 0.25f;
        spawnRate = ((1f / 1000f) * Manager.instance.depth + 6f) * spawnConstant;

        if (Time.time > nextSpawn && Manager.instance.freeze == false)
        {
            nextSpawn = Time.time + spawnRate;

            Spawn();
        }
    }
}
