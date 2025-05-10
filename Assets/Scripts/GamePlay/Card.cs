using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : GridElement, IClickable
{
    [SerializeField]
    private Vector3 defaultRotation;
    [SerializeField]
    private AnimationCurve rotationCurve;
    [SerializeField]
    private float flipDuration = 0.5f;

    private bool frontFace = true;
    private Coroutine flipCoroutine = null;


    public void OnClick()
    {
        Debug.Log("Card clicked: " + gameObject.name);
        FlipCard();
    }

    public void OnPointerEnter()
    {
        Debug.Log("Pointer entered card: " + gameObject.name);
    }

    public void OnPointerExit()
    {
        Debug.Log("Pointer exited card: " + gameObject.name);
    }

    private void FlipCard()
    {
        if (flipCoroutine == null)
        {
            flipCoroutine = StartCoroutine(Flip(!frontFace, flipDuration));
        }
    }

    private IEnumerator Flip(bool frontFace, float totalDuration)
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(0, frontFace ? 0 : 180f, 0);
        float timeTaken = 0f;

        while (timeTaken < totalDuration)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, rotationCurve.Evaluate(timeTaken / totalDuration));
            timeTaken += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endRot;
        this.frontFace = frontFace;
        flipCoroutine = null;
    }
}
