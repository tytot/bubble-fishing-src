using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerateTest : MonoBehaviour
{

    int FixedUpdatesPerFrame = 0;
    void FixedUpdate()
    {
        FixedUpdatesPerFrame++;
    }


    void Update()
    {
        Debug.Log("FixedUpdatesPerFrame: " + FixedUpdatesPerFrame);
        FixedUpdatesPerFrame = 0;
    }
}