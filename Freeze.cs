using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour {

    public bool freeze;

    public float freezeSpeed;
    private SpriteRenderer overlay;

	// Use this for initialization
	void Start () {
        overlay = GetComponent<SpriteRenderer>();
        overlay.color = new Color(0.6f, 1f, 1f, 0f);
    }

    void Update()
    {
        float opacity;
        if (freeze)
            opacity = 0.5f;
        else
            opacity = 0f;
        overlay.color = Vector4.Lerp(overlay.color, new Color(0.6f, 1f, 1f, opacity), freezeSpeed * Time.deltaTime);
    }
}
