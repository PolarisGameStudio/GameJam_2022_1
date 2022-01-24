using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Buy : UI_BasePopup<UI_Popup_Buy>
{
    protected Action m_OKAction;
    protected Action m_CancelAction;
    
    [Header("타이틀")] public Text m_Title;
    [Header("디스크립션")] public Text m_Description;

    [Header("재화 아이콘")] public Image m_CurrencyIcon;
    [Header("비용")] public Text m_Cost;
    
    [Header("확인 버튼")] public Button m_OKButton;
    [Header("취소 버튼")] public Button m_CancelButton;
    

    public void Open(string title, string description, Enum_CurrencyType currencyType, int cost, Action okAction = null, Action cancelAction = null)
    {
        Open();
        
        m_Title.text = title;
        m_Description.text = description;
        
        m_OKAction = okAction;
        m_CancelAction = cancelAction;
        
        m_OKButton.gameObject.SetActive(true);
        m_CancelButton.gameObject.SetActive(true);

        m_Cost.text = cost.ToString();
        m_CurrencyIcon.sprite = AssetManager.Instance.CurrencyIcon[(int)currencyType];
    }

    public void OnClickOk()
    {
        m_OKAction?.Invoke();

        m_OKAction = null;
        m_CancelAction = null;
        
        Close();
    }

    public void OnClickCancel()
    {
        m_CancelAction?.Invoke();

        m_OKAction = null;
        m_CancelAction = null;
        
        Close();
    }

    protected override void Refresh()
    {
        
    }
}