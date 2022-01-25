using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_OK : UI_BasePopup<UI_Popup_OK>
{
    protected Action m_OKAction;
    protected Action m_CancelAction;
    
    [Header("타이틀")] public Text m_Title;
    [Header("디스크립션")] public Text m_Description;
    
    [Header("확인 버튼")] public Button m_OKButton;
    [Header("취소 버튼")] public Button m_CancelButton;
    

    public void Open(string title, string description, Action okAction = null, Action cancelAction = null)
    {
        Open();
        
        m_Title.text = title;
        m_Description.text = description;
        
        m_OKAction = okAction;
        m_CancelAction = cancelAction;
        
        m_OKButton.gameObject.SetActive(true);
        m_CancelButton.gameObject.SetActive(cancelAction != null);
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
