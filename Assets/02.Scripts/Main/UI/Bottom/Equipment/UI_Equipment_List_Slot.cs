using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Equipment_List_Slot : UI_BaseSlot<TBL_EQUIPMENT>
{
    [SerializeField] private Image _imgFrame;
    [SerializeField] private Image _imgIcon;
    
    [SerializeField] private Text _txtLevel;
    [SerializeField] private Text _txtGrade;
    
    [SerializeField] private Slider _sliderGauge;
    [SerializeField] private Text _txtAmount;
    
    public override void Init(TBL_EQUIPMENT data)
    {
        Refresh();
    }

    private void Refresh()
    {
        _imgFrame.sprite = null;
        _imgIcon.sprite = null;
        
        
    }
}
