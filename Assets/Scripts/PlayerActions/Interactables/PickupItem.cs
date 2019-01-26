using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupItem : IInteractable
{
    protected override void OnInteractingFinish()
    {
        base.OnInteractingFinish();
        progressable = false;
        interactingPlayer.PickupObject(gameObject);
    }
}
