using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using TMPro;

public class ActionManager : MonoBehaviour {

    public float actionTickInterval = 5f;
    public ActionQueue actionQueue;
    public Transform hatpoint;
    public bool useDistanceAttenuation = false;
    public float maxTime = 10f;
    public AdvertisingObject npcSpawnpoint;

    [Header("Debug")]
    public TextMeshPro tmpdebug;

    private AbstractAction currentAction = null;
    private float tickTimer = 0f;
    private Needs needs;
    private float leaveTimer = -1f;
    private bool leaving = false;

    private float score;

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

        if (!leaving) CheckStayingAmount();
    }

    protected virtual void CheckStayingAmount()
    {
        leaveTimer += Time.deltaTime;
        if (leaveTimer >= maxTime)
        {


            // dont call this method again
            leaving = true;

            // save score and catch NaN
            if (!float.IsNaN(needs.CalculateMood()))
            {
                ScoreManager.instance.Score += needs.CalculateMood() * 10;
                Debug.Log("<color=red>I am getting out of here!</color>: " + gameObject.name + ":::" + ScoreManager.instance.Score);
                
                // queue leaving action
                actionQueue.Enqueue(new TimedAction(
                    npcSpawnpoint,
                    "Getting outta here",
                    null,
                    null,
                    () => { Destroy(gameObject); },//onend
                    () => { }, //onstart
                    3f));
            }

            else
            {
                // queue leaving action
                actionQueue.Enqueue(new TimedAction(
                    npcSpawnpoint,
                    "Getting outta here",
                    null,
                    null,
                    () => { Destroy(gameObject); },//onend
                    () => { }, //onstart
                    3f));
            }



        }
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
        if (currentAction.Name == "Dance to Music")
        {
            GetComponentInChildren<IKControl>().isDancing = true;
        }

        // actions will be thrown away either when they are canceled/interrupted or qhen they are performed successfully
        if (currentAction.interrupted || currentAction.Perform())
        {
            GetComponentInChildren<IKControl>().isDancing = false;

            if (currentAction.interrupted) currentAction.EndInterrupted();
            currentAction = null;

            // select the next action in the queue immedieatly
            if (actionQueue.Count > 0)
            {
                Debug.Log(gameObject.name + ": Follow up with next action!");
                ActionTick();
            }
        }
    }

    protected virtual void ActionTick()
    {
        //Debug.Log(gameObject.name + ": ActionTick!");
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
        //Debug.Log(gameObject.name + ": Try Select an Action");

        // go thru all objects
        List<KeyValuePair<float, AbstractAction>> listedActions = new List<KeyValuePair<float, AbstractAction>>();
        float  sqrDistance, currentMood, futureMood; //score,
        foreach (var obj in AdvertisingObject.allObjects)
        {
            // go thru all actions
            foreach(var action in obj.GetAdvertisedActions())
            {
                // calculate the score
                currentMood = needs.CalculateMood();
                futureMood = needs.CalculatePotentialMood(action.AdvertisedReward);
                score = currentMood - futureMood;

                if (useDistanceAttenuation)
                {
                    sqrDistance = (transform.position - action.MyObjectPosition).sqrMagnitude;
                    score = score / sqrDistance;
                }

                listedActions.Add(new KeyValuePair<float, AbstractAction>(score, action));
            }
        }

        if (listedActions.Count <= 0) return;

        // sort the scores and actions
        listedActions.Sort((emp1, emp2) => emp2.Key.CompareTo(emp1.Key));
        Debug.Log("<b>" + gameObject.name + " Score List</b>" + listedActions.PrintList());

        // queue one of the three best actions
        int numOptions = Mathf.Min(listedActions.Count, 2); //max 3 options
        int selectedOption = Random.Range(0, numOptions);
        AbstractAction selectedAction = listedActions[selectedOption].Value;
        Debug.Log(gameObject.name + ": Selected: " + selectedAction.Name);
        selectedAction.MyObject.RemoveAction(selectedAction); // remove action from advertised actions
        actionQueue.Enqueue(selectedAction);
    }

    protected virtual void SetCurrentAction(AbstractAction _action)
    {
        Debug.Log(gameObject.name + ": SetCurrentAction " + _action.Name);
        currentAction = _action;
        currentAction.SetupActionForMe(GetComponent<Needs>());
    }

    protected virtual void UpdateDebug()
    {
        if (currentAction == null) tmpdebug.text = "no action";
        else tmpdebug.text = currentAction.Name;
    }

//    protected virtual void OnDrawGizmosSelected()
//    {
//        if (actionQueue != null)
//            Handles.Label(transform.position + Vector3.up, actionQueue.ToString());
//        else
//            Handles.Label(transform.position + Vector3.up, "No Actions in the Queue");
//    }

    public virtual void ForceAction(AbstractAction _action)
    {
        Debug.Log(gameObject.name + ": Force Action: " + _action.Name);
        // store current action to front
        currentAction.PausePerform();
        actionQueue.EnqueueFront(currentAction);
        // take the forced action as current action
        SetCurrentAction(_action);
    }
}
