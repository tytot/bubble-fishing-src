using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour {

    private Light lt;
    public SpriteRenderer overlay;

	// Use this for initialization
	void Start () {
        lt = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        lt.intensity = -0.001f * Manager.instance.depth + 1f;

        float opacity = Mathf.Lerp(0f, 0.7f, Manager.instance.depth / 5000f);
        overlay.color = new Color(0f, 0f, 0f, opacity);
	}
}
