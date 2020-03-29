using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorekeeper : MonoBehaviour {

    public int highScore;

	// Use this for initialization
	void Start () {
        highScore = PlayerPrefs.GetInt("High Score", 0);
	}
	
	// Update is called once per frame
	void Update () {
		if(Manager.instance.depth > highScore)
        {
            highScore = (int)Manager.instance.depth;
        }
	}

    private void OnDisable()
    {
        PlayerPrefs.SetInt("High Score", highScore);
        PlayerPrefs.Save();
    }
}
