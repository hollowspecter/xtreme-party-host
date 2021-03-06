﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AIMoveController : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    CharacterMovement movement;
    IKControl iKControl;
    const float MOVEMENTTHRESHHOLD = 0.1f;

    NavMeshPath navMeshPath;
    int currentCorner = 0;
    Vector3 toNextCornerVector;

    [Header("Drunkness")]
    [Range(0f, 1f)]
    public float drunkness = 0.0f;
    public float speed = 5;

    public float swerveAmount = 2f;
    public float swerveSpeed = 2;

    public float minCrashStopTime = 0.5f;
    public float maxCrashStopTime = 3f;

    [Header("Components")]
    public Transform target;

    [SerializeField]
    private Transform pukePosition;

    [SerializeField]
    private GameObject pukePrefab;

    private Vector3 lastPosition;

    private float perlinYCoordinate;

    private bool canWalk = true;

    private UnityAction PathComplete;

    //Matt: added for Konfetti throwing
    [SerializeField]
    private GameObject _konfettiPrefab;

    private void Awake()
    {
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        //navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        movement = GetComponentInChildren<CharacterMovement>();
        perlinYCoordinate = Random.Range(0, 100);
        iKControl = GetComponentInChildren<IKControl>();

        StartCoroutine(KonfettiTimer());
    }

    private void Update()
    {
        CheckIfNeedToPuke();
        if (canWalk && target && navMeshPath != null && navMeshPath.corners.Length > currentCorner)
        {
            toNextCornerVector = (navMeshPath.corners[currentCorner] - movement.transform.position);
            toNextCornerVector.y = 0;
            if (toNextCornerVector.magnitude > MOVEMENTTHRESHHOLD)
            {
                iKControl.isWalking = true;
                iKControl.walkAnimSpeed = movement.rigidBodyVelocity.magnitude;
                movement.Move(AddDrunknessToDirection(toNextCornerVector));
            }
            else
            {
                //corner was reached
                if (currentCorner < navMeshPath.corners.Length - 1)
                {
                    //if this was not the last corner on our path
                    currentCorner++;
                    toNextCornerVector = (navMeshPath.corners[currentCorner] - movement.transform.position);
                    toNextCornerVector.y = 0;
                    if (toNextCornerVector.magnitude > MOVEMENTTHRESHHOLD)
                    {

                        iKControl.isWalking = true;
                        iKControl.walkAnimSpeed = movement.rigidBodyVelocity.magnitude;
                        movement.Move(AddDrunknessToDirection(toNextCornerVector));
                    }
                }
                else
                {
                    //if this was the last corner, clear the path
                    StopPath();
                    if(PathComplete != null)
                        PathComplete();
                }
            }
        }
        else if(!canWalk)
        {
            movement.Stop();
            iKControl.isWalking = false;
        }
    }
    bool pukingInvoked = false;
    protected virtual void CheckIfNeedToPuke()
    {
        if (drunkness >= 1f && !pukingInvoked)
        {
            pukingInvoked = true;
            Invoke("Puke", 10f);
        }
        if (drunkness <= 0.8f && pukingInvoked)
            pukingInvoked = false;
    }

    // TODO JOSCHA MACH HIER DEIN KOTZE NEI
    private void Puke()
    {
        // spawn a puddle, add a vfx

        // make puddle a rubbish, with value 50f
        GameObject random = Instantiate<GameObject>(pukePrefab, pukePosition.position, pukePosition.rotation);
        RubbishSpawner.AddRubbishToObject(random, 50.0f, 8.0f);
    }

    private Vector3 AddDrunknessToDirection(Vector3 direction)
    {
        Vector3 delta = toNextCornerVector;
        Vector3 deltaP = new Vector3(-delta.y, 0, delta.x);
        Vector3 offset = deltaP * swerveAmount * ((Mathf.PerlinNoise(Time.time * swerveSpeed, perlinYCoordinate) - 0.5f) * 2f) * drunkness;
        Vector3 swerveTarget = navMeshPath.corners[currentCorner] + offset;
        Debug.DrawLine(movement.transform.position, swerveTarget, Color.cyan);
        return (swerveTarget - movement.transform.position).normalized;
    }

    private void DEBUGFindRandomLocation()
    {
        navMeshPath = new NavMeshPath();
        target = RandomTarget.instance.GetRandomTarget();
        Repath();
    }

    public void SetTarget(Transform target, UnityAction completionCallback)
    {
        this.target = target;
        PathComplete = completionCallback;
        Repath();
    }



    public void StopPath()
    {
        movement.Stop();
        iKControl.isWalking = false;
        navMeshPath = new NavMeshPath();
        target = null;
    }


    public void Crash()
    {
        StartCoroutine(DoCrash());
    }

    IEnumerator DoCrash()
    {
        canWalk = false;
        //Debug.Log("false");
        yield return new WaitForSeconds(Mathf.Lerp(minCrashStopTime, maxCrashStopTime, drunkness));
        //Debug.Log("true");
        canWalk = true;
    }

    public void Repath()
    {
        currentCorner = 0;
        navMeshPath = new NavMeshPath();
        if (target == null) return;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(target.position, out hit, 5f, NavMesh.AllAreas))
            navMeshAgent.CalculatePath(hit.position, navMeshPath);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (navMeshPath != null && navMeshPath.corners.Length > 0)
        {
            for (int i = 1; i < navMeshPath.corners.Length; i++)
            {
                Gizmos.DrawLine(navMeshPath.corners[i - 1], navMeshPath.corners[i]);
            }
        }

        if (navMeshPath != null && navMeshPath.corners.Length > currentCorner)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(navMeshPath.corners[currentCorner], movement.transform.position);

        }
    }

    IEnumerator KonfettiTimer()
    {
        int wait_time = Random.Range(10, 60);
        yield return new WaitForSeconds(wait_time);
        GameObject konf = Instantiate(_konfettiPrefab, pukePosition.position, Quaternion.Euler(-90f,0,0));
        RubbishSpawner.AddRubbishToObject(konf, 4.0f, 2.5f);
    }
}