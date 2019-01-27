using UnityEngine;
using System.Collections;

public class PickupPizza : PickupItem
{
    protected override void OnInteractingFinish()
    {
        base.OnInteractingFinish();

        GetComponent<Pizza>().enabled = true;
    }
}
