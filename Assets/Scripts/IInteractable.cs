using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{


    void StartInteracting();

    // Update is called once per frame
    void StopInteracting();

    void OnInteractingFinish();
    void OnInteractingInterrupted();
}