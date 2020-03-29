using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour {

    public GameObject petiteBubble;
    public GameObject smallBubble;
    public GameObject mediumBubble;
    private GameObject bubbletospawn;

    public GameObject bubble;

    public bool changed = false;
    float nextSpawn = 0.0f;
    Vector3 spawnpoint;

    private float width;

    public Transform parent;
    private Tutorial tut;
    
	void Spawn (float x, float y, float spawnRate)
    {
        if (Time.time > nextSpawn && Manager.instance.freeze == false)
        {
            nextSpawn = Time.time + spawnRate;

            spawnpoint = new Vector3(x, y + Camera.main.transform.position.y, -0.5f);

            float rand = Random.value;

            if (rand < .5f)
                bubbletospawn = petiteBubble;
            else if (rand < .85f)
                bubbletospawn = smallBubble;
            else
                bubbletospawn = mediumBubble;

            Instantiate(bubbletospawn, spawnpoint, transform.rotation, parent);
        }
    }

    void Start()
    {
        width = Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad) * Camera.main.aspect;
        tut = FindObjectOfType<Tutorial>();

        Debug.Log(PlayerPrefs.GetInt("Scope"));
    }

    // Update is called once per frame
    void Update()
    {
        float frustumWidth = Mathf.Abs(Camera.main.transform.position.z) * width;

        
        if (!changed)
        {
            float spawnRate;
            if (Manager.instance.percentage > 75f)
                spawnRate = 2f;
            else
                spawnRate = 4f;
            if (PlayerPrefs.GetInt("Tutorial") > 0)
                Spawn(Random.Range(bubble.transform.position.x - frustumWidth, bubble.transform.position.x + frustumWidth), -33f, spawnRate);
            else if (tut.obtained > 0)
                Spawn(Random.Range(bubble.transform.position.x - frustumWidth, bubble.transform.position.x + frustumWidth), -33f, spawnRate);
            else
                Spawn(bubble.transform.position.x, -33f, 0.5f);
        }
        else
        {
            Spawn(bubble.transform.position.x, -10f, 0.5f);
        }
    }
}
