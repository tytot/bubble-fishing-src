using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillBar : MonoBehaviour {

    Image barLeft;
    Image barRight;
    public float barSpeed;

	// Use this for initialization
	void Start () {
        Image[] bars = GetComponentsInChildren<Image>();
        barLeft = bars[1];
        barRight = bars[2];
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (Manager.instance.percentage < 48f)
        {
            barRight.fillAmount = Mathf.Lerp(barRight.fillAmount, (50f - Manager.instance.percentage) / 50f, barSpeed * Time.deltaTime);
            barLeft.fillAmount = Mathf.Lerp(barLeft.fillAmount, 0.02f, barSpeed * Time.deltaTime);
        }
        else if (Manager.instance.percentage > 52f)
        {
            barLeft.fillAmount = Mathf.Lerp(barLeft.fillAmount, (Manager.instance.percentage - 50f) / 50f, barSpeed * Time.deltaTime);
            barRight.fillAmount = Mathf.Lerp(barRight.fillAmount, 0.02f, barSpeed * Time.deltaTime);
        }
        else
        {
            barRight.fillAmount = Mathf.Lerp(barRight.fillAmount, 0.02f, barSpeed * Time.deltaTime);
            barLeft.fillAmount = Mathf.Lerp(barLeft.fillAmount, 0.02f, barSpeed * Time.deltaTime);
        }
	}
}
