using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Skill_Slot : UI_BaseSlot<TBL_SKILL>
{
    [SerializeField] private Image _imgFrame;
    [SerializeField] private Image _imgIcon;
    [SerializeField] private Image _imgNone;

    [SerializeField] private Text _txtLevel;

    [SerializeField] private Slider _sliderGauge;
    [SerializeField] private Text _txtAmount;

    public bool IgnoreSlider;

    public override void Init(TBL_SKILL data)
    {
        _data = data;
        Refresh();
    }

    private void Refresh()
    {
        if (_data == null)
        {
            _imgIcon.enabled = false;
            _imgFrame.enabled = AssetManager.Instance.ItemFrameIcon[0];
            _txtLevel.text = "";
            _txtAmount.text = "";
            _sliderGauge.gameObject.SetActive(false);
            return;
        }

        //_imgIcon.enabled = true;
        _imgIcon.sprite = AssetManager.Instance.SkillIcon[_data.Index];
        _imgFrame.sprite = AssetManager.Instance.ItemFrameIcon[(int)_data.ItemGrade];

        var level = DataManager.SkillData.Levels[_data.Index];

        if (level == 0)
        {
            _txtLevel.text = $"";
            _imgIcon.color = Color.black;
            _imgIcon.enabled = false;
            _imgNone.enabled = true;
        }
        else
        {
            _txtLevel.text = $"{level}등급";
            _imgIcon.color = Color.white;
            _imgIcon.enabled = true;
            _imgNone.enabled = false;
        }

        if(!IgnoreSlider)
        {
            _sliderGauge.gameObject.SetActive(true);

            var cost = DataManager.SkillData.GetLevelUpCost(_data.Index);
            _sliderGauge.minValue = 0;
            _sliderGauge.maxValue = cost;
            _sliderGauge.value = DataManager.SkillData.Counts[_data.Index];

            _txtAmount.text = $"{DataManager.SkillData.Counts[_data.Index]}/{cost}";
        }
    }

    public void OnClickSlot()
    {
       UI_Popup_Skill.Instance.Open(_data);
    }
}