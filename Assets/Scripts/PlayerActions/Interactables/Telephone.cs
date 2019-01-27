using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telephone : GeneralInteractable {

    [Header("Pizza Delivery")]
    public Transform spawnPoint;
    public GameObject deliveryPrefab;
    public Vector2 minMaxDeliveryDuration = new Vector2(15f, 30f);
    public AudioClip[] soundeffect;

    protected bool orderOnTheWay;
    private int index = 0;

    protected override void AlternativeFinish()
    {
        base.AlternativeFinish();

        if (orderOnTheWay) return;
        Invoke("Deliver", Random.Range(minMaxDeliveryDuration.x, minMaxDeliveryDuration.y));
    }

    protected virtual void Deliver()
    {
        Instantiate(deliveryPrefab, spawnPoint.position + Vector3.up * 2f, Quaternion.identity);
        orderOnTheWay = false;
        GetComponent<AudioSource>().PlayOneShot(soundeffect[(index++)%2], 3f);
    }
}
