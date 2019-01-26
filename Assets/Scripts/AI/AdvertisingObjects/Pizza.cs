using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : AdvertisingObject {

    public float foodValue = 30f;
    public float eatingDuration = 10f;
    public int slicesLeft = 3;

    protected override void Awake()
    {
        base.Awake();

        AddSlicesOfPizza();
    }

    protected virtual void AddSlicesOfPizza()
    {
        Debug.Log("Add Slice of Pizza");
        // Create the consume action
        KeyValuePair<NeedType, float>[] ads = new KeyValuePair<NeedType, float>[1];
        ads[0] = new KeyValuePair<NeedType, float>(NeedType.HUNGER, foodValue);
        TimedAction action = new TimedAction(this, "Eat", ads, ads, OnSliceEaten, eatingDuration);
        advertisedActions.Add(action);
    }

    protected virtual void OnSliceEaten()
    {
        Debug.Log("On Slice Eaten!");
        slicesLeft--;
        if (slicesLeft > 0) AddSlicesOfPizza();
    }
}
