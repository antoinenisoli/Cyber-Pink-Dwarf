using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class script : MonoBehaviour
{
    public GameObject credits;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            credits.SetActive(false);
            SceneManager.LoadScene(0);
        }
    }
}
