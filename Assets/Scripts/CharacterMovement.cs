using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour {


    //Caps
    [SerializeField]
    private float maxVelocity;
    [SerializeField]
    private float maxRotationalVelocity;

    //Rates
    [SerializeField]
    private float acceleration;

    //Current Values
    private float currentVelocity;
    private float currentAngularVelocity;
    private Vector2 desiredVector;
    private float desiredVelocity;

    private float angleDiff;
    private float angleAccuracy = 45.0f;

    private Rigidbody rig;
    public bool isAIMovement = false;

    private void Start()
    {
        rig = gameObject.GetComponent<Rigidbody>();
        rig.maxAngularVelocity = maxRotationalVelocity;
    }

    // Update is called once per frame
    void FixedUpdate () {
        UpdateVelocities();
    }

    public void Move(Vector2 desiredDirection)
    {
        desiredVector = desiredDirection;
        Vector2.ClampMagnitude(desiredVector, 1.0f);
        desiredVelocity = desiredVector.magnitude * maxVelocity;
    }

    public void Move(Vector3 desiredDirection)
    {
        desiredVector.x = desiredDirection.x;
        desiredVector.y = desiredDirection.z;
        Move(desiredVector.normalized);
    }

    public void Stop()
    {
        desiredVector = Vector2.zero;
        rig.velocity = Vector3.zero;
    }

    private void UpdateVelocities()
    {
        angleDiff = Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), desiredVector);
        //Debug.DrawLine(transform.position, transform.position + 10.0f * transform.forward, Color.blue);
        //Debug.DrawLine(transform.position, transform.position + 10.0f * new Vector3(desiredVector.x, 0.0f, desiredVector.y), Color.red);

        if (desiredVector.Equals(Vector2.zero))
        {
            rig.angularVelocity = Vector3.zero;
            rig.velocity = Vector3.zero;
            return;
        }
        
        //Wir müssen drehen
        if (Mathf.Abs(angleDiff) >= angleAccuracy)
        {
            if (isAIMovement)
                rig.velocity *= 0.75f;
            else
                rig.velocity = Vector3.zero;
            if (angleDiff > 0.0f)
            {
                
                rig.angularVelocity = transform.up * -maxRotationalVelocity;
            }
            else
            {
                rig.angularVelocity =transform.up * maxRotationalVelocity;
            }
        }
        else
        {
            float rigVelo = rig.velocity.magnitude;
            if (rigVelo < desiredVelocity)
            {
                currentVelocity = Mathf.Clamp(currentVelocity + Time.deltaTime + acceleration, 0.0f, desiredVelocity);
            }
            else if (rigVelo > desiredVelocity)
            { 
                currentVelocity = Mathf.Clamp(currentVelocity - Time.deltaTime - acceleration, desiredVelocity, maxVelocity);
            }
            transform.forward = new Vector3(desiredVector.x, transform.forward.y, desiredVector.y);
            rig.angularVelocity = Vector3.zero;
            rig.velocity = transform.forward * currentVelocity;
        }
    }

}
