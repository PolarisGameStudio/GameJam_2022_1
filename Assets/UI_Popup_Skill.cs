using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Skill : UI_BasePopup<UI_Popup_Skill>, GameEventListener<RefreshEvent>
{
    public UI_Skill_Slot SelectedSkillSlot;
    
    public Text _txtName;
    public Text _txtDescription;
    
    public Text _txtCoolTime;
    public Text _txtDetailDesc;

    public Text _txtPrice;

    public Button _btnLevelUp;
    public Button _btnEquip;

    private TBL_SKILL _data;

    protected void OnEnable()
    {
        this.AddGameEventListening<RefreshEvent>();
    }

    protected void OnDisable()
    {
        this.RemoveGameEventListening<RefreshEvent>();
    }

    public void Open(TBL_SKILL data)
    {
        _data = data;
        base.Open();
    }

    protected override void Refresh()
    {
        SelectedSkillSlot.Init(_data);
        
        _txtName.text = $"[{StringValue.GetGradeName(_data.ItemGrade)}] {_data.name}";
//        _txtDescription.text = $"{_data.name} 간단한 설명";
        _txtDescription.text = "";

        _txtCoolTime.text = $"재사용 대기 시간 : {_data.CoolTime}";
        _txtDetailDesc.text = PlayerSkillManager.Instance.GetDescription(_data.Index);
        
        
        _txtPrice.text = $"({DataManager.SkillData.Counts[_data.Index]}/{DataManager.SkillData.GetLevelUpCost(_data.Index)})";

        var enable = DataManager.SkillData.IsEnableLevelUp(_data.Index);
        _txtPrice.color = enable ? ColorValue.ENABLE_TEXT_COLOR :ColorValue.DISABLE_TEXT_COLOR;
        _btnLevelUp.interactable = enable;
        _btnEquip.interactable = DataManager.SkillData.Levels[_data.Index] > 0;
    }

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Skill)
        {
            Refresh();
        }
    }

    public void OnClickLevelUp()
    {
        if (DataManager.SkillData.TryLevelUp(_data.Index))
        {
            Refresh();
            SoundManager.Instance.PlaySound("ui_levelup_button2");
        }
    }

    public void OnClickTryEquip()
    {
        if (DataManager.SkillData.Levels[_data.Index] <= 0)
        {
            return;
        }

        UI_Popup_Skill_Equip.Instance.Open(_data);
    }
}