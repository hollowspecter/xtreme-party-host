using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : AdvertisingObject {

    protected override void Awake()
    {
        base.Awake();

        // create toilet action
        AddToiletAction();
    }

    protected virtual void AddToiletAction()
    {
        Debug.Log("Add Toilet Action");
        KeyValuePair<NeedType, float>[] ads = new KeyValuePair<NeedType, float>[1];
        ads[0] = new KeyValuePair<NeedType, float>(NeedType.BLADDER, 100f);
        TimedAction toiletAction = new TimedAction(this, "Empty Bladder", ads, ads, OnEndToiletUsed, null, 5f);
        advertisedActions.Add(toiletAction);
    }

    protected virtual void OnEndToiletUsed()
    {
        AddToiletAction();
    }

    // gets called when toilet action is done!
    public override void RemoveAction(AbstractAction _action)
    {
        base.RemoveAction(_action);
    }
}
