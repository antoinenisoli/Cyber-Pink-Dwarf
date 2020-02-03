using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public AudioSource completeRoomSound;
    private bool songPlayed;
    public float radius;
    public bool cleanedRoom;
    public GMscript gm;
    public Collider2D ThisRoom;
    public bool Sabotage;

    public GameObject startCam;
    public GameObject[] cameras;
    public GameObject[] rooms;

    public LayerMask doorLayer;
    public Collider2D[] doors;

    public LayerMask workingMachineLayer;
    public Collider2D[] workingMachines;

    public LayerMask machineLayer;
    public Collider2D[] machines;

    public LayerMask ennemyLayer;
    public Collider2D[] ennemies;

    public LayerMask spawnLayer;
    public Collider2D[] spawnPoints;

    void Start()
    {
        gm = FindObjectOfType<GMscript>();
        ThisRoom = GetComponentInParent<Collider2D>();

        if (startCam != null)
        {
            startCam.SetActive(true);
        }
    }

    void Update()
    {
        machines = Physics2D.OverlapCircleAll(transform.position, radius, machineLayer);
        doors = Physics2D.OverlapCircleAll(transform.position, radius, doorLayer);
        workingMachines = Physics2D.OverlapCircleAll(transform.position, radius, workingMachineLayer);
        ennemies = Physics2D.OverlapCircleAll(transform.position, radius, ennemyLayer);
        spawnPoints = Physics2D.OverlapCircleAll(transform.position, radius, spawnLayer);

        FindCameras();
        FindRooms();

        if (gm.selectedRoom == ThisRoom)
        {
            Sabotage = true;
        }
    }

    void LateUpdate()
    {
        if (machines.Length != 0) //machines cassées dans la room
        {
            cleanedRoom = false;
            songPlayed = false;
        }
        else
        {
            cleanedRoom = true;

            if (cleanedRoom && !songPlayed)
            {
                if (!completeRoomSound.isPlaying)
                {
                    completeRoomSound.Play();
                    songPlayed = true;
                }
            }
        }

        if (Sabotage && cleanedRoom)
        {
            gm.selectedRoom = null;
            Sabotage = false;
            gm.wagonHealth += 20;
            gm.CallRandomGenerator(0);
        }       
    }

    public void ResetRoom()
    {
        cleanedRoom = false;

        foreach(Collider2D ennemy in ennemies)
        {
            Destroy(ennemy);
        }

        foreach(Collider2D spawn in spawnPoints)
        {
            spawn.gameObject.GetComponent<EnnemySpawner>().Spawn();
        }

        foreach (Collider2D door in doors)
        {
            if (door.gameObject.GetComponent<Door>().open)
            {
                door.gameObject.GetComponent<Door>().open = false;
            }
            else
            {
                door.gameObject.GetComponent<Door>().open = true;
            }            
        }
        
        foreach (Collider2D fixedMachine in workingMachines)
        {
            if (fixedMachine.GetComponent<Machine>().startWorking)
            {
                fixedMachine.gameObject.layer = 14;
                fixedMachine.GetComponent<Machine>().isWorking = true;
            }
            else
            {
                fixedMachine.gameObject.layer = 11;
                fixedMachine.GetComponent<Machine>().isWorking = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void FindCameras()
    {
        cameras = GameObject.FindGameObjectsWithTag("MainCamera");

        foreach (GameObject cam in cameras)
        {
            
        }
    }

    void FindRooms()
    {
        rooms = GameObject.FindGameObjectsWithTag("Room");
    }
}
