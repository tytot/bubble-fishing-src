using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorial;
    Animator anim;

    public GameObject spawner;
    Spawner s;
    BubbleSpawner bs;
    PowerUpSpawner pus;
    EnemySpawner es;

    public Toggle pause;

    public int obtained = 0;

    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Tutorial") == 0)
        {
            anim = tutorial.GetComponent<Animator>();

            s = spawner.GetComponent<Spawner>();
            bs = spawner.GetComponent<BubbleSpawner>();
            pus = spawner.GetComponent<PowerUpSpawner>();
            es = spawner.GetComponent<EnemySpawner>();

            s.enabled = false;
            bs.enabled = false;
            pus.enabled = false;
            es.enabled = false;

            pause.interactable = false;

            StartCoroutine(Guide());
        }
    }

    public void Next(bool screen)
    {
        tutorial.SetActive(screen);
        Manager.instance.ledge = screen;
        if (screen)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
    }

    public void Timescale(float val)
    {
        Time.timeScale = val;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main");
    }

    IEnumerator Guide()
    {
        Next(true);
        anim.Play("Tutorial3");
        yield return new WaitForSeconds(5f);
        Next(false);
        yield return new WaitForSeconds(5f);
        Next(true);
        anim.Play("Tutorial4");
        yield return new WaitForSeconds(4f);
        anim.Play("Tutorial45");
        yield return new WaitForSeconds(6f);
        Next(false);
        s.enabled = true;
        while (obtained < 2)
            yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        obtained = 0;
        Next(true);
        anim.Play("Tutorial5");
        yield return new WaitForSeconds(5f);
        Next(false);
        bs.enabled = true;
        while (obtained < 1)
            yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        Next(true);
        anim.Play("Tutorial6");
        yield return new WaitForSeconds(3f);
        anim.Play("Tutorial65");
        yield return new WaitForSeconds(5f);
        Next(false);
        PlayerPrefs.SetInt("Tutorial", 1);
        PlayerPrefs.Save();
        pus.enabled = true;
        es.enabled = true;
        pause.interactable = true;

        yield break;
    }
}
