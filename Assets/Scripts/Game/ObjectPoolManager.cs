using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    private IObjectPool<GridElement> m_Pool;
    private GridElement toCreateGameObject;

    public static ObjectPoolManager Instance;

    public IObjectPool<GridElement> Pool { get => m_Pool; set => m_Pool = value; }

    private void Start()
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
            m_Pool = new ObjectPool<GridElement>(CreatePooledObject, OnGetFromPool, OnReleaseToPool, DestroyPooledObject, false, 10);
        }
    }

    private void DestroyPooledObject(GridElement poolObject)
    {
        Debug.Log("destroyed from pool");
    }

    private void OnReleaseToPool(GridElement poolObject)
    {
        poolObject.OnReleaseToPool();
    }

    private void OnGetFromPool(GridElement poolObject)
    {
        poolObject.OnGetFromPool();
    }

    private GridElement CreatePooledObject()
    {
        Debug.Log("new object created");
        return Instantiate(toCreateGameObject,transform);
    }
}
