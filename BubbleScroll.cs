using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleScroll : MonoBehaviour {

    private Rigidbody2D rb;
    public float speed;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate ()
    {
        Vector2 scrollPos;

        if (Manager.instance.ledge == false && Manager.instance.freeze == false)
        {
            float velocity = (Manager.instance.percentage - 50f) / 5f;
            scrollPos = new Vector2(0, (speed + velocity) * Time.deltaTime);
        }
        else if (Manager.instance.freeze)
        {
            float velocity = (Manager.instance.percentage - 50f) / 20f;
            scrollPos = new Vector2(0, velocity * Time.deltaTime);
        }
        else
        {
            scrollPos = new Vector2(0, speed * Time.deltaTime);
        }

        rb.position += scrollPos;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sky")
            Destroy(gameObject);
    }
}
