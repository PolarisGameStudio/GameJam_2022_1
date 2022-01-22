using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CanvasGroup))]
public abstract class UI_BasePopup<T> : SingletonBehaviour<T> where T:  GameBehaviour
{
    [Header("UIType")] public UIType UIType;
    
    protected CanvasGroup m_CanvasGroup;

    protected override void Awake()
    {
        base.Awake();

        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_CanvasGroup.alpha = 1f;
        
        SafeSetActive(false);
    }

    protected abstract void Refresh();
    
    public virtual void Open()
    {
        UIManager.Instance.Push(UIType);
        
        SafeSetActive(true);
        
        Refresh();
    }

    public virtual void Close()
    {
        UIManager.Instance.Pop(UIType);
        SafeSetActive(false);
    }
}
