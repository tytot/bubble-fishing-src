using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IgnorePause : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        AudioSource theme = GetComponent<AudioSource>();
        theme.ignoreListenerPause = true;
	}

    public void goHome()
    {
        SceneManager.LoadScene("Title");
    }
}
