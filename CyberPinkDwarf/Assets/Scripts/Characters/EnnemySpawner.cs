using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemySpawner : MonoBehaviour
{
    public GameObject mobPrefab;

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((transform.position + new Vector3(0.5f,-0.5f)), new Vector3(1, 1, 1));
    }
    public void Spawn()
    {
        Instantiate(mobPrefab, transform.position, Quaternion.identity);
    }

    public void Awake()
    {
        Spawn();
    }

}
