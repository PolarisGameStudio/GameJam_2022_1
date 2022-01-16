using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TouchBounce : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform m_RectTransform;

    public float m_Sacle = 0.96f;

    public bool Run = true;

    public Button m_Button;

    public bool Play = false;
    
    private void Awake()
    {
        if (!m_RectTransform)
        {
            m_RectTransform = GetComponent<RectTransform>();
        }
    }

    private void OnDisable()
    {
        Play = false;
        
        m_RectTransform.localScale = Vector3.one;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Run) return;

        if (m_Button && !m_Button.interactable) return;

        Play = true;

        m_RectTransform.DOScale(new Vector3(m_Sacle, m_Sacle, 1f), 0.1f).SetUpdate(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (m_Button && !m_Button.interactable) return;

        m_RectTransform.DOScale(new Vector3(1f, 1f, 1f), 0.08f).SetUpdate(true).OnComplete(() =>
        {
            Play = false;
        });
    }
}