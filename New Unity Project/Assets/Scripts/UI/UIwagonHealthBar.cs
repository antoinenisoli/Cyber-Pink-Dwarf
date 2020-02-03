using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIwagonHealthBar : MonoBehaviour
{
    public Slider slide;
    public GMscript gm;

    private void Start()
    {
        gm = FindObjectOfType<GMscript>();
    }

    void Update()
    {
        slide.value = gm.wagonHealth;
        slide.maxValue = gm.wagonHealthMax;
    }
}
