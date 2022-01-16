using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ButtonTouching : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Button m_Button;

    private static float FIRST_SPEED = 0.22f;
    private static float MIN_SPEED = 0.06f;
    private static float ACC_TIME = 0.8f;
    private static float ACC_ = 0.06f;
    
    
    private float m_DownTimeSecond = 0.25f;
    private float m_DownTimer = 0f;

    private float m_AccTimer = 0f;
    
    private bool m_IsDown = false;
    
    private void Awake()
    {
        if (!m_Button)
        {
            m_Button = GetComponent<Button>();
        }
    }

    private void OnDisable()
    {
        //CurrencyManager.Instance.AddGold(33333333, CurrencySource.Battle);
        
        m_DownTimeSecond = FIRST_SPEED;
        m_DownTimer = 0f;
        m_AccTimer = 0f;
        
        m_IsDown = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_DownTimeSecond = FIRST_SPEED;
        m_DownTimer = 0f;
        m_AccTimer = 0f;

        m_IsDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_IsDown = false;
        
        m_DownTimeSecond = FIRST_SPEED;
        m_DownTimer = 0f;
        m_AccTimer = 0f;

    }

    private void Update()
    {
        if (!m_IsDown) return;
        if (!m_Button.interactable) return;


        float u = Time.unscaledDeltaTime;
        m_AccTimer += u;
        m_DownTimer += u;


        if (m_AccTimer >= ACC_TIME)
        {
            m_AccTimer = 0f;
            m_DownTimeSecond = Mathf.Max(MIN_SPEED, m_DownTimeSecond - ACC_);
        }
        if (m_DownTimer >= m_DownTimeSecond)
        {
            m_Button.onClick.Invoke();
            
            // Vector2 position = UICamera.Instance.Camera.ScreenToWorldPoint(Input.mousePosition);
            // TouchEffectFactory.Instance.Show(position);

            m_DownTimer = 0f;
        }
    }
}
