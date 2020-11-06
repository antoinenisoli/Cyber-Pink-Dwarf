using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour
{
    private UIwagon km;
    public Text texte;

    private void OnEnable()
    {
        km = FindObjectOfType<UIwagon>();
    }

    private void Update()
    {
        string text = km.total + " Km";
        texte.text = text;
    }
}
