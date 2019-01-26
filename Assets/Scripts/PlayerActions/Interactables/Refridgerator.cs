using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refridgerator : GeneralInteractable {

    [SerializeField]
    private int maxAmountOfBeer;


    public int beerAmount;

    [SerializeField]
    protected GameObject beerbottlePrefab;

    protected override void Awake()
    {
        base.Awake();
        AlternativeMethod = true;
        ItemNeeded = "Beercrate";
    }

    protected override void ConsumeRequirement()
    {
        base.ConsumeRequirement();
        beerAmount = maxAmountOfBeer;
    }

    protected override void AlternativeFinish()
    {
        base.AlternativeFinish();
        GameObject beerbottle = Instantiate(beerbottlePrefab);
        interactingPlayer.PickupObject(beerbottle);
        beerAmount--;
    }



}
