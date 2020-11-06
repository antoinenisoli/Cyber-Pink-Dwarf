using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyClose : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerScript;
    private Animator anim;
    public float moveSpeed;

    public int maxHealth = 3;
    public int Health;

    private SpriteRenderer spr;
    public Material startMat;
    public Material lightMat;

    public Collider2D ThisRoom;
    private Vector2 startPos;

    public Collider2D ThisCollider01;
    public Collider2D ThisCollider02;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();

        spr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        startMat = spr.material;

        startPos = transform.position;
        Health = maxHealth * 2;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Room"))
        {
            ThisRoom = col;
        }
    }

    void Update()
    {
        FollowPlayer();
        Life();
    }

    void FollowPlayer()
    {
        if (Health > 0)
        {
            if (ThisRoom == playerScript.room && playerScript.health > 0)
            {
                float step = moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
            }
            else
            {
                float step = moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, startPos, step);
            }
        }
    }

    public void Damage(int dmg)
    {
        StartCoroutine(TakeDamage(dmg));
    }

    public IEnumerator TakeDamage(int damage)
    {
        if (Health > 0)
        {
            spr.material = lightMat;
            yield return new WaitForSeconds(0.1f);
            spr.material = startMat;
            Health -= damage;
        }
    }

    void Life()
    {
        if (Health <= 0)
        {
            ThisCollider01.enabled = false;
            ThisCollider02.enabled = false;
            anim.SetBool("dead", true);
        }
    }
}
