using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup_Skill_Equip : UI_BasePopup<UI_Popup_Skill_Equip>
{
    public List<UI_Skill_Slot> Slots;
    private TBL_SKILL _data;

    public void Open(TBL_SKILL data)
    {
        base.Open();
        _data = data;

        Refresh();
    }

    protected override void Refresh()
    {
        for (int i = 0; i < DataManager.SkillData.EquippedIndex.Count; i++)
        {
            var index = DataManager.SkillData.EquippedIndex[i];

            if (index < 0 || index >= TBL_SKILL.CountEntities)
            {
                Slots[i].Init(null);
                continue;
            }

            Slots[i].Init(TBL_SKILL.GetEntity(index));
        }
    }

    public void OnClickSelectSlot(int index)
    {
        DataManager.SkillData.TryEquip(_data.Index, index);
        UI_Popup_Skill.Instance.Close();
        Close();
    }
}