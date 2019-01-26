using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedAction : AbstractAction
{
    protected float duration;
    private float timer = 0f;
    
    public TimedAction(AdvertisingObject _myObject, string _name, KeyValuePair<NeedType, float>[] _advertisedReward, KeyValuePair<NeedType, float>[] _rewards, UnityAction _onEnd, UnityAction _onStart, float _duration)
    : base(_myObject, _name, _advertisedReward, _rewards, _onEnd, _onStart)
    {
        duration = _duration;
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

    public override bool Perform()
    {
        if (!base.Perform()) return false;

        timer += Time.deltaTime;

        if (timer >= duration)
        {
            EndAction();
            return true;
        }
        else
            return false;
    }

    public static TimedAction CreateWaitAction(float _duration, string _name)
    {
        return new TimedAction(null, _name, null, null, null, null, _duration);
    }

    public override void PausePerform()
    {

    }

    public override void EndInterrupted()
    {

    }
}
