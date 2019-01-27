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
        Debug.Log("Activating Pizza");
        AddSlicesOfPizza();
    }

    protected virtual void AddSlicesOfPizza()
    {
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
        slicesLeft--;
        if (slicesLeft <= 0)
        {
            Debug.Log("Pizza is now empty and turns to rubbish!");
            slices[0].SetActive(false);
            RubbishSpawner.AddRubbishToObject(gameObject, 20f);
            //Destroy(gameObject);
        }
        else
        {
            slices[(slicesLeft)%slices.Length].SetActive(false);
        }
    }
}
