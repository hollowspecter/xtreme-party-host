using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IActorInput {

    [SerializeField]
    private CharacterMovement usedMovement;
    [SerializeField]
    private int playerNumber;

    Vector2 desiredMovement;
    #region IActorInput

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
        throw new System.NotImplementedException();
    }

    public void MainActionPressed()
    {
        throw new System.NotImplementedException();
    }

    public void MainActionReleased()
    {
        throw new System.NotImplementedException();
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
            usedMovement.Move(desiredMovement);
        else
        {
            usedMovement.Move(Vector2.zero);
        }
    }

    // Use this for initialization
    void Start () {
        InputManager.Instance.SetPlayer(playerNumber-1, this);
        desiredMovement = Vector2.zero;
	}

}
