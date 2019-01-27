using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBoxBouncer : MonoBehaviour
{
    public AnimationCurve bounceCurve;

    public float bounceSpeed = 2f;
    public float bounceHeight;

    Vector3 startPosition;
    Vector3 goalPosition;

    Vector3 startEulerAngles;
    public Vector3 goalEulerAngles;

    float originalScale;

    private void Start()
    {
        startPosition = transform.position;
        goalPosition = startPosition + new Vector3(0, bounceHeight, 0);

        startEulerAngles = transform.eulerAngles;

        originalScale = transform.localScale.x;

        StartCoroutine(Bounce());
    }

    IEnumerator Bounce()
    {
        float alpha = 0;

        while(true)
        {
            alpha += Time.deltaTime * bounceSpeed;
            float curve = bounceCurve.Evaluate(alpha);
            transform.localScale = new Vector3(originalScale + curve * 2f, originalScale + curve * 0.25f, originalScale + curve * 0f);
            transform.position = Vector3.Lerp(startPosition, goalPosition, curve);
            //transform.rotation = Quaternion.Euler(Vector3.Lerp(startEulerAngles, goalEulerAngles, curve));
            yield return null;
        }
    }
}
