using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musik : AdvertisingObject
{
    public string actionName = "Dance to Music";
    public float socialValue = 10f;
    public float funValue = 10f;
    public float danceDurationMin = 5f;
    public float danceDurationMax = 15f;

    protected override void Awake()
    {
        base.Awake();

        AddDanceAction();
    }

    protected virtual void AddDanceAction()
    {
        KeyValuePair<NeedType, float>[] ads = new KeyValuePair<NeedType, float>[2];
        ads[0] = new KeyValuePair<NeedType, float>(NeedType.SOCIAL, socialValue);
        ads[1] = new KeyValuePair<NeedType, float>(NeedType.FUN, funValue);
        TimedAction danceAction = new TimedAction(this, actionName, ads, ads, OnDanceEnd, null, Random.Range(danceDurationMin, danceDurationMax));
        advertisedActions.Add(danceAction);
    }

    protected virtual void OnDanceEnd()
    {
        AddDanceAction();
    }
}
