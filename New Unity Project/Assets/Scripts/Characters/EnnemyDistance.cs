using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyDistance : MonoBehaviour
{
    public float bulletSpeed;
    public float moveSpeed;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    private Vector2 startpos;

    public float fireRate = 1;
    private float nextFire;

    public SpriteRenderer spr;
    public Material startMat;
    public Material lightMat;

    public Animator anim;

    public int health;
    public int healthMax = 6;

    public GameObject player;
    public PlayerController playerScript;
    public Collider2D ThisRoom;

    public Collider2D ThisCollider01;
    public Collider2D ThisCollider02;

    public Collider2D[] players;
    public float radius;
    public LayerMask playerLayer;

    [Header("Sounds")]
    public AudioSource shotSound;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        spr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        nextFire = Time.time;
        startpos = transform.position;

        startMat = spr.material;

        health = healthMax;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Room"))
        {
            ThisRoom = col;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void Update()
    {
        AimThePlayer();
        FollowPlayer();
        Life();
    }

    private void FollowPlayer()
    {
        if (health > 0)
        {
            if (ThisRoom == playerScript.room && playerScript.health > 0)
            {
                float step = moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
            }
            else
            {
                float step = moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, startpos, step);
            }
        }
    }

    public void Damage(int dmg)
    {
        StartCoroutine(TakeDamage(dmg));
    }

    public IEnumerator TakeDamage(int damage)
    {
        spr.material = lightMat;        
        yield return new WaitForSeconds(0.1f);
        spr.material = startMat;
        health -= damage;
    }

    void Life()
    {
        if (health <= 0)
        {
            ThisCollider01.enabled = false;
            ThisCollider02.enabled = false;
            anim.SetBool("dead", true);
        }
    }

    private void AimThePlayer()
    {
        players = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);

        if (players.Length != 0)
        {
            if (ThisRoom == playerScript.room && playerScript.health > 0)
            {
                if (Time.time > nextFire)
                {
                    Shoot();
                    nextFire = Time.time + fireRate;
                }
            }
        }        
    }

    void Shoot()
    {
        if (health >= 0)
        {
            GameObject b = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity) as GameObject;
            shotSound.Play();

            Rigidbody2D bulletPhysic = b.GetComponent<Rigidbody2D>();
            Vector2 distance = player.transform.position - transform.position;
            Vector2 direction = distance.normalized * bulletSpeed;
            bulletPhysic.velocity = (direction);
        }
    }
}
