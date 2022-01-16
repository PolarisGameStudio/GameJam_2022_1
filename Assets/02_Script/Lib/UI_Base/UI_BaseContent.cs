using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UI_BaseContent<T, S> : SingletonBehaviour<T> where T :  GameBehaviour where S : GameBehaviour
{
    protected bool m_Open = false;
    public bool IsOpen => m_Open;
    
    [Header("슬롯 부모")]
    [SerializeField] 
    protected Transform m_SlotParentTransform;
    
    [Header("슬롯 프리팹")]
    [SerializeField] 
    protected S m_SlotPrefab;

    [Header("슬롯 목록")]
    [SerializeField]
    protected List<S> m_SlotList = new List<S>();
    
    protected CanvasGroup m_CanvasGroup;

    protected bool m_Dirty = true;
    
    
    protected override void Awake()
    {
        base.Awake();

        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_CanvasGroup.alpha = 1;

        var slotList = m_SlotParentTransform.GetComponentsInChildren<S>();
        foreach (var slot in slotList)
        {
            m_SlotList.Add(slot);
        }
    }

    public virtual void Open()
    {
        SafeSetActive(true);

        m_Open = true;
        
        Refresh();
    }

    public virtual void Close()
    {
        SafeSetActive(false);
    }
    
    protected virtual void OnEnable()
    {
        m_Open = true;
        
        m_CanvasGroup.alpha = 1;
        
        Enable();
    }

    protected virtual void Enable()
    {
        
    }
    
    protected virtual void OnDisable()
    {
        m_Open = false;
        
        Disable();
    }

    protected virtual void Disable()
    {
        
    }
    
    protected virtual void Expand(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            S slot = Instantiate(m_SlotPrefab, m_SlotParentTransform);
            
            m_SlotList.Add(slot);
        }
    }

    protected bool IsEnableRefresh()
    {
        if (!m_Open)
        {
            return false;
        }

        if (!m_Dirty)
        {
            return false;
        }

        m_Dirty = false;

        return true;
    }
    
    protected abstract void Refresh();
}