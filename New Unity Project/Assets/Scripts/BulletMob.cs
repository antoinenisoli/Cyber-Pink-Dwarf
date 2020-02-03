using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMob : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;

    public void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Destruction()
    {
        Destroy(gameObject);
    }

    public void Collision()
    {
        rb.velocity = new Vector2(0, 0);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 9 || col.gameObject.layer == 12 || col.gameObject.layer == 11) //tir dans un mur
        {
            anim.SetBool("dead", true);
        }

        if (col.gameObject.layer == 14) //tir dans une machine
        {
            if (col.GetComponent<Machine>())
            {
                col.GetComponent<Machine>().isWorking = false;
            }

            anim.SetBool("dead", true);
        }

        if (col.gameObject.layer == 8) //tir sur le joueur
        {
            if (col.GetComponent<PlayerController>() && !col.GetComponent<PlayerController>().detect)
            {
                col.GetComponent<PlayerController>().Damage(1);
            }

            anim.SetBool("dead", true);
        }
    }
}
