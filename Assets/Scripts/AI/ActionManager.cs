using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class ActionManager : MonoBehaviour {

    public Pizza pizza;

    public float actionTickInterval = 5f;
    public ActionQueue actionQueue;

    [Header("Debug")]
    public TextMeshPro tmpdebug;

    private AbstractAction currentAction = null;
    private float tickTimer = 0f;

    protected virtual void Awake()
    {
        actionQueue = new ActionQueue();
    }

    protected virtual void Update () {
        if (currentAction == null)
            UpdateActionTickTimer();
        else
            TryPerformCurrentAction();

        if (tmpdebug) UpdateDebug();
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
        // actions will be thrown away either when they are canceled/interrupted or qhen they are performed successfully
        if (currentAction.interrupted || currentAction.Perform()) currentAction = null;
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

        // TODO pick only first one, and remove it using PICK
        foreach (AbstractAction action in pizza.GetAdvertisedActions())
        {
            Debug.Log("Enqueue Action!");

            actionQueue.Enqueue(pizza.PickAction(action));
            return;
        }
    }

    protected virtual void SetCurrentAction(AbstractAction _action)
    {
        Debug.Log("SetCurrentAction");
        currentAction = _action;
        currentAction.SetupActionForMe(GetComponent<Needs>());
    }

    protected virtual void UpdateDebug()
    {
        if (currentAction == null) tmpdebug.text = "no action";
        else tmpdebug.text = currentAction.Name;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (actionQueue != null)
            Handles.Label(transform.position + Vector3.up, actionQueue.ToString());
        else
            Handles.Label(transform.position + Vector3.up, "No Actions in the Queue");
    }

    public virtual void ForceAction(AbstractAction _action)
    {
        // store current action to front
        actionQueue.EnqueueFront(_action);
        // take the forced action as current action
        SetCurrentAction(_action);
    }
}
