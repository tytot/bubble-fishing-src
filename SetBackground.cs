using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBackground : MonoBehaviour {
    
    public Sprite[] backgrounds;
    SpriteRenderer sky;

    // Use this for initialization
    void Start () {
        sky = GetComponent<SpriteRenderer>();
        ChangeBackground();
    }

    public void ChangeBackground()
    {
        sky.sprite = backgrounds[PlayerPrefs.GetInt("Background", 0)];
    }
}
