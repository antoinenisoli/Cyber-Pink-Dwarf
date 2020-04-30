using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    public bool isWorking;
    public bool startWorking;
    private Animator anim;
    public GameObject progressBar;
    public bool repairing;

    public Collider2D thisRoom;
    public Door[] doors;
    public AudioSource repairingSound;
    public AudioSource destroySound;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        startWorking = isWorking;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Room"))
        {
            thisRoom = col;
        }
    }

    public void PlaySound()
    {
        destroySound.Play();
    }

    void Update()
    {
        if (isWorking)
        {
            anim.SetBool("fixed", true);
            gameObject.layer = 14;
            repairing = false;
        }
        else
        {
            anim.SetBool("fixed", false);
            gameObject.layer = 11;
        }

        DoorsManager();
        Progress();
    }

    private void DoorsManager()
    {
        foreach(Door porte in doors)
        {
            if (isWorking)
            {
                porte.open = true;
            }
            else
            {
                porte.open = false;
            }
        }
    }

    public void Progress()
    {
        Animator barAnim = progressBar.GetComponent<Animator>();

        if (repairing)
        {
            progressBar.SetActive(true);
            barAnim.SetBool("isRepairing", true);

            if (!repairingSound.isPlaying)
            {
                repairingSound.Play();
            }
        }
        else
        {            
            barAnim.SetBool("isRepairing", false);
            progressBar.SetActive(false);

            if (repairingSound.isPlaying)
            {
                repairingSound.Stop();
            }
        }
    }
}
