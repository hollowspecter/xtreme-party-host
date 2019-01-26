using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : AdvertisingObject {

    public float foodValue = 30f;
    public float eatingDuration = 10f;
    public int slicesLeft = 6;

    public GameObject[] slices;
    public GameObject box;

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

        for (int i = 0; i < slicesLeft+1; ++i)
        {
            TimedAction action = new TimedAction(this, "Eat Pizza " + i, ads, ads, OnSliceEaten, OnStartedEatingPizza, eatingDuration);
            advertisedActions.Add(action);
        }
    }

    protected virtual void OnStartedEatingPizza()
    {
        box.SetActive(false);
    }

    protected virtual void OnSliceEaten()
    {
        Debug.Log("On Slice Eaten!");
        slicesLeft--;
        slices[slicesLeft - 1].SetActive(false);
        if (slicesLeft == 0)
        {
            Debug.Log("Pizza is now empty!");
            Destroy(gameObject);
        }
    }
}
