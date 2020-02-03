using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    public float magnitude;
    public float moveSpeed;
    private Rigidbody2D rb;

    public int health;
    public int maxHealth = 10;

    public GameObject aliveSpr;
    public GameObject deadSpr;

    public LayerMask ennemies;
    public Transform collisionCenter;
    public float collisionRadius;
    public bool detect;
    public float damageRate = 1;

    public bool OnMachine;
    private Collider2D machineCollider;
    public float repairMachine;
    public int isFixed = 2;

    public SpriteRenderer spr;
    public Material startMat;
    public Material lightMat;

    public Animator headAnimator;
    public Animator bodyAnimator;

    private GMscript gm;

    private float mouseX;
    private float mouseY;
    private GameObject mainCam;
    public Transform mousePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public bool isShooting;
    private Vector2 shootDirection;
    public float fireRate = 0.5f;
    private float nextFire;

    public Collider2D room;
    public Transform shooter;

    [Header("Sounds")]
    public AudioSource shotSound;

    public bool blocked;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gm = FindObjectOfType<GMscript>();

        health = maxHealth;
        startMat = spr.material;

        nextFire = Time.time;
        deadSpr.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(collisionCenter.position, collisionRadius);
    }
   
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Room"))
        {
            room = col;
        }

        if (col.CompareTag("MachineZone"))
        {
            OnMachine = true;
            machineCollider = col;
        }
        else
        {
            OnMachine = false;
            machineCollider = null;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Heal"))
        {
            if (health != maxHealth)
            {
                health += 1;
                Destroy(other.gameObject);
            }
        }
    }

    void Update()
    {
        GetCamera();
        GetInput();
        Move();
        Aim();
        Life();
        DetectMob();
        InteractWithMachine();
        BodyAnimation();
        PlaySounds();
    }

    void PlaySounds()
    {

    }

    void BodyAnimation()
    {
        float speed = rb.velocity.sqrMagnitude;
        bodyAnimator.SetFloat("speed", speed);

        if (horizontalInput > 0)
        {
            bodyAnimator.SetBool("right", true);
        }
        else
        {
            bodyAnimator.SetBool("right", false);
        }

        if (horizontalInput < 0)
        {
            bodyAnimator.SetBool("left", true);
        }
        else
        {
            bodyAnimator.SetBool("left", false);
        }

        if (verticalInput > 0)
        {
            bodyAnimator.SetBool("up", true);
        }
        else
        {
            bodyAnimator.SetBool("up", false);
        }

        if (verticalInput < 0)
        {
            bodyAnimator.SetBool("down", true);
        }
        else
        {
            bodyAnimator.SetBool("down", false);
        }

    }

    public IEnumerator Reload(int time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(2);
    }

    void InteractWithMachine()
    {
        if (machineCollider != null)
        {
            Machine script = machineCollider.gameObject.GetComponentInParent<Machine>();

            if (OnMachine && !script.isWorking)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    script.repairing = true;
                    blocked = true;
                }
                else
                {
                    script.repairing = false;
                    blocked = false;
                }
            }
            else
            {
                script.repairing = false;
                blocked = false;
            }
        }
        else
        {
            blocked = false;
        }
    }

    void DetectMob()
    {
        detect = Physics2D.OverlapCircle(collisionCenter.position, collisionRadius, ennemies);

        if (detect)
        {            
            if (Time.time > nextFire)
            {
                Damage(1);
                nextFire = Time.time + damageRate;
            }
        }
    }

    public void Damage(int dmg)
    {
        StartCoroutine(TakeDmg(dmg));
    }

    public IEnumerator TakeDmg(int dmg)
    {
        spr.material = lightMat;       
        yield return new WaitForSeconds(0.1f);
        spr.material = startMat;
        health -= dmg;
    }

    void Life()
    {
        if (health <= 0) //mort du perso
        {
            bool death = false;

            aliveSpr.SetActive(false);
            deadSpr.SetActive(true);
            
            if (!death)
            {
                StartCoroutine(GameOver(3));
                
                blocked = true;
                death = true;
            }
        }

        if (gm.wagonHealth <= 0)
        {
            blocked = true;
        }
    }

    IEnumerator GameOver(int nbr)
    {
        yield return new WaitForSeconds(nbr);
        gm.GameOver();
    }

    void GetCamera()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void GetInput()
    {
        if (!blocked && health > 0)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            mouseX = Input.mousePosition.x;
            mouseY = Input.mousePosition.y;
        }
        else
        {
            horizontalInput = 0;
            verticalInput = 0;
        }
    }

    void Move()
    {
        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        movement = movement.normalized;
        magnitude = movement.magnitude;
        rb.velocity = movement * moveSpeed;
    }

    void Aim()
    {
        if (!blocked && health > 0)
        {
            Vector3 mousePos = new Vector3(mouseX, mouseY, transform.position.z);
            Vector3 target = mainCam.GetComponent<Camera>().ScreenToWorldPoint(mousePos);
            mousePoint.position = new Vector2(target.x, target.y);

            Vector3 difference = target - shooter.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            shooter.rotation = Quaternion.Euler(0, 0, rotationZ);
            headAnimator.SetFloat("HeadRotation", rotationZ);

            float distance = difference.magnitude;
            Vector2 direction = difference / distance;
            direction.Normalize();
            shootDirection = direction;

            if (Input.GetMouseButton(0))
            {
                isShooting = true;

                if (Time.time > nextFire)
                {
                    StartCoroutine(ShootBullet(direction, rotationZ));
                    nextFire = Time.time + fireRate;
                }
            }
            else
            {
                isShooting = false;

            }
        }
    }

    IEnumerator ShootBullet(Vector2 dir, float rotation)
    {
        yield return new WaitForSeconds(0);

        if (isShooting)
        {
            dir = shootDirection;
            GameObject bullet = Instantiate(bulletPrefab) as GameObject;

            shotSound.Play();

            if (!shotSound.isPlaying)
            {
                
            }

            Rigidbody2D bulletPhysics = bullet.GetComponent<Rigidbody2D>();
            bullet.transform.position = shooter.position;
            bullet.transform.rotation = Quaternion.Euler(0, 0, rotation-90);
            bulletPhysics.velocity = dir * bulletSpeed;
            
        }
    }
}
