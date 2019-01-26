using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashFeeler : MonoBehaviour
{
    private AIMoveController aiMoveController;

    private void Awake()
    {
        aiMoveController = GetComponentInParent<AIMoveController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("crash");
        aiMoveController.Stop();
        aiMoveController.Repath();
    }
}
