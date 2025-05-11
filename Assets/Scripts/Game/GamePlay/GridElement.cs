using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour, IGridElement, IPoolObject
{
    [SerializeField]
    public Renderer renderer;
    public Vector2 gridPosition;

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

    protected void ReleaseGridElementToPool()
    {
        GridElementPool.Instance.Pool.Release(this);
    }

    public virtual void OnGetFromPool()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnReleaseToPool()
    {
        gameObject.SetActive(false);
    }
}
