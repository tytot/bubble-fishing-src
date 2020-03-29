using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    Transform[] clouds = new Transform[3];
    float[] velocity = { 0f, 0f, 0f };
    int available = 3;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            clouds[i] = transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.value < (0.1f * Time.deltaTime) && available != 0)
        {
            StartCloud();
        }

        for (int i = 0; i < 3; i++)
        {
            clouds[i].Translate(velocity[i], 0f, 0f, Space.Self);
        }
    }

    void StartCloud()
    {
        int index;
        if (velocity[0] == 0f) index = 0;
        else if (velocity[1] == 0f) index = 1;
        else index = 2;

        bool left = Random.value < 0.5f;
        clouds[index].localPosition = new Vector3(left ? -0.4f : 0.4f, Random.Range(-0.1f, 0.2f), Random.Range(-1f, 5f));
        velocity[index] = Random.Range(0.002f, 0.02f) * (left ? 1f : -1f);
        available--;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        int index = int.Parse(col.name.Substring(7));
        velocity[index] = 0f;
        available++;
    }
}
