using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedAction : AbstractAction
{
    protected float duration;
    private float timer = 0f;

    public TimedAction(AdvertisingObject _myObject, string _name, KeyValuePair<NeedType, float>[] _advertisedReward, KeyValuePair<NeedType, float>[] _rewards, UnityAction _onEnd, float _duration)
    : base(_myObject, _name, _advertisedReward, _rewards, _onEnd)
    {
        duration = _duration;
    }

    public override bool Perform()
    {
        timer += Time.deltaTime;

        if (timer >= duration)
        {
            timer = 0f;
            EndAction();
            return true;
        }
        else
            return false;
    }
}
