using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIwagon : MonoBehaviour
{
    public GMscript gm;

    private int traveled = 0;
    public int normalSpeed;
    public int wrongSpeed;
    public int reduc = 10;

    public int total = 0;
    public Text affiche;
    private string km;

    void Start()
    {
        gm = FindObjectOfType<GMscript>();
    }

    private void OnEnable()
    {
        gm = FindObjectOfType<GMscript>();
    }

    void Update()
    {
        if (gm.wagonHealth > 0)
        {
            if (gm.TakingDamage)
            {
                traveled += wrongSpeed;
            }
            else
            {
                traveled += normalSpeed;
            }

            total = traveled / reduc;
        }

        total = Mathf.Clamp(total, 0, 999999);

        km = total + " Km";
       
        affiche.text = km;
    }
}
