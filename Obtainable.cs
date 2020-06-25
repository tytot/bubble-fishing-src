using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obtainable : MonoBehaviour {
    
    private int lastc = 4;
    private int lastw = 0;
    public GameObject bubble;

    public Dictionary<GameObject, bool> myDic = new Dictionary<GameObject, bool>();
    
    void Update()
    {
        if (Manager.instance.capacity != lastc || Manager.instance.weight != lastw)
        {
            int children = transform.childCount;
            for (int i = 0; i < children; i++)
            {
                GameObject fish = transform.GetChild(i).gameObject;
                check(fish);
            }
        }
        lastc = Manager.instance.capacity;
        lastw = Manager.instance.weight;
    }

    public void check(GameObject fish)
    {
        if (fish.CompareTag("Fish"))
        {
            bool obtainable = IsObtainable(fish.transform);
            if (obtainable)
            {
                if (!myDic[fish])
                {
                    fish.GetComponent<SpriteRenderer>().sprite = fish.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite;
                    myDic[fish] = true;
                    Debug.Log("set fish " + fish.name + " to be obtainable");
                }
            }
            else
            {
                if (myDic[fish])
                {
                    fish.GetComponent<SpriteRenderer>().sprite = fish.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                    myDic[fish] = false;
                    Debug.Log("set fish " + fish.name + " to be unobtainable");
                }
            }
        }
    }

    bool IsObtainable(Transform fish)
    {
        if (160f * bubble.transform.localScale.x <= fish.GetComponent<SpriteRenderer>().sprite.rect.xMax)
        {
            return false;
        }
        int potAdd;
        switch (fish.name)
        {
            case "Minnow(Clone)":
                potAdd = 2;
                break;
            case "Trout(Clone)":
                potAdd = 4;
                break;
            case "Pike(Clone)":
                potAdd = 6;
                break;
            case "Bass(Clone)":
                potAdd = 8;
                break;
            case "Tuna(Clone)":
                potAdd = 10;
                break;
            case "Kingfish(Clone)":
                potAdd = 12;
                break;
            case "Grouper(Clone)":
                potAdd = 14;
                break;
            case "Shark(Clone)":
                potAdd = 16;
                break;
            case "Whale(Clone)":
                potAdd = 20;
                break;
            case "Golden Fish(Clone)":
                potAdd = 2;
                break;
            default:
                potAdd = 0;
                break;
        }
        if (Manager.instance.weight + potAdd > Manager.instance.capacity)
            return false;

        return true;
    }
}
