using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBubble : MonoBehaviour {

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject cr = collision.gameObject;
        if (CompareTag("InBubble"))
        {
            if (cr.CompareTag("Player"))
            {
                rb.position = Vector2.Lerp(rb.position, cr.transform.position, 0.1f);
            }
            else if (cr.CompareTag("Sky"))
            {
                Physics2D.IgnoreCollision(cr.GetComponents<BoxCollider2D>()[1], GetComponent<Collider2D>());
                GetComponent<SpriteRenderer>().sortingOrder = 5;
            }
        }
    }
}
