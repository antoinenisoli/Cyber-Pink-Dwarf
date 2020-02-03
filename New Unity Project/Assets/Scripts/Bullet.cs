using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 14) //tir dans une machine
        {
            print("mdr");
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 9 || col.gameObject.layer == 12) //tir dans un mur
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

        if (col.gameObject.layer == 10) //tir sur mob
        {
            if (col.GetComponent<EnnemyClose>())
            {
                col.GetComponent<EnnemyClose>().Damage(1);
            }
            else if (col.GetComponent<EnnemyDistance>())
            {
                col.GetComponent<EnnemyDistance>().Damage(1);
            }

            anim.SetBool("dead", true);
        }
    }
}
