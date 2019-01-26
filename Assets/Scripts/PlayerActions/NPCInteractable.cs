using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : GeneralInteractable {

    public Transform hand;

    protected GameObject beerBottle;
    protected IKControl ikcontrol;

    protected override void Awake()
    {
        base.Awake();

        ItemNeeded = "Beerbottle";
        ikcontrol = GetComponentInChildren<IKControl>();
    }

    protected override void ConsumeRequirement()
    {
        beerBottle = interactingPlayer.holdingItem;

        interactingPlayer.PutDownObject();

        Rigidbody itemRig = beerBottle.GetComponent<Rigidbody>();
        if (itemRig)
            itemRig.isKinematic = true;
        itemRig.GetComponent<Collider>().enabled = false;
        beerBottle.transform.parent = hand;
        beerBottle.transform.localPosition = Vector3.zero;
        Destroy(beerBottle.GetComponent<PickupItem>());
        interactable = false;
        ikcontrol.GrabItem(true, true);

        AddBeerDrinkingAction();
    }

    protected virtual void AddBeerDrinkingAction()
    {
        KeyValuePair<NeedType, float>[] rewards = new KeyValuePair<NeedType, float>[2];
        rewards[0] = new KeyValuePair<NeedType, float>(NeedType.FUN, 30f);
        rewards[1] = new KeyValuePair<NeedType, float>(NeedType.BLADDER, -20f);

        TimedAction action = new TimedAction(
        beerBottle.GetComponent<AdvertisingObject>(),
            "Drinking beer",
        null,
        rewards,
        () => { // onend
            beerBottle.transform.parent = null;
            beerBottle.GetComponent<Rigidbody>().isKinematic = false;
            beerBottle.GetComponent<Collider>().enabled = true;
            RubbishSpawner.AddRubbishToObject(beerBottle, 5f, 2f);
            interactable = true;
            GetComponent<AIMoveController>().drunkness += 0.2f;
            ikcontrol.DrinkItem(false);
            ikcontrol.GrabItem(false);
        },
        () => {
            ikcontrol.DrinkItem(true);
        }, // on start
        5f); // dirnking duration

        GetComponent<ActionManager>().actionQueue.Enqueue(action);
    }
}
