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
    private UIwagon wagonscript;
    private Animator anim;

    public GameObject Warning;
    public GameObject deathScreen;
    public float wagonHealth;
    public float wagonHealthMax = 100;
    public float damageAmount = 0.01f;

    public Collider2D selectedRoom;
    public Collider2D generatorRoom;
    public Collider2D engineRoom;
    public Collider2D cockpitRoom;

    public AudioSource alarmSound;
    private bool alarm;

    public void Start()
    {
        wagonscript = FindObjectOfType<UIwagon>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        anim = GetComponentInParent<Animator>();

        wagonHealth = wagonHealthMax;
        deathScreen.SetActive(false);

        StartCoroutine(GenerateRandom(4));
    }

    void Update()
    {
        wagonHealth = Mathf.Clamp(wagonHealth, 0, wagonHealthMax);
        TakeDmg();
        CheckHealth();                      
    }

    private void LateUpdate()
    {
        if (TakingDamage)
        {
            Warning.SetActive(true);
        }
        else
        {
            Warning.SetActive(false);
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
            print("Recommence");
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
            print("Recommence");
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
            print("Recommence");
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
