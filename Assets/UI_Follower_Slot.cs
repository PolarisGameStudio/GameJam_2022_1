using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Follower_Slot : UI_BaseSlot<TBL_FOLLOWER>
{
    [SerializeField] private Image _imgIcon;

    [SerializeField] private Text _txtLevel;

    [SerializeField] private Slider _sliderGauge;
    [SerializeField] private Text _txtAmount;
    
    [SerializeField] private GameObject _onEquipObject;
    

    public bool IgnoreSlider;

    public override void Init(TBL_FOLLOWER data)
    {
        _data = data;
        Refresh();
    }

    private void Refresh()
    {
        if (_data == null)
        {
            _imgIcon.enabled = false;
            _txtLevel.text = "";
            _txtAmount.text = "";
            _sliderGauge.gameObject.SetActive(false);
            return;
        }

        _imgIcon.enabled = true;
        _imgIcon.sprite = AssetManager.Instance.FollowerIcon[_data.Index];

        var level = DataManager.FollowerData.Levels[_data.Index];

        if (level == 0)
        {
            _txtLevel.text = $"";
            _imgIcon.color = Color.black;
        }
        else
        {
            _txtLevel.text = $"{level}등급";
            _imgIcon.color = Color.white;
        }

        if(!IgnoreSlider)
        {
            _sliderGauge.gameObject.SetActive(true);

            var cost = DataManager.FollowerData.GetLevelUpCost(_data.Index);
            _sliderGauge.minValue = 0;
            _sliderGauge.maxValue = cost;
            _sliderGauge.value = DataManager.FollowerData.Counts[_data.Index];

            _txtAmount.text = $"{DataManager.FollowerData.Counts[_data.Index]}/{cost}";
            
            _onEquipObject.gameObject.SetActive(DataManager.FollowerData.EquippedIndex.Contains(_data.Index));
        }
    }

    public void OnClickSlot()
    {
        UI_Popup_Follower.Instance.Open(_data);
    }
}