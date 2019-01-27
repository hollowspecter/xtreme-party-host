using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Toilet : AdvertisingObject {

    [SerializeField]
    private Transform smellyPoint;
    [SerializeField]
    private GameObject smellyParticle;

    private GameObject currentSmell;

    [Range(0,1)]
    public float probability = 0.0f;

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
        if(currentSmell != null)
        {
            return;
        }
        probability += 0.2f;
        if(Random.value <= probability )
        {
            //Fire Toilet event
            probability = 0.0f;
            currentSmell = Instantiate<GameObject>(smellyParticle, smellyPoint);
        }
    }

    // gets called when toilet action is done!
    public override void RemoveAction(AbstractAction _action)
    {
        base.RemoveAction(_action);
    }
}
