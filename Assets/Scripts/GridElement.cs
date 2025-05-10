using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour, IGridElement
{
    [SerializeField]
    public Renderer renderer;

    public Bounds GetBounds()
    {
        return renderer.bounds;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
