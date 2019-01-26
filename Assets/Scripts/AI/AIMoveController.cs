using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMoveController : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    CharacterMovement movement;
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
    private Vector3 lastPosition;

    private float perlinYCoordinate;

    private bool canWalk = true;

    private void Awake()
    {
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        //navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        movement = GetComponentInChildren<CharacterMovement>();
        perlinYCoordinate = Random.Range(0, 100);
    }

    private void Update()
    {
        if (canWalk && navMeshPath != null && navMeshPath.corners.Length > currentCorner)
        {
            toNextCornerVector = (navMeshPath.corners[currentCorner] - movement.transform.position);
            toNextCornerVector.y = 0;
            if (toNextCornerVector.magnitude > MOVEMENTTHRESHHOLD)
            {
                movement.Move(AddDrunknessToDirection(toNextCornerVector));
            }
            else
            {
                //corner was reached
                if (currentCorner < navMeshPath.corners.Length - 1)
                {
                    //if this was not the last corner on our path
                    currentCorner++;
                }
                else
                {
                    //if this was the last corner, clear the path
                    navMeshPath = new NavMeshPath();
                }
            }
        }
        else if(!canWalk)
        {
            movement.Stop();
        }
        else
        {
            DEBUGFindRandomLocation();
        }
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

    public void Stop()
    {
        StartCoroutine(DoStop());
    }

    IEnumerator DoStop()
    {
        canWalk = false;
        Debug.Log("false");
        yield return new WaitForSeconds(Mathf.Lerp(minCrashStopTime, maxCrashStopTime, drunkness));
        Debug.Log("true");
        canWalk = true;
    }

    public void Repath()
    {
        currentCorner = 0;
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
}