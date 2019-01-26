using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WalkAction : AbstractAction
{
    bool walking = false;
    bool reached = false;

    public WalkAction(AdvertisingObject _myObject, KeyValuePair<NeedType, float>[] _advertisedReward, KeyValuePair<NeedType, float>[] _rewards, UnityAction _onEnd)
    : base(_myObject, "Walking to"+_myObject.name, _advertisedReward, _rewards, _onEnd)
    {

    }

    protected override bool EvaluatePrecondition()
    {
        // no precondition to check here
        return true;
    }

    public override bool Perform()
    {
        if (!base.Perform()) return false;

        if (myObject == null)
        {
            interrupted = true;
            return false;
        }

        // not walking towards rn?
        if (!walking)
        {
            // set the target and wait for the callback to be called
            needs.GetComponent<AIMoveController>().SetTarget(myObject.transform, TargetReached);
            walking = true;
        }
        // when walking
        else if (!reached)
        {
            // check if you are already in range
            float distance = Vector3.Distance(needs.transform.position, myObject.transform.position);
            if (distance <= myObject.interactionDistance)
            {
                needs.GetComponent<AIMoveController>().StopPath();
                TargetReached();
            }
        }

        // determine end condition
        if (reached)
        {
            EndAction();
            return true;
        }
        else return false;
    }

    protected virtual void TargetReached()
    {
        reached = true;
    }

    public override void PausePerform()
    {
        walking = false;
    }

    public override void EndInterrupted()
    {
        needs.GetComponent<AIMoveController>().StopPath();
    }
}
