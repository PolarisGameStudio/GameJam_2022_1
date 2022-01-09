using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMultiPool<ObjPool, ObjPrefab> : SingletonBehaviour<ObjPool> where ObjPool : GameBehaviour where ObjPrefab : MultiPoolItem
{
    [Header("폴링 시점")] public PoolInitOption m_PoolInitOption = PoolInitOption.Awake;
    [Header("풀링할 오브젝트들")] public List<ObjPrefab> m_ItemsToPool;
    [Header("풀링 확장 여부")] public bool m_PoolExpansion = true;

    protected List<ObjPrefab> m_PooledObjects; // 풀 리스트

    protected bool m_Init = false; // 초기화
    
    protected override void Awake()
    {
        base.Awake();

        int capacity = 0;

        foreach (var item in m_ItemsToPool)
        {
            capacity += item.PoolAmount;
        }
        
        m_PooledObjects = new List<ObjPrefab>(capacity);

        
        if (!m_Init && m_PoolInitOption == PoolInitOption.Awake)
        {
            InitPool();
        }
    }
    
    protected virtual void InitPool()
    {
        if (m_Init) return;
        
        var alreadyExists = GetComponentsInChildren<ObjPrefab>();
        if (alreadyExists.Length > 0)
        {
            foreach (var item in alreadyExists)
            {
                item.SafeSetActive(false);
                
                m_PooledObjects.Add(item);
            }
        }
        
        foreach (var item in m_ItemsToPool)
        {
            int amount = item.PoolAmount;

            for (int i = 0; i < amount; ++i)
            {
                ObjPrefab obj = Instantiate(item, transform).GetComponent<ObjPrefab>();
                obj.SafeSetActive(false);
                
                m_PooledObjects.Add(obj);
            }
        }
        
        m_Init = true;
    }

    protected virtual void Start()
    {
        if (!m_Init && m_PoolInitOption == PoolInitOption.Start)
        {
            InitPool();
        }
    }

    public ObjPrefab GetPooledObject(Func<ObjPrefab, bool> selector)
    {
        if (!m_Init && m_PoolInitOption == PoolInitOption.Lazy)
        {
            InitPool();
        }
        
        foreach (var obj in m_PooledObjects)
        {
            if (!obj.isActiveAndEnabled && selector(obj))
            {
                return obj;
            }
        }

        if (m_PoolExpansion)
        {
            foreach (var item in m_ItemsToPool)
            {
                if (selector(item))
                {
                    var newObj = Instantiate(item, transform).GetComponent<ObjPrefab>();
                    m_PooledObjects.Add(newObj);

                    return newObj;
                }
            }
        }
        
        return null;
    }
    
    public void HideAll()
    {
        foreach (var obj in m_PooledObjects)
        {
            obj.SafeSetActive(false);
        }
    }

}