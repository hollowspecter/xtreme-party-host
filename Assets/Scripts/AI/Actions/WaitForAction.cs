using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaitForAction : AbstractAction
{
    public delegate bool OnCheckCondition();
    protected OnCheckCondition evaluateEndCondition;
    protected bool forcedSuccess;

    public WaitForAction(AdvertisingObject _myObject, string _name, KeyValuePair<NeedType, float>[] _advertisedReward, KeyValuePair<NeedType, float>[] _rewards, UnityAction _onEnd, UnityAction _onStart, OnCheckCondition _evaluateEndCondition)
    : base(_myObject, _name, _advertisedReward, _rewards, _onEnd, _onStart)
    {
        evaluateEndCondition = _evaluateEndCondition;
    }

    public override bool Perform()
    {
        if (!base.Perform()) return false;

        if (forcedSuccess || (evaluateEndCondition != null && evaluateEndCondition()))
        {
            if (onEnd != null) onEnd();
            return true;
        }

        return false;
    }

    public virtual void ForceSuccess()
    {
        forcedSuccess = true;
    }

    public override void EndInterrupted()
    {
    }

    public override void PausePerform()
    {
    }

    protected override bool EvaluatePrecondition()
    {
        // check distance to player
        float distance = Vector3.Distance(needs.transform.position, myObject.transform.position);
        if (distance <= myObject.interactionDistance)
        {
            return true;
        }

        // force enqueue a move action
        WalkAction walkAction = new WalkAction(myObject, null, null, null);
        needs.GetComponent<ActionManager>().ForceAction(walkAction);

        return false;
    }
}
