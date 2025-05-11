using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable
{
    void OnPointerEnter();
    void OnPointerExit();
    void OnClick();
}
