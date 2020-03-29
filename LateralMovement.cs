using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LateralMovement : MonoBehaviour {

    public int force;
    private Rigidbody2D rb;
    
    bool leftPressed;
    bool rightPressed;

    public GameObject L;
    public GameObject R;

    bool tiltControls;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        tiltControls = (PlayerPrefs.GetInt("Tilt Controls", 1) == 1);
        L.SetActive(!tiltControls);
        R.SetActive(!tiltControls);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!tiltControls)
        {
            if (Input.GetKey(KeyCode.A) || leftPressed)
            {
                rb.AddForce(new Vector3(-force, 0f));
            }

            if (Input.GetKey(KeyCode.D) || rightPressed)
            {
                rb.AddForce(new Vector3(force, 0f));
            }
        }
        else
        {
            float accel = Input.acceleration.x;
            int sign = (int)Mathf.Sign(accel);
            if (Mathf.Abs(accel) > 0.5f)
            {
                rb.AddForce(new Vector3(sign * force, 0f));
            }
            else if (Mathf.Abs(accel) > 0.05f)
            {
                rb.AddForce(new Vector3(sign * Mathf.Lerp(force / 2f, force, accel * 2f), 0f));
            }
        }
    }
    public void onPress(bool left)
    {
        if (left)
            leftPressed = true;
        else
            rightPressed = true;
    }

    public void onRelease(bool left)
    {
        if (left)
            leftPressed = false;
        else
            rightPressed = false;
    }
}