using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public abstract class IInteractable : MonoBehaviour
{
    [SerializeField]
    protected float neededTime = 1.0f;

    public void setNeededTime(float _value)
    {
        neededTime = _value;
    }

    protected float interactionProgress = 0.0f;

    public bool ResetOnStop = false;

    protected PlayerActions interactingPlayer = null;

    protected bool progressable = true;
    public bool interactable = true;

    public string ItemType = "default";


    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    protected virtual void OnEnable()
    {
        progressable = true;
    }

    protected virtual void OnDisable()
    {
        progressable = false;
    }

    public virtual void StartInteracting(PlayerActions playerAction)
    {
        interactingPlayer = playerAction;
    }

    // Update is called once per frame
    public virtual void StopInteracting()
    {
        OnInteractingInterrupted();
    }

    public virtual void OnInteractionProgress(float progress)
    {
        if(progressable && interactingPlayer)
        {
            progress /= neededTime;
            bool wasFinished = 1.0f - interactionProgress <= float.Epsilon;
            interactionProgress = Mathf.Clamp01(interactionProgress + progress);
            if (1.0f - interactionProgress <= float.Epsilon && !wasFinished)
                OnInteractingFinish();
        }
    }

    protected virtual void OnInteractingFinish()
    {
        ResetInteraction();
    }
    protected virtual void OnInteractingInterrupted()
    {
        interactingPlayer = null;
        if (ResetOnStop)
            ResetInteraction();
    }

    public virtual void ResetInteraction()
    {
        interactingPlayer = null;
        interactionProgress = 0.0f;
        progressable = true;
    }
}