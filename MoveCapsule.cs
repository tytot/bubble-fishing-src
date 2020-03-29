using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCapsule : MonoBehaviour {
    
    public GameObject bubble;
    
    void FixedUpdate () {
        transform.position = bubble.transform.position;
        transform.localScale = bubble.transform.localScale;
	}
}
