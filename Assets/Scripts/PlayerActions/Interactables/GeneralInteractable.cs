using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralInteractable : IInteractable {

    [SerializeField]
    protected string ItemNeeded = "";
    [SerializeField]
    protected bool AlternativeMethod;


    protected GameObject foundRequirement;



    protected override void Awake()
    {
    }

    public override bool StartInteracting(PlayerActions playerAction)
    {
        if (interactingPlayer)
            return false;
        //Kein Item required
        if (ItemNeeded.Equals("")) {
            return base.StartInteracting(playerAction);
        }
        else
        {
            bool requirementMet = playerAction.holdingItem;
            //Item wird gehalten
            if (requirementMet) {
                IInteractable holdingInteractable = playerAction.holdingItem.GetComponent<IInteractable>();
                requirementMet &= holdingInteractable != null && holdingInteractable.ItemType.Equals(ItemNeeded);
                //Hat es benötigte Komponente, und ist es das richtige Item?
                if (requirementMet)
                {
                    foundRequirement = playerAction.holdingItem;
                    return base.StartInteracting(playerAction);
                }
            }

            //Kein Item, kann ich alternativ damit antworten
            else if (AlternativeMethod) {
                foundRequirement = null;
                return base.StartInteracting(playerAction);
            }
        }
        return false;
    }


    protected override void OnInteractingFinish()
    {
        if(foundRequirement)
            ConsumeRequirement();
        else {
            AlternativeFinish();
        }
        base.OnInteractingFinish();
    }

    protected virtual void AlternativeFinish()
    {
        
    }

    protected virtual void ConsumeRequirement()
    {
        interactingPlayer.holdingItem = null;
        Destroy(foundRequirement);
    }
}
