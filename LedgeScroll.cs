using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeScroll : MonoBehaviour
{
    Death d;

    private void Start()
    {
        d = FindObjectOfType<Death>();
    }

    void Update()
    {
        if (Manager.instance.ledge == false)
        {
            Vector2 scrollPos = new Vector2(0, Time.deltaTime * (Manager.instance.percentage - 50f) / 5f);

            if (Manager.instance.freeze)
                scrollPos.Scale(new Vector2(0.25f, 0.25f));

            transform.position = (Vector2)transform.position + scrollPos;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bubble")
            Destroy(col.gameObject);

        if (col.gameObject.tag == "Player")
        {
            Manager.instance.ledge = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (!d.die)
                Manager.instance.ledge = false;
        }
    }
}
