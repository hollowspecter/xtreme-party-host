using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    [SerializeField]
    private CharacterMovement usedMovement;

    [SerializeField]
    private float deadZone = 0.125f;
	// Use this for initialization
	void Start () {     
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 desiredDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Mathf.Abs(desiredDirection.x) <= deadZone)
            desiredDirection.x = 0.0f;

        if (Mathf.Abs(desiredDirection.y) <= deadZone)
            desiredDirection.y = .0f;

        if(desiredDirection.sqrMagnitude > 0.0f)
            usedMovement.Move(desiredDirection.normalized);
        else
        {
            usedMovement.Move(Vector2.zero);
        }

    }
}
