using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour {

    // Use this for initialization
    public void Restart(string sceneName)
    {
        GameObject audio = GameObject.Find("SoundPlayer");
        Destroy(audio);
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }
}
