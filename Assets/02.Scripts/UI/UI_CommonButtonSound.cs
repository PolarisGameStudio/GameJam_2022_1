using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
public class UI_CommonButtonSound : GameBehaviour
{
    [SerializeField] Button m_Button;
    [SerializeField] private Toggle m_Toggle;
    void Start()
    {
        m_Button = GetComponent<Button>();

        if (m_Button != null)
        {
            m_Button.OnClickAsObservable().Subscribe(OnClickButton).AddTo(this);
        }

        m_Toggle = GetComponent<Toggle>();


        if (m_Toggle != null)
        {
            m_Toggle.OnValueChangedAsObservable().Subscribe(OnClickToggle).AddTo(this);
        }
    }

    void OnClickButton(Unit u)
    {
        SoundManager.Instance.PlaySound("ui_common_button");
    }
    
    
    void OnClickToggle(bool isOn)
    {
        if (isOn)
        {
            SoundManager.Instance.PlaySound("ui_common_button"); 
        }
    }
}
