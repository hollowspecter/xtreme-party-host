using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class ActionManager : MonoBehaviour {

    public float actionTickInterval = 5f;
    public ActionQueue actionQueue;

    [Header("Debug")]
    public TextMeshPro tmpdebug;

    private AbstractAction currentAction = null;
    private float tickTimer = 0f;
    private Needs needs;

    protected virtual void Awake()
    {
        actionQueue = new ActionQueue();
        needs = GetComponent<Needs>();
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

        // go thru all objects
        List<KeyValuePair<float, AbstractAction>> listedActions = new List<KeyValuePair<float, AbstractAction>>();
        float score;
        foreach (var obj in AdvertisingObject.allObjects)
        {
            // go thru all actions
            foreach(var action in obj.GetAdvertisedActions())
            {
                // calculate the score
                score = needs.CalculateMood() - needs.CalculatePotentialMood(action.AdvertisedReward);

                listedActions.Add(new KeyValuePair<float, AbstractAction>(score, action));
            }
        }

        if (listedActions.Count <= 0) return;

        // sort the scores and actions
        listedActions.Sort((emp1, emp2) => emp2.Key.CompareTo(emp1.Key));

        // queue one of the three best actions
        int numOptions = Mathf.Min(listedActions.Count, 2); //max 3 options
        int selectedOption = Random.Range(0, numOptions);
        AbstractAction selectedAction = listedActions[selectedOption].Value;
        Debug.Log("Selected: " + selectedAction.Name);
        actionQueue.Enqueue(selectedAction);
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
