using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedheadAnimManager : MonoBehaviour
{
    public GameObject redhead;
    public GameObject heart;
    public AudioSource deathSound;

    public void Destruction()
    {
        int random = Random.Range(1, 11);

        if (random == 1)
        {
            Instantiate(heart, transform.position, Quaternion.identity);
        }

        print(random);
        Destroy(redhead);
    }

    public void PlaySound()
    {
        if (!deathSound.isPlaying)
        {
            deathSound.Play();
        }
    }
}
