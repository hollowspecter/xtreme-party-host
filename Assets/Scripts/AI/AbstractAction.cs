using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// one action can only be used by one player?
public abstract class AbstractAction {

    public KeyValuePair<NeedType, float>[] AdvertisedReward { get { return advertisedReward; } }

    protected string name;
    public bool interrupted = false;
    public string Name { get { return name; } }
    protected KeyValuePair<NeedType, float>[] advertisedReward;
    protected KeyValuePair<NeedType, float>[] rewards;

    protected Needs needs;
    public ActionManager ActionManager { get { return needs.GetComponent<ActionManager>(); } }
    protected UnityAction onEnd;
    protected UnityAction onStart;
    protected AdvertisingObject myObject;
    public AdvertisingObject MyObject { get { return myObject; } }
    public Vector3 MyObjectPosition { get { return myObject.transform.position; } }

    protected bool startedOnce = false;

    protected AbstractAction(AdvertisingObject _myObject, string _name, KeyValuePair<NeedType, float>[] _advertisedReward, KeyValuePair<NeedType, float>[] _rewards, UnityAction _onEnd = null, UnityAction _onStart = null)
    {
        myObject = _myObject;
        name = _name;
        advertisedReward = _advertisedReward;
        rewards = _rewards;
        onEnd = _onEnd;
        onStart = _onStart;
    }

    public virtual void SetupActionForMe(Needs _needs)
    {
        needs = _needs; // sets the player and is now occupied

        if (myObject != null)
            myObject.GetAdvertisedActions().Remove(this);
    }

    public virtual bool Perform()
    {
        bool evaluationResult = EvaluatePrecondition();
        if (!startedOnce && evaluationResult)
        {
            startedOnce = true;
            if (onStart != null) onStart();
        }

        return evaluationResult;
    }
    
    abstract protected bool EvaluatePrecondition();
    abstract public void PausePerform();
    abstract public void EndInterrupted();

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
