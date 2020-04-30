using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GMscript : MonoBehaviour
{
    public int randomNumber;
    public bool TakingDamage;
    public float timer = 0;
    public float StartSabotage = 5;
    private GameObject player;
    private PlayerController playerScript;
    private Animator anim;

    public GameObject Warning;
    public Animator wagonAnim;
    public GameObject deathScreen;
    public float wagonHealth;
    public float wagonHealthMax = 100;
    public float damageAmount = 0.01f;

    public Collider2D selectedRoom;
    public Collider2D thisRoom;

    public Transform marker;
    public Collider2D room00;
    public Collider2D room01;
    public Collider2D room02;
    public Collider2D room03;
    public Collider2D room04;
    public Collider2D generatorRoom;
    public Collider2D engineRoom;
    public Collider2D cockpitRoom;

    public Transform pos00;
    public Transform pos01;
    public Transform pos02;
    public Transform pos03;
    public Transform pos04;
    public Transform posEngine;
    public Transform posCockpit;
    public Transform posGenerator;

    public AudioSource alarmSound;
    private bool alarm;
    
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        anim = GetComponentInParent<Animator>();

        wagonHealth = wagonHealthMax;
        deathScreen.SetActive(false);

        StartCoroutine(GenerateRandom(4));
    }

    void Update()
    {
        Minimap();
        wagonHealth = Mathf.Clamp(wagonHealth, 0, wagonHealthMax);
        //Reload();        
        TakeDmg();
        CheckHealth();                      
    }

    private void FixedUpdate()
    {
        Minimap();
    }

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(2);
        }
    }

    private void LateUpdate()
    {
        Minimap();

        if (TakingDamage)
        {
            Warning.SetActive(true);
            wagonAnim.speed = 1;
        }
        else
        {
            Warning.SetActive(false);
            wagonAnim.speed = 3;
        }
    }

    public void CheckHealth()
    {
        if (wagonHealth <= 0)
        {
            GameOver();
        }
        else
        {
            transform.position = player.transform.position;
        }
    }

    public void GameOver()
    {
        wagonHealth = 0;
        TakingDamage = false;
        deathScreen.SetActive(true);
    }

    public void CallRandomGenerator(int time)
    {
        StartCoroutine(GenerateRandom(time));
    }

    public void Minimap()
    {
        if (playerScript.room == room00)
        {
            marker.position = pos00.position;
        }
        else if (playerScript.room == room01)
        {
            marker.position = pos01.position;
        }
        else if (playerScript.room == room02)
        {
            marker.position = pos02.position;
        }
        else if (playerScript.room == room03)
        {
            marker.position = pos03.position;
        }
        else if (playerScript.room == room04)
        {
            marker.position = pos04.position;
        }
        else if (playerScript.room == cockpitRoom)
        {
            marker.position = posCockpit.position;
        }
        else if (playerScript.room == engineRoom)
        {
            marker.position = posEngine.position;
        }
        else if (playerScript.room == generatorRoom)
        {
            marker.position = posGenerator.position;
        }
    }

    public IEnumerator GenerateRandom(int second)
    {
        yield return new WaitForSeconds(second);

        timer = 0;
        int random = Random.Range(1, 4); //génére un nombre entre 1 et 3

        if (random == 1 && playerScript.room != generatorRoom) //salle du générateur
        {
            anim.SetBool("generator", true);
            anim.SetBool("cockpit", false);
            anim.SetBool("engine", false);
            randomNumber = random;
            selectedRoom = generatorRoom;
        }
        if (random == 1 && playerScript.room == generatorRoom)
        {
            StartCoroutine(GenerateRandom(0));
        }

        if (random == 2 && playerScript.room != cockpitRoom) //salle du cockpit
        {
            anim.SetBool("cockpit", true);
            anim.SetBool("engine", false);
            anim.SetBool("generator", false);
            randomNumber = random;
            selectedRoom = cockpitRoom;
        }
        if (random == 2 && playerScript.room == cockpitRoom)
        {
            StartCoroutine(GenerateRandom(0));
        }

        if (random == 3 && playerScript.room != engineRoom) //salle des machines
        {
            anim.SetBool("engine", true);
            anim.SetBool("cockpit", false);
            anim.SetBool("generator", false);
            randomNumber = random;
            selectedRoom = engineRoom;
        }
        if (random == 3 && playerScript.room == engineRoom)
        {
            StartCoroutine(GenerateRandom(0));
        }        
    }

    void TakeDmg()
    {
        if (wagonHealth > 0)
        {
            if (TakingDamage)
            {
                Warning.SetActive(true);
                wagonHealth -= damageAmount;

                if (!alarmSound.isPlaying && alarm)
                {
                    alarmSound.Play();
                    alarm = false;
                }
            }
            else
            {
                Warning.SetActive(false);
                timer += Time.deltaTime;
                alarm = true;
            }

            if (timer >= StartSabotage)
            {
                TakingDamage = true;
            }
            else
            {
                TakingDamage = false;
            }
        }
        else
        {
            timer = 0;
        }
    }
}
