using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterMovement))]
public class PlayerActions : MonoBehaviour {

    [SerializeField]
    List<GameObject> interactablesList;
    IInteractable currentInteractingObject;
    IKControl ikcontrol;
    CharacterMovement movement;

    //Holding item?
    public GameObject holdingItem;

    public Transform holdingPoint;

	// Use this for initialization
	void Awake () {
        interactablesList = new List<GameObject>();
        ikcontrol = GetComponentInChildren<IKControl>();
        movement = GetComponent<CharacterMovement>();
	}
	
	// Update is called once per frame
	void Update () {
        ProgressAction();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerInteractable"))
        {
                interactablesList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
            interactablesList.Remove(other.gameObject);
    }

    public void RemoveFromList(GameObject _item)
    {
        interactablesList.Remove(_item);
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
            Debug.Log("Interacting with: " + closest.name);
            movement.movementBlocked = true;
            currentInteractingObject = closest.GetComponent<IInteractable>();
            currentInteractingObject.StartInteracting(this);

            if (currentInteractingObject.ItemType == "Refridgerator")
                GetComponent<UsePPSound>().PlayFridgeBeer();
            else if (currentInteractingObject.ItemType == "Telephone")
                GetComponent<UsePPSound>().PlayTelephoneCall();
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
            movement.movementBlocked = false;
        }
    }

    private void ProgressAction()
    {
        if (currentInteractingObject != null) {
            if (!currentInteractingObject.isPlayerInteracting(this))
            {
                movement.movementBlocked = false;
                currentInteractingObject = null;
            }
            else
                currentInteractingObject.OnInteractionProgress(Time.deltaTime);
        }
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
        interactable.interactable = false;
        Collider itemCollider = holdingItem.GetComponent<Collider>();
        itemCollider.enabled = false;
        OnTriggerExit(itemCollider);
        //holdingItem.transform.position = holdingPoint.transform.position;
        //holdingItem.transform.up = transform.up;
        holdingItem.transform.SetParent(holdingPoint.transform);
        holdingItem.transform.localRotation = Quaternion.identity;

        if (interactable.ItemType == "Beercrate"
        || interactable.ItemType == "Pizza")
        {
            holdingItem.transform.up = transform.up;
        }

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
            IInteractable interactble = holdingItem.GetComponent<IInteractable>();
            {
                interactble.ResetInteraction();
                interactble.interactable = true;
            }
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
