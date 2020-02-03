using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterGameOver : MonoBehaviour
{
    public GameObject credits;

    private void Start()
    {
        credits.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !credits.activeSelf)
        {
            credits.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(2);
        }
    }
}
