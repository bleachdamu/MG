using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class InputEventHandler : MonoBehaviour
{
    private IClickable clickable;
    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        MouseEvents();
#elif UNITY_ANDROID || UNITY_IOS
        TouchEvents();
#endif
    }

    private void TouchEvents()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                if (hit.collider != null)
                {
                    if (clickable == null)
                    {
                        clickable = hit.transform.gameObject.GetComponent<IClickable>();
                        if (clickable != null)
                        {
                            clickable.OnClick();
                            clickable = null;
                        }
                    }
                }

            }
        }
    }

    private void MouseEvents()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

        if (hit.collider != null)
        {
            if (clickable == null)
            {
                clickable = hit.transform.gameObject.GetComponent<IClickable>();
                if (clickable != null)
                {
                    clickable.OnPointerEnter();
                }
            }
        }
        else
        {
            if (clickable != null)
            {
                clickable.OnPointerExit();
                clickable = null;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (clickable != null)
            {
                clickable.OnClick();
            }
        }
    }
}
