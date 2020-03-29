using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievements : MonoBehaviour {

    private int obtained = 0;
    private int total;

    void Start()
    {
        Transform content = transform.GetChild(1).transform.GetChild(0);
        total = content.childCount;
        for (int i = 0; i < total - 1; i++)
        {
            Transform child = content.GetChild(i);
            if (PlayerPrefs.GetInt(child.name, 0) == 0)
            {
                child.GetComponent<Image>().color = Color.gray;
                child.GetComponentInChildren<Text>().color = Color.gray;
            }
            else
                obtained++;
        }

        Text signText = transform.GetChild(2).GetComponentInChildren<Text>();
        signText.text = "Achievements\n<size=50>" + obtained + "/" + total + " Complete</size>";
    }

    public void Exit()
    {
        gameObject.SetActive(false);
    }
}
