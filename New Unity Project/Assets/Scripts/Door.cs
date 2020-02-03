using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public RoomManager gmn;
    public bool open;
    private Collider2D bloc;
    private SpriteRenderer spr;

    public Sprite closedSpr;
    public Collider2D closedCollider;

    public Sprite openSpr;
    public Collider2D openCollider;

    public Transform tp;

    public GameObject currentCam;
    public GameObject nextCam;

    public AudioSource sound;

    void Awake()
    {
        spr = GetComponentInChildren<SpriteRenderer>();
        bloc = GetComponent<Collider2D>();

        GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");

        foreach (GameObject cam in cameras)
        {
            cam.SetActive(false);
        }

        open = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 8)
        {
            col.transform.position = tp.position;
            Transition();
        }
    }

    void Update()
    {
        OpenDoor();
        CheckMachines();
    }    

    void CheckMachines()
    {
        if (gmn.cleanedRoom)
        {
            open = true;
        }
        else
        {
            open = false;
        }
    }

    void OpenDoor()
    {
        if (open)
        {
            bloc.isTrigger = true;
            spr.sprite = openSpr;
            openCollider.enabled = true;
            closedCollider.enabled = false;
        }
        else
        {
            bloc.isTrigger = false;
            spr.sprite = closedSpr;
            openCollider.enabled = false;
            closedCollider.enabled = true;
        }
    }

    void Transition()
    {
        gmn.ResetRoom();
        currentCam.SetActive(false);
        nextCam.SetActive(true);
        
        if (!sound.isPlaying)
        {
            sound.Play();
        }
    }
}
