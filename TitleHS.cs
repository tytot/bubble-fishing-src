using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleHS : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Text text = GetComponent<Text>();
        int HS = PlayerPrefs.GetInt("High Score", 0);
        text.text = "HS: " + HS;
	}
}
