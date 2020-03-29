using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScoll : MonoBehaviour
{
    bool start = false;
    int nagbo;
    float added = 0f;
    Transform ledge;

    void Start()
    {
        int launcher = PlayerPrefs.GetInt("Launcher");
        nagbo = (launcher - 1) * 3;
        if (PlayerPrefs.GetInt("Tutorial") == 0)
            nagbo = 0;

        ledge = transform.GetChild(3);
    }

    private void Update()
    {
        if (start && nagbo != 0)
            added += nagbo * Time.deltaTime;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (start && nagbo != 0)
        {
            for (int i = 0; i < 3; i++)
            {
                Transform child = transform.GetChild(i);
                child.transform.position += new Vector3(0f, added);
            }
            ledge.transform.position += new Vector3(0f, nagbo * Time.deltaTime);
        }
    }

    public void SetStart()
    {
        start = true;
    }
}
