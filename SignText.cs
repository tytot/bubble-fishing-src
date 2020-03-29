using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignText : MonoBehaviour {

    private Text signText;

	// Use this for initialization
	void Start () {
        signText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Manager.instance.depth > 0f)
            signText.text = Mathf.Round(Manager.instance.depth) + " m";
        else
            signText.text = "0 m";
	}
}
