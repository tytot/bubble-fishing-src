using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public GameObject bubble;

    public float LerpTime;
    public float maxnibba;
    private float moveThreshold;
    private bool isLerping;
    private float BeganLerp;
    private float width;
    private float frustumWidth;

    private Vector3 startPos;
    private Vector3 endPos;
    private float center;

    float min;
    float lastP = 50;

    public CameraShake cs;

    void StartLerping()
    {
        BeganLerp = Time.time;

        startPos = transform.position;

        var max = -maxnibba / width;
        float zDes = Mathf.Lerp(min, max, (bubble.transform.localScale.x - 1f) / 7f);
        float yDes;
        if (Manager.instance.percentage > 50f)
            yDes = zDes / 5f;
        else if (Manager.instance.percentage < 50f)
            yDes = zDes / -10f;
        else
            yDes = 0f;

        endPos = new Vector3(transform.position.x, yDes, zDes);
    }

    void Start()
    {
        isLerping = false;

        center = 0f;

        width = Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad) * Camera.main.aspect;

        min = -maxnibba / width / (3.75f - 0.75f * PlayerPrefs.GetInt("Scope", 2));
        transform.position = new Vector3(0f, 0f, min);

        cs.shakeAmount = -12f / transform.position.z;
    }

    void Update()
    {
        if (bubble)
        {
            if (Manager.instance.percentage != lastP && !isLerping) {
                lastP = Manager.instance.percentage;
                StartLerping();
                isLerping = true;
            }
        }

        frustumWidth = Mathf.Abs(transform.position.z) * width;
        moveThreshold = (1f / 3f) * frustumWidth;
    }

    void LateUpdate ()
    {
        if (isLerping)
        {
            float timeSinceStart = Time.time - BeganLerp;
            float percentageComplete = timeSinceStart / LerpTime;
            transform.position = Vector3.Lerp(startPos, endPos, percentageComplete);
            //Debug.Log(startPos + " to " + endPos);
            cs.shakeAmount = -12f / transform.position.z;

            if (percentageComplete >= 1f)
                isLerping = false;
        }

        if (bubble)
        {
            float bx = bubble.transform.position.x;

            if (bx > moveThreshold)
                center = bx - moveThreshold;
            else if (bx < -moveThreshold)
                center = bx + moveThreshold;
            else
                center = 0f;

            transform.position = new Vector3(center, transform.position.y, transform.position.z);
        }
        if (transform.position.x + frustumWidth > maxnibba)
            transform.position = new Vector3(maxnibba - frustumWidth, transform.position.y, transform.position.z);
        else if (transform.position.x - frustumWidth < -maxnibba)
            transform.position = new Vector3(-maxnibba + frustumWidth, transform.position.y, transform.position.z);
    }
}
