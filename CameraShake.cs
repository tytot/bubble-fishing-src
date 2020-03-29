using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;

    // How long the object should shake for.
    float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 1f;
    public float decreaseFactor = 1.0f;
    
    private float opacity;
    public bool flash;
    public SpriteRenderer bang;

    float startZ;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void Update()
    {
        if (opacity != 0f) {
            bang.color = new Color(1f, 1f, 1f, opacity);
            opacity -= Time.deltaTime;
        }
    }

    void LateUpdate()
    {
        Vector3 initPos = new Vector3(camTransform.position.x, camTransform.position.y, startZ);
        if (shakeDuration > 0)
        {
            camTransform.localPosition = initPos + (Random.insideUnitSphere * shakeAmount);

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else if (shakeDuration < 0)
        {
            shakeDuration = 0f;
            camTransform.localPosition = initPos;
        }
    }

    public void StartShake(float duration)
    {
        shakeDuration = duration;

        if (flash)
            opacity = 1f;
        else
            opacity = 0f;

        startZ = camTransform.position.z;
    }
}
