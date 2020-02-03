using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stay : MonoBehaviour
{
    public static Stay musicInstance;
    public AudioSource mainMusic;
    public bool isplaying;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (musicInstance == null)
        {
            musicInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        
        if (!mainMusic.isPlaying)
        {
            mainMusic.Play();
            isplaying = false;
        }
        else
        {
            isplaying = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
