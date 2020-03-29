using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Parallax scrolling script that should be assigned to a layer
/// </summary>
public class Scrolling : MonoBehaviour
{
    /// <summary>
    /// Movement should be applied to camera
    /// </summary>
    public bool isLinkedToCamera = false;

    /// <summary>
    /// 1 - Background is infinite
    /// </summary>
    public bool isLooping = false;

    /// <summary>
    /// 2 - List of children with a renderer.
    /// </summary>
    public List<SpriteRenderer> backgroundPart;

    public AudioLowPassFilter lpf;

    // 3 - Get all the children
    void Start()
    {
        // For infinite background only
        if (isLooping)
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
        }
    }

    void Update()
    {
        lpf.cutoffFrequency = Mathf.Lerp(5000f, 1000f, Manager.instance.depth / 10000f);

        if (!Manager.instance.ledge)
        {
            float speed;

            speed = (Manager.instance.percentage - 50f) / 5f;

            if (Manager.instance.freeze)
                speed *= 0.25f;

            Vector3 movement = new Vector3(0, speed);
            movement *= Time.deltaTime;
            transform.Translate(movement);
        }

        // 4 - Loop
        if (isLooping)
        {
            if (Manager.instance.percentage < 50f)
            {
                // Get the first object.
                // The list is ordered from bottom (y position) to top.
                SpriteRenderer firstChild = backgroundPart.FirstOrDefault();

                if (firstChild != null)
                {
                    // Check if the child is already (partly) before the camera.
                    // We test the position first because the IsVisibleFrom
                    // method is a bit heavier to execute.
                    if (firstChild.transform.position.y < Camera.main.transform.position.y)
                    {
                        // If the child is already below the camera,
                        // we test if it's completely outside and needs to be
                        // recycled.
                        if (firstChild.IsVisibleFrom(Camera.main) == false)
                        {
                            // Get the last child position.
                            SpriteRenderer lastChild = backgroundPart.LastOrDefault();

                            Vector3 lastPosition = lastChild.transform.position;
                            Vector3 lastSize = (lastChild.bounds.max - lastChild.bounds.min);

                            // Set the position of the recyled one to be AFTER
                            // the last child..
                            firstChild.transform.position = new Vector3(firstChild.transform.position.x, lastPosition.y + lastSize.y, firstChild.transform.position.z);

                            // Set the recycled child to the last position
                            // of the backgroundPart list.
                            backgroundPart.Remove(firstChild);
                            backgroundPart.Add(firstChild);
                        }
                    }
                }
            }
            if (Manager.instance.percentage >= 50f)
            {
                // Get the last object.
                // The list is ordered from bottom (y position) to top.
                SpriteRenderer lastChild = backgroundPart.LastOrDefault();

                if (lastChild != null)
                {
                    // Check if the child is already (partly) after the camera.
                    // We test the position first because the IsVisibleFrom
                    // method is a bit heavier to execute.
                    if (lastChild.transform.position.y > Camera.main.transform.position.y)
                    {
                        // If the child is already above the camera,
                        // we test if it's completely outside and needs to be
                        // recycled.
                        if (lastChild.IsVisibleFrom(Camera.main) == false)
                        {
                            // Get the first child position.
                            SpriteRenderer firstChild = backgroundPart.FirstOrDefault();

                            Vector3 firstPosition = firstChild.transform.position;
                            Vector3 firstSize = (firstChild.bounds.max - firstChild.bounds.min);

                            // Set the position of the recyled one to be BEFORE
                            // the first child..
                            lastChild.transform.position = new Vector3(lastChild.transform.position.x, firstPosition.y - firstSize.y, lastChild.transform.position.z);

                            // Set the recycled child to the first position
                            // of the backgroundPart list.
                            backgroundPart.Remove(firstChild);
                            backgroundPart.Add(firstChild);
                        }
                    }
                }
            }
        }
    }
}
