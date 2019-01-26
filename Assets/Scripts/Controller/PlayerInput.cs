using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    [SerializeField]
    private CharacterMovement usedMovement;
	// Use this for initialization
	void Start () {     
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 desiredDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if(desiredDirection.sqrMagnitude > 0.0f)
            usedMovement.Move(desiredDirection);
        else
        {
            usedMovement.Move(Vector2.zero);
        }

    }
}
