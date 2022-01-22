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
        _data = data;
        Refresh();
    }

    private void Refresh()
    {
        _imgFrame.sprite = AssetManager.Instance.ItemFrameIcon[(int)_data.Grade];
        _imgIcon.sprite = AssetManager.Instance.EquipmentIcon[_data.Index];

        var level = DataManager.EquipmentData.Levels[_data.Index];

        if (level == 0)
        {
            _txtGrade.text = $"";
            _imgIcon.color = Color.black;
        }
        else
        {
            _txtGrade.text = $"+{level}";
            _imgIcon.color = Color.white;
        }

        _txtLevel.text = $"{_data.Star + 1} 등급";

        _sliderGauge.value = DataManager.EquipmentData.Counts[_data.Index] / 5f;
        _txtAmount.text = $"{DataManager.EquipmentData.Counts[_data.Index]}/5";
    }

    public void OnClickSlot()
    {
        UI_Popup_Equipment.Instance.Open(_data);
    }
}
