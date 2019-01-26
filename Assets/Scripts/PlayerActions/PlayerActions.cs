using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerActions : MonoBehaviour {


    List<GameObject> interactablesList;
    IInteractable currentInteractingObject;


    //Holding item?
    public GameObject holdingItem;

    public Transform holdingPoint;

	// Use this for initialization
	void Awake () {
        interactablesList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        ProgressAction();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.CompareTag("PlayerInteractable"))
        //{
            IInteractable item = other.GetComponent<IInteractable>();
            if(item != null)
                interactablesList.Add(other.gameObject);
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //if(other.CompareTag("PlayerInteractable"))
        //{
            IInteractable item = other.GetComponent<IInteractable>();
            if (item != null)
                interactablesList.Remove(other.gameObject);
        //}
    }

    public void Dance()
    {
        Debug.Log("Im dancing!");
    }

    public void StartAction()
    {
        float closestDistance = 100.0f;
        GameObject closest = null;
        foreach(GameObject obj in interactablesList)
        {
            if (obj.Equals(holdingItem))
                continue;
            float curDist = Vector3.Distance(obj.transform.position, transform.position);
            if (curDist< closestDistance)
            {
                closest = obj;
                closestDistance = curDist;
            }
        }
        if(closest) {
            Debug.Log(closest.name);
            currentInteractingObject = closest.GetComponent<IInteractable>();
            currentInteractingObject.StartInteracting(this);
        }
        else if(holdingItem){
            PutDownObject();
        }
    }



    public void StopAction()
    {
        if(currentInteractingObject)
        {
            currentInteractingObject.StopInteracting();
            currentInteractingObject = null;
        }
    }

    private void ProgressAction()
    {
        if (currentInteractingObject != null)
            currentInteractingObject.OnInteractionProgress(Time.deltaTime);
    }

    public void PickupObject(GameObject objectToCarry)
    {
        holdingItem = objectToCarry;
        holdingItem.transform.position = holdingPoint.transform.position;
        holdingItem.transform.up = transform.up;
        objectToCarry.transform.SetParent(transform);
    }

    public void PutDownObject()
    {
        holdingItem.transform.SetParent(null);
        if (holdingItem != null)
        {
            holdingItem.GetComponent<PickupItem>().ResetInteraction();
        }
        holdingItem = null;
    }

    public void ThrowObject()
    {

    }




}
