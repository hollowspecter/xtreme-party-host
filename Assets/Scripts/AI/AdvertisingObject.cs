using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO make it being occupied
public abstract class AdvertisingObject : MonoBehaviour {

    public static List<AdvertisingObject> allObjects = new List<AdvertisingObject>();

    protected List<AbstractAction> advertisedActions = new List<AbstractAction>();

    public bool occupied = false;

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
}
