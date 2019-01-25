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

    private float angleDiff;
    private float angleAccuracy = 30.0f;

    private Rigidbody rig;

    private void Start()
    {
        rig = gameObject.GetComponent<Rigidbody>();
        rig.maxAngularVelocity = maxRotationalVelocity;
    }

    // Update is called once per frame
    void FixedUpdate () {
        UpdateVelocities();
        //UpdatePositions();
    }

    public void Move(Vector2 desiredDirection)
    {
        desiredVector = desiredDirection;
    }

    private void UpdateVelocities()
    {
        angleDiff = Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), desiredVector);
        Debug.DrawLine(transform.position, transform.position + 10.0f * transform.forward, Color.blue);
        Debug.DrawLine(transform.position, transform.position + 10.0f * new Vector3(desiredVector.x, 0.0f, desiredVector.y), Color.red);

        if (desiredVector.Equals(Vector2.zero))
        {
            rig.angularVelocity = Vector3.zero;
            rig.velocity = Vector3.zero;
            return;
        }
        
        //Wir müssen drehen
        if (Mathf.Abs(angleDiff) >= angleAccuracy)
        {
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
            rig.angularVelocity = Vector3.zero;
            currentVelocity = Mathf.Clamp(currentVelocity + Time.deltaTime + acceleration, 0.0f, maxVelocity);
            rig.velocity = transform.forward * currentVelocity;
        }
    }


    private void UpdatePositions()
    { 
        transform.position += new Vector3(desiredVector.x, 0, desiredVector.y).normalized * (currentVelocity * Time.deltaTime);

        if (!desiredVector.Equals(Vector2.zero))
        {
            if(Mathf.Abs(angleDiff) <= angleAccuracy)
                transform.forward = new Vector3(desiredVector.x, transform.forward.y, desiredVector.y);
            else
            {
                transform.Rotate(Vector3.up, Time.deltaTime * currentAngularVelocity);
            }
        }
    }


}
