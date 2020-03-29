using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float VelocityConstant;
    public GameObject Projectile;
    private GameObject clone;
    Animator anim;
    private float startTime = 0f;
    private float timer = 0f;
    public float TimeUntilFullCharge;
    private bool chargefinished;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            startTime = Time.time;
            timer = startTime;
            anim.SetTrigger("startcharge");
            chargefinished = false;
        }

        if (Input.GetButton("Fire1"))
        {
            timer += Time.deltaTime;

            if (chargefinished == false)
            {
                if ((timer - startTime) >= TimeUntilFullCharge)
                {
                    anim.SetTrigger("finishcharge");
                    chargefinished = true;
                }
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            anim.SetTrigger("aftercharge");
            // Spawn the bullet
            clone = Instantiate(Projectile, transform.position, transform.rotation);
            OnShot();
        }

        if (clone)
        {
            float speed = (Manager.instance.weight - 50) / 5;
            var pos = clone.transform.position;
            pos += new Vector3(0, Time.deltaTime * speed);
            clone.transform.position = pos;
        }
    }

    private void OnShot()
    {
        // Ignore collision
        Physics2D.IgnoreCollision(clone.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        // Get the Rigid.2D reference
        var rigid = clone.GetComponent<Rigidbody2D>();

        float x = Mathf.Cos(Mathf.PI / 180 * (transform.eulerAngles.z + 45));
        float y = Mathf.Sin(Mathf.PI / 180 * (transform.eulerAngles.z + 45));

        var direction = new Vector2(x, y);

        // Add force to the rigidbody
        rigid.AddForce(direction * VelocityConstant * (timer - startTime));
    }
}