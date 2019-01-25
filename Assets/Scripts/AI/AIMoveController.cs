using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMoveController : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    DebugMovement debugViewMovement;
    const float MOVEMENTTHRESHHOLD = 0.125f;

    NavMeshPath navMeshPath;
    int currentCorner = 0;
    Vector3 toNextCornerVector;

    [Range(0f, 1f)]
    public float drunkness = 0.0f;
    private Vector3 drunkSteerDirection;
    const float MAXDRUNKANGLE = 10f;

    //DebugStuff
    Vector3 gizmoOfffset = new Vector3(0, 0.1f, 0);

    private void Awake()
    {
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        //navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        debugViewMovement = GetComponentInChildren<DebugMovement>();
        drunkSteerDirection = transform.forward;
    }

    private void Update()
    {
        if (navMeshPath != null && navMeshPath.corners.Length > currentCorner)
        {
            toNextCornerVector = (navMeshPath.corners[currentCorner] - debugViewMovement.transform.position);
            toNextCornerVector.y = 0;
            if (toNextCornerVector.magnitude > MOVEMENTTHRESHHOLD)
            {
                debugViewMovement.Move(AddDrunknessToDirection(toNextCornerVector).normalized);
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
        else
        {
            DEBUGFindRandomLocation();
        }
    }

    private Vector3 AddDrunknessToDirection(Vector3 direction)
    {
        float maxDeviationAngle = MAXDRUNKANGLE;
        float deviationAngle = Random.Range(-maxDeviationAngle, maxDeviationAngle);
        drunkSteerDirection = Quaternion.AngleAxis(deviationAngle, Vector3.up)  * drunkSteerDirection;
        //drunkSteerDirection = Vector3.Lerp(drunkSteerDirection, toNextCornerVector, Time.deltaTime * 1f * (1.0f-drunkness));
        return Vector3.Lerp(toNextCornerVector, drunkSteerDirection, drunkness);
    }

    private void DEBUGFindRandomLocation()
    {
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        navMeshPath = new NavMeshPath();
        currentCorner = 0;
        navMeshAgent.CalculatePath(new Vector3(x, 0, z), navMeshPath);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if(navMeshPath != null && navMeshPath.corners.Length > 0)
        {
            for(int i = 1; i < navMeshPath.corners.Length; i++)
            {
                Gizmos.DrawLine(navMeshPath.corners[i - 1], navMeshPath.corners[i]);
            }
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, drunkSteerDirection * 2f);
        if (navMeshPath != null && navMeshPath.corners.Length > currentCorner)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(navMeshPath.corners[currentCorner], debugViewMovement.transform.position);

        }
    }
}
