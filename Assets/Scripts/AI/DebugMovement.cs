using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMovement: MonoBehaviour
{
    public float movementSpeed = 5.0f;

    public void Move(Vector3 desiredDirection)
    {
        transform.position += desiredDirection * movementSpeed * Time.deltaTime;
    }
}
