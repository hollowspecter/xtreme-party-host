using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IActorInput {

    [SerializeField]
    private CharacterMovement playerMovement;
    [SerializeField]
    private PlayerActions playerActions;

    [SerializeField]
    private int playerNumber;

    Vector2 desiredMovement;
    private IKControl ikcontrol;
    #region IActorInput

    void Awake()
    {
        ikcontrol = GetComponentInChildren<IKControl>();
    }

    public void AltActionDown()
    {
        throw new System.NotImplementedException();
    }

    public void AltActionPressed()
    {
        throw new System.NotImplementedException();
    }

    public void AltActionReleased()
    {
        throw new System.NotImplementedException();
    }

    public void DownPressed()
    {
        throw new System.NotImplementedException();
    }

    public void DownReleased()
    {
        throw new System.NotImplementedException();
    }

    public void HorizontalPosition(float value)
    {
        desiredMovement.x = value;
    }

    public void LeftPressed()
    {
        throw new System.NotImplementedException();
    }

    public void LeftReleased()
    {
        throw new System.NotImplementedException();
    }

    public void MainActionDown()
    {
        playerActions.StartAction();
    }

    public void MainActionPressed()
    {

    }

    public void MainActionReleased()
    {
        playerActions.StopAction();
    }

    public void MobileJoystickPosition(Vector2 value)
    {
        throw new System.NotImplementedException();
    }

    public void RespawnActionDown()
    {
        throw new System.NotImplementedException();
    }

    public void RespawnActionPressed()
    {
        throw new System.NotImplementedException();
    }

    public void RespawnActionReleased()
    {
        throw new System.NotImplementedException();
    }

    public void RightPressed()
    {
        throw new System.NotImplementedException();
    }

    public void RightReleased()
    {
        throw new System.NotImplementedException();
    }

    public void UpPressed()
    {
        throw new System.NotImplementedException();
    }

    public void UpReleased()
    {
        throw new System.NotImplementedException();
    }
    #endregion

    public void VerticalPosition(float value)
    {
        desiredMovement.y = value;
        if (desiredMovement.sqrMagnitude > 0.0f)
        {
            playerMovement.Move(desiredMovement);
            ikcontrol.walkAnimSpeed = Mathf.Clamp(playerMovement.rigidBodyVelocity.magnitude, 0f, 3f);
            ikcontrol.isWalking = true;
        }
        else
        {
            playerMovement.Move(Vector2.zero);
            ikcontrol.isWalking = false;
        }
    }

    // Use this for initialization
    void Start () {
        InputManager.Instance.SetPlayer(playerNumber, this);
        desiredMovement = Vector2.zero;
	}

}
