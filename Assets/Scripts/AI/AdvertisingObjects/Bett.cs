using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bett : AdvertisingObject
{
    public string actionName = "Have sex";
    public float socialValue = 30f;
    public float funValue = 30f;
    public float sexInterval = 10f;
    public float sexDuration = 20f;

    public TimedAction action1, action2;

    protected bool invoked = false;

    protected override void Awake()
    {
        base.Awake();

        Invoke("AddBedActions", sexInterval);
    }

    protected override void Update()
    {
        base.Update();

        if (action1 == null || action2 == null) return;

        if (action1.CheckDistance() && action2.CheckDistance())
        {
            action1.Paused = false;
            action2.Paused = false;
        }
        else
        {
            action1.Paused = true;
            action2.Paused = true;
        }
    }

    protected virtual void AddBedActions()
    {
        KeyValuePair<NeedType, float>[] ads = new KeyValuePair<NeedType, float>[2];
        ads[0] = new KeyValuePair<NeedType, float>(NeedType.SOCIAL, socialValue);
        ads[1] = new KeyValuePair<NeedType, float>(NeedType.FUN, funValue);
        action1 = new TimedAction(this, actionName + "1", ads, ads, OnEndBedAction, null, sexDuration);
        advertisedActions.Add(action1);
        action2 = new TimedAction(this, actionName + "2", ads, ads, OnEndBedAction, null, sexDuration);
        advertisedActions.Add(action2);

        // pause the actions so no progress can be made except walking there
        action1.Paused = true;
        action2.Paused = true;
    }

    protected virtual void OnEndBedAction()
    {
        Debug.Log("Finished Sex");
        if (action1 != null) action1.finishEarly = true;
        if (action2 != null) action2.finishEarly = true;
        action1 = null;
        action2 = null;
        
        if (!invoked)
        {
            Invoke("AddBedActions", sexInterval);
            invoked = true;
        }
        else
        {
            invoked = false;
        }
    }
}
