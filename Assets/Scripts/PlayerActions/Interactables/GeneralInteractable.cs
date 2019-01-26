using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralInteractable : IInteractable {

    [SerializeField]
    private string ItemNeeded = "";



    protected override void Awake()
    {
    }

    public override void StartInteracting(PlayerActions playerAction)
    {
        //Kein Item required
        if (ItemNeeded.Equals("")) {
            base.StartInteracting(playerAction);
        }
        else
        {
            //Item wird gehalten
            if(playerAction.holdingItem)
            {
                IInteractable holdingItem = playerAction.holdingItem.GetComponent<IInteractable>();
                //Hat es benötigte Komponente, und ist es das richtige Item?
                if(holdingItem != null && holdingItem.Equals(ItemNeeded)) {
                    Debug.Log(ItemNeeded+" Requirement met!");
                    base.StartInteracting(playerAction);
                }
            }
            //Requirement nicht erfüllt
            else {
                Debug.Log(ItemNeeded+" Requirement needed!");
            }
        }
    }

    protected override void OnInteractingFinish()
    {
        base.OnInteractingFinish();

    }
}
