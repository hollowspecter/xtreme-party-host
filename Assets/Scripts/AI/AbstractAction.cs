﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// one action can only be used by one player?
public abstract class AbstractAction {

    public KeyValuePair<NeedType, float>[] AdvertisedReward { private set; get; }

    protected string name;
    public bool interrupted = false;
    public string Name { get { return name; } }
    protected KeyValuePair<NeedType, float>[] advertisedReward;
    protected KeyValuePair<NeedType, float>[] rewards;

    protected Needs needs;
    protected UnityAction onEnd;
    protected AdvertisingObject myObject;

    protected AbstractAction(AdvertisingObject _myObject, string _name, KeyValuePair<NeedType, float>[] _advertisedReward, KeyValuePair<NeedType, float>[] _rewards, UnityAction _onEnd)
    {
        myObject = _myObject;
        name = _name;
        advertisedReward = _advertisedReward;
        rewards = _rewards;
        onEnd = _onEnd;
    }

    public virtual void SetupActionForMe(Needs _needs)
    {
        needs = _needs; // sets the player and is now occupied

        if (myObject != null)
            myObject.GetAdvertisedActions().Remove(this);
    }

    public virtual bool Perform()
    {
        return EvaluatePrecondition();
    }
    
    abstract protected bool EvaluatePrecondition();

    /// <summary>
    /// Is called when progress is 1
    /// Describes what happens on the end of this action.
    /// will evaluate the reward on the needs
    /// </summary>
    public virtual void EndAction()
    {
        if (onEnd != null) onEnd();

        if (rewards != null)
        {
            foreach (KeyValuePair<NeedType, float> reward in rewards)
            {
                needs.RewardNeed(reward.Key, reward.Value);
            }
        }

        needs = null; // free the player
    }
}
