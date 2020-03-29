using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateShooter : MonoBehaviour {
    public float speed = 30f;
    private Vector3 zvector;

    // Use this for initialization
    void Start () {
        zvector = new Vector3(0, 0, 1);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(zvector * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(-zvector * speed * Time.deltaTime);
        }
    }
}
