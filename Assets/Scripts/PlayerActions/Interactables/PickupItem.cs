using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupItem : IInteractable
{
   
    protected override void OnInteractingFinish()
    {
        interactingPlayer.PickupObject(gameObject);
        progressable = false;
        base.OnInteractingFinish();
    }
}
