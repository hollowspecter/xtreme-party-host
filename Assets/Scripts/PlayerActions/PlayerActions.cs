using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerActions : MonoBehaviour {


    List<GameObject> interactablesList;
    IInteractable currentInteractingObject;
    IKControl ikcontrol;


    //Holding item?
    public GameObject holdingItem;

    public Transform holdingPoint;

	// Use this for initialization
	void Awake () {
        interactablesList = new List<GameObject>();
        ikcontrol = GetComponentInChildren<IKControl>();
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
            if(item != null && item.interactable)
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
        IInteractable interactable = objectToCarry.GetComponent<IInteractable>();
        if (interactable.ItemType == "Beercrate"
        || interactable.ItemType == "Pizza")
        {
            ikcontrol.GrabItem(true);
        }
        else
        {
            ikcontrol.GrabItem(true, true);
        }

        holdingItem = objectToCarry;
        holdingItem.GetComponent<Collider>().enabled = false;
        //holdingItem.transform.position = holdingPoint.transform.position;
        //holdingItem.transform.up = transform.up;
        holdingItem.transform.SetParent(holdingPoint.transform);
        holdingItem.transform.localRotation = Quaternion.identity;
        holdingItem.transform.localPosition = Vector3.zero;
        Rigidbody itemRig = holdingItem.GetComponent<Rigidbody>();
        if (itemRig)
            itemRig.isKinematic = true;
    }

    public void PutDownObject()
    {
        holdingItem.transform.SetParent(null);
        holdingItem.GetComponent<Collider>().enabled = true;
        if (holdingItem != null)
        {
            holdingItem.GetComponent<PickupItem>().ResetInteraction();
            Rigidbody itemRig = holdingItem.GetComponent<Rigidbody>();
            if (itemRig)
                itemRig.isKinematic = false;
        }
        holdingItem = null;
        ikcontrol.GrabItem(false);

    }

    public void ThrowObject()
    {

    }




}
