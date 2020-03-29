using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeSpawner : MonoBehaviour {

    public GameObject rockLedge;
    public GameObject rightLedgeEnd;
    public GameObject leftLedgeEnd;

    public GameObject bubble;

    public Transform parent;

    public float spawnRate;
    private float nextSpawn = 0.0f;

    public GameObject collapse;

    void Spawn(bool left, int segments, float y)
    {
        int x;
        GameObject prefab;
        if (left)
        {
            x = -1;
            prefab = rightLedgeEnd;
        }
        else
        {
            x = 1;
            prefab = leftLedgeEnd;
        }

        Transform subParent = new GameObject("FullLedge").transform;
        subParent.parent = parent;
        subParent.tag = "Ledge";

        for (int i = 0; i < segments; i++)
        {
            var segment = Instantiate(rockLedge, new Vector3(x * (17.92f - i * 2.56f), y, -0.5f), transform.rotation, subParent);

            Physics2D.IgnoreCollision(bubble.GetComponent<Collider2D>(), segment.GetComponent<PolygonCollider2D>());

            if (i == Mathf.CeilToInt((segments + 1) / 2f) - 1)
            {
                var bc = segment.AddComponent<BoxCollider2D>();
                bc.offset = new Vector2(0f, 0.16f);

                if (segments % 2 == 1)
                    bc.size = new Vector2(2.56f * (segments + 1) + 1.92f, 2.56f);
                else
                    bc.size = new Vector2(2.56f * (segments) + 1.92f, 2.56f);

                bc.isTrigger = true;
            }
        }
        
        if (segments > 0)
            Instantiate(prefab, new Vector3(x * (17.92f - segments * 2.56f), y, -0.5f), transform.rotation, subParent);
    }
    
	// Update is called once per frame
	void Update () {
		if (Time.time > nextSpawn)
        {
            if (!Manager.instance.drill && !Manager.instance.ledge && !Manager.instance.freeze)
                spawnRate = Mathf.Abs(Manager.instance.percentage - 50f) * -0.15f + 9f;
            else if (Manager.instance.drill)
                spawnRate = 1f;
            else
                spawnRate = 9f;

            nextSpawn = Time.time + spawnRate;

            float rand = Random.value;

            if (Manager.instance.ledge == false && Manager.instance.freeze == false)
            {
                bool left;
                if (rand < 0.5f)
                    left = true;
                else
                    left = false;
                Spawn(left, Random.Range(1, Mathf.FloorToInt((20f - bubble.transform.localScale.x) / 2.56f)), 33f * Mathf.Sign(50f - Manager.instance.percentage) + Camera.main.transform.position.y);
            }
        }
	}

    public void Reset(float yPos, Transform collider)
    {
        int index = collider.GetSiblingIndex();
        int cc = collider.parent.childCount;
        bool xPos = collider.transform.position.x < 0f;
        Spawn(xPos, index-1, yPos);
        for (int i = index; i < cc; i++)
        {
            if (i >= 0)
                Instantiate(collapse, collider.parent.GetChild(i).transform.position, Quaternion.identity);
        }
        Destroy(collider.parent.gameObject);
    }
}
