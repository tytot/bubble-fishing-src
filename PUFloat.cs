using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUFloat : MonoBehaviour {

    public float moveSpeed;
    Vector3 randVel;
    Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        randVel = new Vector3(Random.Range(-1f, 1f) * moveSpeed * Time.deltaTime, Random.Range(-1f, 1f) * moveSpeed * Time.deltaTime);
        rb = GetComponent<Rigidbody2D>();
	}
	
    void SetVelocity()
    {
        randVel.x += Random.Range(-1f, 1f) * moveSpeed * Time.deltaTime;
        randVel.y += Random.Range(-1f, 1f) * moveSpeed * Time.deltaTime;
    }

	// Update is called once per frame
	void FixedUpdate () {
        if (!Manager.instance.freeze)
        {
            SetVelocity();
            rb.velocity = randVel;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
	}
}
