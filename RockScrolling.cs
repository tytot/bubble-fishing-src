using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RockScrolling : MonoBehaviour {

    public Scrolling waterscroll;

    private List<SpriteRenderer> backgroundPart;

    private SpriteRenderer firstwall;
    private SpriteRenderer secondwall;

    // 3 - Get all the children
    void Start()
    {
        // Get all the children of the layer with a renderer
        backgroundPart = new List<SpriteRenderer>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            SpriteRenderer r = child.GetComponent<SpriteRenderer>();

            // Add only the visible children
            if (r != null)
            {
                backgroundPart.Add(r);
            }
        }

        // Sort by position.
        // Note: Get the children from bottom to top.
        // We would need to add a few conditions to handle
        // all the possible scrolling directions.
        backgroundPart = backgroundPart.OrderBy(
            t => t.transform.position.y
        ).ToList();

        firstwall = backgroundPart.FirstOrDefault();
        secondwall = backgroundPart.LastOrDefault();
    }

    void Update()
    {
        firstwall.transform.position = new Vector3(transform.position.x, waterscroll.backgroundPart.FirstOrDefault().transform.position.y, -0.5f);
        secondwall.transform.position = new Vector3(transform.position.x, waterscroll.backgroundPart.LastOrDefault().transform.position.y, -0.5f);
    }
}
