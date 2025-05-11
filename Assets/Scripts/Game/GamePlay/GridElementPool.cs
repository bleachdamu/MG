using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GridElementPool : MonoBehaviour
{
    private IObjectPool<GridElement> m_Pool;
    private GridElement toCreateGameObject;

    public static GridElementPool Instance;

    public IObjectPool<GridElement> Pool { get => m_Pool; set => m_Pool = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
    }

    public void Initialize(GridElement toCreateGameObject)
    {
        if (m_Pool == null)
        {
            this.toCreateGameObject = toCreateGameObject;
            m_Pool = new ObjectPool<GridElement>(CreatePooledObject, OnGetFromPool, OnReleaseToPool, collectionCheck:true, defaultCapacity:10);
        }
    }

    private void OnReleaseToPool(GridElement poolObject)
    {
        poolObject.transform.SetParent(transform);
        poolObject.OnReleaseToPool();
    }

    private void OnGetFromPool(GridElement poolObject)
    {
        poolObject.OnGetFromPool();
    }

    private GridElement CreatePooledObject()
    {
        return Instantiate(toCreateGameObject,transform);
    }
}
