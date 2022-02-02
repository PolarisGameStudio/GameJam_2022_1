using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Review : UI_BasePopup<UI_Popup_Review>
{
    public List<Toggle> Stars;

    private int _selectedNumber;

    public void OnClickStar(int number)
    {
        _selectedNumber = number;

        for (int i = 0; i < Stars.Count; i++)
        {
            Stars[i].isOn = number > i;
        }
    }

    private void Start()
    {
        OnClickStar(5);
    }

    public void SendReview()
    {
        Close();
        ServerManager.Instance.SendCreative(_selectedNumber);
    }

    protected override void Refresh()
    {
        
    }
}
