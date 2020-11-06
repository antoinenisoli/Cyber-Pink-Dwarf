using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIlifebar : MonoBehaviour
{
    public Image image;
    public PlayerController playerScript;

    public Sprite ZeroLife;
    public Sprite OneLife;
    public Sprite TwoLifes;
    public Sprite ThreeLifes;
    

    void Start()
    {
        playerScript = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (playerScript.health == 3)
        {
            image.sprite = ThreeLifes;
        }
        else if (playerScript.health == 2)
        {
            image.sprite = TwoLifes;
        }
        else if (playerScript.health == 1)
        {
            image.sprite = OneLife;
        }
        else if (playerScript.health <= 0)
        {
            image.sprite = ZeroLife;
        }
    }
}
