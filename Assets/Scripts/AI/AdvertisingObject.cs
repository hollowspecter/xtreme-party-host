using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO make it being occupied
public abstract class AdvertisingObject : MonoBehaviour {

    public static List<AdvertisingObject> allObjects = new List<AdvertisingObject>();

    protected List<AbstractAction> advertisedActions = new List<AbstractAction>();

    public float interactionDistance = 2f; //what distance do you need to interact with it

    protected virtual void Awake()
    {
    
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    protected virtual void OnEnable()
    {
        allObjects.Add(this);
    }

    protected virtual void OnDisable()
    {
        allObjects.Remove(this);
    }

    public virtual List<AbstractAction> GetAdvertisedActions()
    {
        return advertisedActions;
    }

    public virtual AbstractAction PickAction(AbstractAction _action)
    {
        if (advertisedActions.Contains(_action))
        {
            advertisedActions.Remove(_action);
            return _action;
        }
        else
        {
            Debug.LogError("This action is not available anymore. Check why");
            return null;
        }
    }

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}
