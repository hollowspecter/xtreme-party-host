using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTarget : MonoBehaviour
{
    public static RandomTarget instance;
    public Transform[] targets;


    private void Awake()
    {
        instance = this;
    }
    public Transform GetRandomTarget()
    {
        return targets[Random.Range(0, targets.Length - 1)];
    }

}
