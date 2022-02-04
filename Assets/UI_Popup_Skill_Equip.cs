using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Skill_Equip : UI_BasePopup<UI_Popup_Skill_Equip>
{
    public List<UI_Skill_Slot> Slots;
    public List<Text> ConditionTexts;
    public List<Image> LockImage;
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
            bool isLock = !DataManager.SkillData.IsSlotUnlock(i);

            ConditionTexts[i].enabled = isLock;
            ConditionTexts[i].text = $"레벨 {DataManager.SkillData.GetUnlockCondition(i)} 개방";

            LockImage[i].gameObject.SetActive(isLock);
            
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
        if (!DataManager.SkillData.IsSlotUnlock(index))
        {
            return;
        }
        
        DataManager.SkillData.TryEquip(_data.Index, index);
        SoundManager.Instance.PlaySound("sfx_slot_equip");
        UI_Popup_Skill.Instance.Close();
        Close();
    }
}