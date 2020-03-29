using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour {

    Text storeText;
    Transform content;

    Transform confirmation;
    Button yes;
    Button no;

    public GameObject procs;

    int boins;
    bool decrease = false;

    public Animator poorNIBBA;
    Animator that;

    AudioSource lol;
    public AudioClip loss;
    public AudioClip notEnough;

    SetBackground sb;

    public AudioSource pranav;
    public AudioClip click;

    public Text titleBoins;
    public Animator tut;
    int val;

    IEnumerator TrustTheProcess(Transform parsex)
    {
        procs.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        procs.SetActive(false);
        Process(parsex);
    }

	// Use this for initialization
	public void Initialize () {
        storeText = transform.GetChild(2).GetChild(0).GetComponent<Text>();
        boins = PlayerPrefs.GetInt("Boins", 0);
        storeText.text = "Store\n<size=50>   x" + boins.ToString("D4") + "</size>";

        content = transform.GetChild(1).GetChild(0);

        foreach (Transform child in content)
        {
            StoreItem item = child.GetComponent<StoreItem>();
            if (item == null) continue;
            
            if (PlayerPrefs.GetInt(child.name) == 2)
            {
                item.buttons.SetActive(false);
                item.enableText.SetActive(true);
            }
            else if (PlayerPrefs.GetInt(child.name) == 3)
            {
                item.buttons.SetActive(false);
                item.enableText.SetActive(true);
                Text enub = item.enableText.GetComponent<Text>();
                enub.text = "ENABLED";
                enub.color = new Color(0.2f, 0.12f, 0.08f);
                item.toggle.isOn = true;
            }

            if (item.prefsName == "")
            {
                item.toggle.onValueChanged.AddListener(delegate
                {
                    ToggleValueChanged(item.toggle, item.transform);
                });
            }
            else
            {
                item.toggle.onValueChanged.AddListener(delegate
                {
                    ToggleValueChanged(item.toggle, item.transform, item.prefsName, item.index);
                });
            }
        }
    }

    private void Start()
    {
        val = PlayerPrefs.GetInt("Tutorial");
        if (val == 2)
        {
            if (!PlayerPrefs.HasKey("Wood Ramp"))
                PlayerPrefs.SetInt("Wood Ramp", 3);
            if (!PlayerPrefs.HasKey("Launcher"))
                PlayerPrefs.SetInt("Launcher", 1);
            PlayerPrefs.Save();
        }

        confirmation = transform.GetChild(3);
        yes = confirmation.GetChild(0).GetComponent<Button>();
        no = confirmation.GetChild(1).GetComponent<Button>();

        lol = GetComponent<AudioSource>();

        sb = FindObjectOfType<SetBackground>();
        that = GetComponent<Animator>();

        if (val == 1)
        {
            tut.Play("Tutorial8");
        }
    }

    /* 1 -> buttons
     * 2 -> not enabled
     * 3 -> enabled */

    void ToggleValueChanged(Toggle tgl, Transform parent, string prefsName, int index)
    {
        int iot = PlayerPrefs.GetInt(prefsName);
        ToggleValueChanged(tgl, parent);
        if (tgl.isOn)
        {
            PlayerPrefs.SetInt(prefsName, index);
            if (prefsName == "Background")
            {
                sb.ChangeBackground();
            }
        }
        else
        {
            if (index == iot)
            {
                if (prefsName == "Light Type")
                    PlayerPrefs.SetInt(prefsName, 0);
                if (prefsName == "Scope")
                    PlayerPrefs.SetInt(prefsName, 2);
                if (prefsName == "Background")
                {
                    PlayerPrefs.SetInt(prefsName, 0);
                    sb.ChangeBackground();
                }
            }
        }
        if (index != iot)
        {
            pranav.PlayOneShot(click);
        }

        if (PlayerPrefs.GetInt("Tutorial") == 1)
        {
            tut.Play("Tutorial10");
        }

        PlayerPrefs.Save();
    }
    void ToggleValueChanged(Toggle tgl, Transform parent)
    {
        Text enub = parent.GetChild(2).GetChild(0).gameObject.GetComponent<Text>();
        if (tgl.isOn)
        {
            PlayerPrefs.SetInt(parent.gameObject.name, 3);
            enub.text = "ENABLED";
            enub.color = new Color(0.2f, 0.12f, 0.08f);
        }
        else
        {
            PlayerPrefs.SetInt(parent.gameObject.name, 2);
            enub.text = "ENABLE";
            enub.color = Color.white;
        }
        PlayerPrefs.Save();
    }

    public void Buy (Transform button)
    {
        Text priceText = button.GetComponentInChildren<Text>();
        int price = int.Parse(priceText.text);

        if (boins >= price)
        {
            Transform paront = button.parent.parent;

            confirmation.gameObject.SetActive(true);
            Text info = confirmation.GetChild(2).GetComponent<Text>();
            info.text = paront.name + " - " + price + " boins";

            if (val == 1)
                tut.Play("Tutorial9");

            yes.onClick.AddListener(() => Purchase(paront, price));
            no.onClick.AddListener(() => Cancel(paront, price));
        }
        else
        {
            poorNIBBA.Play("NotEnough");
            lol.PlayOneShot(notEnough);
        }
    }

    private void Purchase(Transform paront, float price)
    {
        Process(paront);
        decrease = true;
        PlayerPrefs.SetInt("Boins", boins - (int)price);

        PlayerPrefs.Save();

        if (val == 1)
            tut.Play("Tutorial8", -1, 0.75f);
    }

    public void Process(Transform parenig)
    {
        parenig.GetChild(2).GetComponent<Toggle>().interactable = true;
        parenig.GetChild(2).GetChild(0).gameObject.SetActive(true);
        parenig.GetChild(1).gameObject.SetActive(false);
        PlayerPrefs.SetInt(parenig.gameObject.name, 2);
        yes.onClick.RemoveAllListeners();
        no.onClick.RemoveAllListeners();

        PlayerPrefs.Save();
    }

    private void Cancel(Transform paront, float price)
    {
        yes.onClick.RemoveListener(() => Purchase(paront, price));
        no.onClick.RemoveListener(() => Cancel(paront, price));
    }

    public void PlayAnim(Transform paresex)
    {
        StartCoroutine(TrustTheProcess(paresex));
    }

    void Update()
    {
        if (decrease)
        {
            boins -= 8;
            lol.PlayOneShot(loss, 0.2f);

            if (boins <= PlayerPrefs.GetInt("Boins", boins))
            {
                boins = PlayerPrefs.GetInt("Boins");
                decrease = false;
            }

            storeText.text = "Store\n<size=50>   x" + boins.ToString("D4") + "</size>";
        }
    }

    public void Exit()
    {
        foreach (Transform child in content)
        {
            StoreItem item = child.GetComponent<StoreItem>();
            if (item == null) continue;
            item.toggle.onValueChanged.RemoveAllListeners();
        }

        gameObject.SetActive(false);
        titleBoins.text = PlayerPrefs.GetInt("Boins").ToString("D4");

        if (val == 1)
        {
            tut.gameObject.SetActive(false);
            PlayerPrefs.SetInt("Tutorial", 2);

            PlayerPrefs.SetInt("Cannon", 1);
        }
    }

    private void OnApplicationQuit()
    {
        Exit();
    }
}
