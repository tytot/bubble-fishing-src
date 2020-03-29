using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {

    public float spawnRate = 6f;
    float nextSpawn = 0.0f;

    public GameObject[] cliffs;

    // Use this for initialization
    void Spawn () {
        float rand = Random.value;
        GameObject thing;

        if (rand < 0.25)
            thing = cliffs[0];
        else if (rand < 0.5)
            thing = cliffs[1];
        else if (rand < 0.75)
            thing = cliffs[2];
        else
            thing = cliffs[3];

        float y = 48f * Mathf.Sign(50f - Manager.instance.percentage) + Camera.main.transform.position.y;

        Instantiate(thing, new Vector3(0f, y, -0.25f), Quaternion.identity, transform);
    }

    // Update is called once per frame
    void Update() {
        if (Time.time > nextSpawn && Manager.instance.freeze == false)
        {
            nextSpawn = Time.time + spawnRate;

            if (transform.childCount <= 1 && Manager.instance.depth > 50)
                Spawn();
        }

        Vector3 scrollPos = new Vector2(0, Time.deltaTime * (Manager.instance.percentage - 50f) / Random.Range(10f, 20f));

        if (Manager.instance.ledge == false && Manager.instance.freeze == false)
        { 
            foreach (Transform child in transform)
            {
                child.transform.position += scrollPos;
                if (Mathf.Abs(child.transform.position.y) > 48)
                    Destroy(child.gameObject);
            }
        }
    }
}
