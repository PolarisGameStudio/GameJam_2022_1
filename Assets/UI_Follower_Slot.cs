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
        }
        else
        {
            _txtLevel.text = $"{level}등급";
        }

        _sliderGauge.gameObject.SetActive(true);

        var cost = DataManager.FollowerData.GetLevelUpCost(_data.Index);
        _sliderGauge.minValue = 0;
        _sliderGauge.maxValue = cost;
        _sliderGauge.value = DataManager.FollowerData.Counts[_data.Index];

        _txtAmount.text = $"{DataManager.FollowerData.Counts[_data.Index]}/{cost}";
    }

    public void OnClickSlot()
    {
    }
}