using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubbish : GeneralInteractable {


    static List<Rubbish> allRubbish = new List<Rubbish>();

    protected override void Awake()
    {
        base.Awake();
        ItemType = "Rubbish";
        ResetOnStop = true;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        allRubbish.Add(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        allRubbish.Remove(this);
    }

    protected override void OnInteractingFinish()
    {
        //Räume den Müll weg!
        Destroy(gameObject);
    }



    public float disgustness;



}
