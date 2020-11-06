using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMob : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    public LayerMask playerLayer;
    public float radius;
    public bool hitOnce;
    public bool hitPlayer;

    public void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        hitOnce = false;
    }

    private void Update()
    {
        HitPlayer();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void HitPlayer()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        hitPlayer = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
        
        if (hitPlayer && !player.detect && !hitOnce)
        {
            hitOnce = true;
            player.GetComponent<PlayerController>().Damage(1);
            anim.SetBool("dead", true);
        }        
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
                col.GetComponent<Machine>().PlaySound();
            }

            anim.SetBool("dead", true);
        }
    }
}
