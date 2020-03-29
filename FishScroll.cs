using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScroll : MonoBehaviour {

    private Rigidbody2D rb;
    Obtainable ob;
    FishObtain fo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ob = FindObjectOfType<Obtainable>();
        fo = FindObjectOfType<FishObtain>();
    }

    void FixedUpdate()
    {
        Vector2 scrollPos = Vector2.zero;

        if (!Manager.instance.ledge)
        {
            if (tag == "Fish" || tag == "Enemy")
                scrollPos = new Vector2(0, Time.deltaTime * (Manager.instance.percentage - 50f) / 5f);
            else
                scrollPos = new Vector2(0, Time.deltaTime * (Manager.instance.percentage - 50f) / Random.Range(10f, 20f));

            if (Manager.instance.freeze)
                scrollPos.Scale(new Vector2(0.25f, 0.25f));
        }

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.position += scrollPos;
            if (Manager.instance.freeze && tag != "InBubble")
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        else
            transform.position += (Vector3)scrollPos;
    }

    void OnDestroy()
    {
        ob.myDic.Remove(gameObject);
        fo.bubblefish.Remove(gameObject);
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
