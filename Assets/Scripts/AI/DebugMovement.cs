using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DebugMovement: MonoBehaviour
{
    public float movementSpeed = 5.0f;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 desiredDirection)
    {
        rigid.AddForce(desiredDirection * movementSpeed * 10 * Time.deltaTime);
    }
}
