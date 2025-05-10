using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEventHandler : MonoBehaviour
{
    private IClickable clickable;
    private void Update()
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
