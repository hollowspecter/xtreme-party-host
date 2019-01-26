using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour {

    public Pizza pizza;

    public float actionTickInterval = 5f;
    public Queue<AbstractAction> actionQueue;

    private AbstractAction currentAction = null;
    private float tickTimer = 0f;

    protected virtual void Awake()
    {
        actionQueue = new Queue<AbstractAction>();
    }

    protected virtual void Update () {
        if (currentAction == null)
            UpdateActionTickTimer();
        else
            TryPerformCurrentAction();
    }

    protected virtual void UpdateActionTickTimer()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= actionTickInterval)
        {
            tickTimer = 0f;
            ActionTick();
        }
    }

    protected virtual void TryPerformCurrentAction()
    {
        Debug.Log("Try Perform Current Action");
        // check if it is in range

        if (currentAction.Perform()) currentAction = null;
    }

    protected virtual void ActionTick()
    {
        Debug.Log("ActionTick!");
        // are there any actions in the queue? if yes, take the next one
        if (actionQueue.Count == 0)
        {
            SelectAction();
        }
        if (actionQueue.Count > 0)
        {
            SetCurrentAction(actionQueue.Dequeue());
        }
    }

    protected virtual void SelectAction()
    {
        Debug.Log("Try Select an Action");
        // TODO: do correct action selection only on objects that are not occupied i guess or not performed rn? or let them wait?

        foreach (AbstractAction action in pizza.GetAdvertisedActions())
        {
            Debug.Log("Enqueue Action!");
            actionQueue.Enqueue(action);
        }
    }

    protected virtual void SetCurrentAction(AbstractAction _action)
    {
        Debug.Log("SetCurrentAction");
        currentAction = _action;
        currentAction.SetupActionForMe(GetComponent<Needs>());
    }
}
