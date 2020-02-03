using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineAnimationManager : MonoBehaviour
{
    private Machine MachineScript;
    public AudioSource repairedSound;

    private void Update()
    {
        MachineScript = GetComponentInParent<Machine>();
    }

    void Activation()
    {
        MachineScript.isWorking = true;
        repairedSound.Play();
    }
}
