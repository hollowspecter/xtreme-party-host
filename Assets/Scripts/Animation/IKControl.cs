using UnityEngine;
using System;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(Animator))]

public class IKControl : MonoBehaviour
{

    protected Animator animator;
    Rigidbody rigid;

    [Header ("Toggles")]
    public bool ikActive = false;
    public bool headIkActive = false;
    public bool isWalking;
    public bool isDancing;


    public bool holdingItem;
    public bool holdingOnlyRight;

    public bool drinkingItem;

    [Header("Animation")]
    public AnimationCurve easeIn;

    [Header("Handles")]
    public Transform rightHandObj = null;
    public Transform leftHandObj = null;
    public Transform rightFootObj = null;
    public Transform leftFootObj = null;
    public Transform lookObj = null;

    [Header ("Hands")]
    public Transform grabObjectPosition;
    private Vector3 defaultRightHandPosition;
    private Quaternion defaultRightHandRotation;
    private Vector3 defaultLeftHandPosition;
    private Quaternion defaultLeftHandRotation;
    float grabWidth = 0.125f;

    [Header("Feet")]
    private Vector3 defaultRightFootPosition;
    private Vector3 defaultLeftFootPosition;
    public float walkAnimSpeed = 1f;
    public AnimationCurve feetCurve;

    [Header("Head")]
    private Vector3 defaultHeadPosition;

    [Header("AnimPositions")]
    public Transform drinkObjectHandPosition;
    public Transform drinkObjectLookPosition;
    public float drinkTiltBackAngle;

    [Header("Dancing")]
    public float danceAnimSpeed = 1f;
    public Dances chosenDance = Dances.Shuffle;
    private float danceTimer;
    public float changeDanceInterval;
    [Space]
    public Hands chosenHands = Hands.PumpingDown;
    private float handsTimer;
    public float changeHandsInterval;
    public AnimationCurve pointedDance;
    public AnimationCurve posedDance;


    void Start()
    {
        animator = GetComponent<Animator>();
        defaultRightHandPosition = rightHandObj.localPosition;
        defaultRightHandRotation = rightHandObj.localRotation;

        defaultLeftHandPosition = leftHandObj.localPosition;
        defaultLeftHandRotation = leftHandObj.localRotation;

        defaultRightFootPosition = rightFootObj.localPosition;
        defaultLeftFootPosition = leftFootObj.localPosition;
        defaultHeadPosition = lookObj.localPosition;
    }

    public void GrabItem(bool value, bool rightOnly = false)
    {
        holdingItem = value;
        holdingOnlyRight = rightOnly;
        if (holdingItem)
        {
            rightHandObj.DOLocalMove(new Vector3(grabWidth, grabObjectPosition.localPosition.y, grabObjectPosition.localPosition.z), 0.2f);
            rightHandObj.DOLocalRotate(grabObjectPosition.localEulerAngles, 0.2f);
            if (!holdingOnlyRight)
            {
                leftHandObj.DOLocalMove(new Vector3(-grabWidth, grabObjectPosition.localPosition.y, grabObjectPosition.localPosition.z), 0.2f);
                leftHandObj.DOLocalRotate(grabObjectPosition.localEulerAngles, 0.2f);
            }
        }
        else
        {
            AnimateRightToDefaultTransform(true);
            AnimateLeftToDefaultTransform(true);
        }
    }

    public void DrinkItem(bool value)
    {
        drinkingItem = value;
        if (drinkingItem)
        {
            Sequence sequence = DOTween.Sequence();
            rightHandObj.DOLocalMove(drinkObjectHandPosition.localPosition, 0.2f);
            rightHandObj.DOLocalRotate(drinkObjectHandPosition.localRotation.eulerAngles, 0.2f).SetDelay(0.2f);
            headIkActive = true;
            lookObj.DOLocalMove(drinkObjectLookPosition.localPosition, 0.2f).SetDelay(0.2f);
            transform.DOLocalRotate(new Vector3 (drinkTiltBackAngle, 0, 0), 0.2f).SetDelay(0.3f);
            leftHandObj.DOLocalMove(defaultLeftHandPosition, 0.2f);
        }
        else
        {
            headIkActive = false;
            lookObj.DOLocalMove(defaultHeadPosition, 0.2f).SetDelay(0.2f);
            transform.DOLocalRotate(Vector3.zero, 0.2f);
            rightHandObj.DOLocalMove(new Vector3(grabWidth, grabObjectPosition.localPosition.y, grabObjectPosition.localPosition.z), 0.2f);
            rightHandObj.DOLocalRotate(grabObjectPosition.localEulerAngles, 0.2f);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            GrabItem(!holdingItem, holdingOnlyRight);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            DrinkItem(!drinkingItem);
        }

        if (isWalking)
        {
            rightFootObj.localPosition =  new Vector3(defaultRightFootPosition.x, defaultRightFootPosition.y, feetCurve.Evaluate(Time.time * walkAnimSpeed));
            leftFootObj.localPosition = new Vector3(defaultLeftFootPosition.x, defaultLeftFootPosition.y, feetCurve.Evaluate(Time.time * walkAnimSpeed + 1.0f));
            if(!holdingItem && !drinkingItem)
            {
                leftHandObj.localPosition = (new Vector3(defaultLeftHandPosition.x, defaultLeftHandPosition.y, feetCurve.Evaluate(Time.time * walkAnimSpeed)));
                rightHandObj.localPosition = (new Vector3(defaultRightHandPosition.x, defaultRightHandPosition.y, feetCurve.Evaluate(Time.time * walkAnimSpeed + 1.0f)));
            }
            else
            {
                if (drinkingItem)
                {
                    rightHandObj.localPosition = (new Vector3(drinkObjectHandPosition.localPosition.x, drinkObjectHandPosition.localPosition.y + feetCurve.Evaluate(Time.time * walkAnimSpeed + 0.5f) * 0.2f, drinkObjectHandPosition.localPosition.z));
                    leftHandObj.localPosition = (new Vector3(defaultLeftHandPosition.x, defaultLeftHandPosition.y, feetCurve.Evaluate(Time.time * walkAnimSpeed)));
                }
                else if (holdingItem)
                {
                    rightHandObj.localPosition = (new Vector3(grabWidth, grabObjectPosition.localPosition.y + feetCurve.Evaluate(Time.time * walkAnimSpeed + 0.5f) * 0.2f, grabObjectPosition.localPosition.z));
                    if (holdingOnlyRight)
                    {
                        leftHandObj.localPosition = (new Vector3(defaultLeftHandPosition.x, defaultLeftHandPosition.y, feetCurve.Evaluate(Time.time * walkAnimSpeed)));
                    }
                    else
                    {
                        leftHandObj.localPosition = (new Vector3(-grabWidth, grabObjectPosition.localPosition.y + feetCurve.Evaluate(Time.time * walkAnimSpeed + 0.5f) * 0.2f, grabObjectPosition.localPosition.z));
                    }


                }
            }

        }

        if(isDancing)
        {
            danceTimer += Time.deltaTime;
            handsTimer += Time.deltaTime;
            
            if(danceTimer > changeDanceInterval)
            {
                danceTimer = 0;
                chosenDance = ChooseRandomDance();
            }

            if (handsTimer > changeHandsInterval)
            {
                handsTimer = 0;
                chosenHands = ChooseRandomHands();
            }

            float danceMoveSpeed = 1f;
            //FEET
            switch (chosenDance)
            {
                case Dances.Shuffle:
                {
                        danceMoveSpeed = 1f;
                        rightFootObj.localPosition = new Vector3(defaultRightFootPosition.x, defaultRightFootPosition.y, pointedDance.Evaluate(Time.time * danceAnimSpeed * danceMoveSpeed) * 0.2f);
                        leftFootObj.localPosition = new Vector3(defaultLeftFootPosition.x, defaultLeftFootPosition.y, pointedDance.Evaluate(Time.time * danceAnimSpeed * danceMoveSpeed + 1.0f) * 0.2f);

                        transform.localPosition = new Vector3(0, 0 - ((pointedDance.Evaluate(Time.time * danceAnimSpeed) + 1f) * 0.5f) * 0.05f, 0);

                        break;
                }
                case Dances.Jumping:
                {
                    danceMoveSpeed = 0.5f;
                    rightFootObj.localPosition = new Vector3(defaultRightFootPosition.x, defaultRightFootPosition.y + pointedDance.Evaluate(Time.time * danceAnimSpeed + 1f) * 0.2f, defaultRightFootPosition.z);
                    leftFootObj.localPosition = new Vector3(defaultLeftFootPosition.x, defaultLeftFootPosition.y + pointedDance.Evaluate(Time.time * danceAnimSpeed + 1f) * 0.2f, defaultRightFootPosition.z);
                    transform.localPosition = new Vector3(0, 0 + ((pointedDance.Evaluate(Time.time * danceAnimSpeed) + 0.8f) * 0.5f) * 0.2f, 0);

                    break;
                }
                case Dances.JumpKickLeft:
                {
                    danceMoveSpeed = 1f;
                    rightFootObj.localPosition = new Vector3(defaultRightFootPosition.x, defaultRightFootPosition.y + pointedDance.Evaluate(Time.time * danceAnimSpeed + 1f) * 0.2f, defaultRightFootPosition.z);
                    leftFootObj.localPosition = new Vector3(defaultLeftFootPosition.x - ((pointedDance.Evaluate(Time.time * danceAnimSpeed * 2f) + 0.75f) * 0.5f * 0.4f), defaultLeftFootPosition.y + pointedDance.Evaluate(Time.time * danceAnimSpeed) * 0.05f + 0.3f + ((pointedDance.Evaluate(Time.time * danceAnimSpeed) + 0.8f) * 0.5f) * 0.2f, defaultLeftFootPosition.z + 0.3f);
                    transform.localPosition = new Vector3(0, 0 + ((pointedDance.Evaluate(Time.time * danceAnimSpeed) + 0.8f) * 0.5f) * 0.2f, 0);

                    break;
                }
                case Dances.JumpKickRight:
                    {
                        danceMoveSpeed = 1f;
                        leftFootObj.localPosition = new Vector3(defaultLeftFootPosition.x, defaultLeftFootPosition.y + pointedDance.Evaluate(Time.time * danceAnimSpeed + 1f) * 0.2f, defaultLeftFootPosition.z);
                        rightFootObj.localPosition = new Vector3(defaultRightFootPosition.x - ((pointedDance.Evaluate(Time.time * danceAnimSpeed * 2f) + 0.75f) * 0.5f * 0.4f), defaultRightFootPosition.y + pointedDance.Evaluate(Time.time * danceAnimSpeed) * 0.05f + 0.3f + ((pointedDance.Evaluate(Time.time * danceAnimSpeed) + 0.8f) * 0.5f) * 0.2f, defaultRightFootPosition.z - 0.3f);
                        transform.localPosition = new Vector3(0, 0 + ((pointedDance.Evaluate(Time.time * danceAnimSpeed) + 0.8f) * 0.5f) * 0.2f, 0);

                        break;
                    }
                case Dances.Polka:
                    {
                        danceMoveSpeed = 1f;
                        leftFootObj.localPosition = new Vector3(defaultLeftFootPosition.x, defaultLeftFootPosition.y + pointedDance.Evaluate(Time.time * danceAnimSpeed * 2f + 1f) * 0.1f + 0.1f, defaultLeftFootPosition.z + 1 * (pointedDance.Evaluate(Time.time * danceAnimSpeed) + 1f) * 0.5f * 0.5f);
                        rightFootObj.localPosition = new Vector3(defaultRightFootPosition.x, defaultRightFootPosition.y + pointedDance.Evaluate(Time.time * danceAnimSpeed * 2f) * 0.1f + 0.1f, defaultRightFootPosition.z + 1 * (pointedDance.Evaluate(Time.time * danceAnimSpeed + 1f) + 1f) * 0.5f * 0.5f);
                        transform.localPosition = new Vector3(0, 0 - ((pointedDance.Evaluate(Time.time * danceAnimSpeed) + 1f) * 0.5f) * 0.05f, 0);

                        break;
                    }
            }
            float handsMoveSpeed = 1f;
            //UpperBody
            switch (chosenHands)
            {
                case Hands.PumpingDown:
                {
                    headIkActive = true;
                    lookObj.localPosition = new Vector3(defaultHeadPosition.x, defaultHeadPosition.y + pointedDance.Evaluate(Time.time * danceAnimSpeed) * 0.5f - 0.8f, defaultHeadPosition.z);
                    handsMoveSpeed = 2f;
                    //Body
                    transform.localEulerAngles = new Vector3(Mathf.LerpUnclamped(-1f, 3f, pointedDance.Evaluate(Time.time * danceAnimSpeed * handsMoveSpeed * danceMoveSpeed)), 0, 0);

                    //Pumping
                    rightHandObj.localPosition = (new Vector3(grabWidth, grabObjectPosition.localPosition.y + pointedDance.Evaluate(Time.time * danceAnimSpeed * handsMoveSpeed * danceMoveSpeed + 1) * 0.3f - 0.15f, grabObjectPosition.localPosition.z));
                    leftHandObj.localPosition = (new Vector3(-grabWidth, grabObjectPosition.localPosition.y + pointedDance.Evaluate(Time.time * danceAnimSpeed * handsMoveSpeed * danceMoveSpeed + 1) * 0.3f - 0.15f, grabObjectPosition.localPosition.z));
                    rightHandObj.localRotation = grabObjectPosition.localRotation;
                    leftHandObj.localRotation = grabObjectPosition.localRotation;
                    break;
                }
                case Hands.PumpingUp:
                {
                    handsMoveSpeed = 2f;
                    //Body
                    transform.localEulerAngles = new Vector3(Mathf.LerpUnclamped(3f, -1f, pointedDance.Evaluate(Time.time * danceAnimSpeed * handsMoveSpeed * danceMoveSpeed)), 0, 0);
                    headIkActive = true;
                    lookObj.localPosition = new Vector3(defaultHeadPosition.x, defaultHeadPosition.y + ((pointedDance.Evaluate(Time.time * danceAnimSpeed) + 0.8f) * 0.5f) * 0.4f + 1f, defaultHeadPosition.z);
                    //Pumping
                    rightHandObj.localPosition = (new Vector3(defaultRightHandPosition.x, defaultRightHandPosition.y + pointedDance.Evaluate(Time.time * danceAnimSpeed * handsMoveSpeed * danceMoveSpeed + 1) * 0.1f + 0.8f, defaultRightHandPosition.z + pointedDance.Evaluate(Time.time * danceAnimSpeed * handsMoveSpeed + 1) * 0.1f + 0.3f));
                    leftHandObj.localPosition = (new Vector3(defaultLeftHandPosition.x, defaultLeftHandPosition.y + pointedDance.Evaluate(Time.time * danceAnimSpeed * handsMoveSpeed * danceMoveSpeed  + 1) * 0.1f + 0.8f, defaultLeftHandPosition.z + pointedDance.Evaluate(Time.time * danceAnimSpeed * handsMoveSpeed + 1) * 0.1f + 0.3f));
                    rightHandObj.localRotation = grabObjectPosition.localRotation;
                    leftHandObj.localRotation = grabObjectPosition.localRotation;
                    break;
                }
                case Hands.SidePump:
                {
                    handsMoveSpeed = 2f;
                    //Body
                    transform.localEulerAngles = new Vector3(Mathf.LerpUnclamped(3f, -1f, pointedDance.Evaluate(Time.time * danceAnimSpeed * handsMoveSpeed * danceMoveSpeed)), 0, 0);
                    headIkActive = true;
                    lookObj.localPosition = new Vector3(defaultHeadPosition.x + pointedDance.Evaluate(Time.time * danceAnimSpeed) * 0.6f, defaultHeadPosition.y + ((pointedDance.Evaluate(Time.time * danceAnimSpeed) + 0.8f) * 0.5f) * 0.2f + 0.3f, defaultHeadPosition.z);
                    //Pumping
                    rightHandObj.localPosition = (new Vector3(defaultRightHandPosition.x + (pointedDance.Evaluate(Time.time * danceAnimSpeed * handsMoveSpeed * danceMoveSpeed + 1f) + 1f) * 0.5f * 0.5f, defaultRightHandPosition.y + pointedDance.Evaluate(Time.time * danceAnimSpeed * handsMoveSpeed * danceMoveSpeed + 1f) * 0.1f + 0.8f, defaultRightHandPosition.z + (pointedDance.Evaluate(Time.time * danceAnimSpeed * handsMoveSpeed + 1)+1) * 0.5f * 0.1f + 0.1f));
                    leftHandObj.localPosition = (new Vector3(defaultLeftHandPosition.x - (pointedDance.Evaluate(Time.time * danceAnimSpeed * handsMoveSpeed * danceMoveSpeed)+1f) * 0.5f * 0.5f, defaultLeftHandPosition.y + pointedDance.Evaluate(Time.time * danceAnimSpeed * handsMoveSpeed * danceMoveSpeed) * 0.1f + 0.8f, defaultLeftHandPosition.z + (pointedDance.Evaluate(Time.time * danceAnimSpeed * handsMoveSpeed) + 1) * 0.5f * 0.1f + 0.1f));
                    rightHandObj.localRotation = defaultRightHandRotation;
                    leftHandObj.localRotation = defaultLeftHandRotation;
                        break;
                }
                case Hands.Point:
                {
                    handsMoveSpeed = 0.5f;
                    //Body
                    transform.localEulerAngles = new Vector3(Mathf.LerpUnclamped(3f, -1f, pointedDance.Evaluate(Time.time * danceAnimSpeed * handsMoveSpeed * danceMoveSpeed)), 0, 0);
                    headIkActive = true;
                    lookObj.localPosition = new Vector3(defaultHeadPosition.x + pointedDance.Evaluate(Time.time * danceAnimSpeed * handsMoveSpeed * danceMoveSpeed) * 0.3f, defaultHeadPosition.y + ((pointedDance.Evaluate(Time.time * danceAnimSpeed) + 0.8f) * 0.5f) * 0.4f + 1f, defaultHeadPosition.z);
                    //Pumping
                    rightHandObj.localPosition = (new Vector3(grabWidth, grabObjectPosition.localPosition.y + 0.2f, grabObjectPosition.localPosition.z + pointedDance.Evaluate(Time.time * danceAnimSpeed * handsMoveSpeed * danceMoveSpeed) * 0.3f + 0.1f));
                    leftHandObj.localPosition = (new Vector3(-grabWidth, grabObjectPosition.localPosition.y + 0.2f, grabObjectPosition.localPosition.z + pointedDance.Evaluate(Time.time * danceAnimSpeed * handsMoveSpeed * danceMoveSpeed + 1) * 0.3f + 0.1f));
                        rightHandObj.localRotation = defaultRightHandRotation;
                    leftHandObj.localRotation = defaultLeftHandRotation;
                    break;
                }
            }
        }
    }

    public enum Dances
    {
        Shuffle,
        Jumping,
        JumpKickLeft,
        JumpKickRight,
        Polka,
    }

    public Dances ChooseRandomDance()
    {
        return (Dances)UnityEngine.Random.Range(0, 5);
    }

    public Hands ChooseRandomHands()
    {
        return (Hands)UnityEngine.Random.Range(0, 4);
    }

    public enum Hands
    {
        PumpingDown,
        PumpingUp,
        SidePump,
        Point,
    }

    void AnimateRightToDefaultTransform(bool tween)
    {
        if(!tween)
        {
            rightHandObj.localPosition = (defaultRightHandPosition);
            rightHandObj.localRotation = defaultRightHandRotation;
        }
        else
        {
            rightHandObj.DOLocalMove(defaultRightHandPosition, 0.2f);
            rightHandObj.DOLocalRotate(defaultRightHandRotation.eulerAngles, 0.2f);
        }

    }

    void AnimateLeftToDefaultTransform(bool tween)
    {
        if(!tween)
        {
            leftHandObj.localPosition = (defaultLeftHandPosition);
            leftHandObj.localRotation = defaultLeftHandRotation;
        }
        else
        {
            leftHandObj.DOLocalMove(defaultLeftHandPosition, 0.2f);
            leftHandObj.DOLocalRotate(defaultLeftHandRotation.eulerAngles, 0.2f);

        }
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (animator)
        {

            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {

                // Set the right hand target position and rotation, if one has been assigned
                if (rightHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                }
                // Set the right hand target position and rotation, if one has been assigned
                if (leftHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
                }
                // Set the right hand target position and rotation, if one has been assigned
                if (rightFootObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
                    animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootObj.rotation);
                }
                // Set the right hand target position and rotation, if one has been assigned
                if (rightHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootObj.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootObj.rotation);
                }


            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            }

            if(headIkActive)
            {
                // Set the look target position, if one has been assigned
                if (lookObj != null)
                {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(lookObj.position);
                }

            }
            else
            {

                animator.SetLookAtWeight(0);
            }
        }
    }

    IEnumerator TweenRightHand(Vector3 startPosition, Vector3 endPosition, float time)
    {
        if (time != 0)
        {

            float alpha = 0;

            while (alpha < 1.0f)
            {
                alpha += Time.deltaTime / time;

                rightHandObj.localPosition = Vector3.Lerp(startPosition, endPosition, easeIn.Evaluate(alpha));
                yield return null;
            }

        }
    }

    IEnumerator PositionTweener(Transform targetTransform, Vector3 startPosition, Vector3 endPosition, float time)
    {
        if (time != 0)
        {

            float alpha = 0;

            while (alpha < 1.0f)
            {
                alpha += Time.deltaTime / time;

                targetTransform.localPosition = Vector3.Lerp(startPosition, endPosition, easeIn.Evaluate(alpha));
                yield return null;
            }

        }
    }
}