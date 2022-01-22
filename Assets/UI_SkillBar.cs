using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillBar : MonoBehaviour, GameEventListener<RefreshEvent>
{
    public List<UI_SkillBarIcon> SkillSlots;

    void Start()
    {
        this.AddGameEventListening<RefreshEvent>();
        Refresh();
    }

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Skill)
        {
            Refresh();
        }
    }

    private void Refresh()
    {
        var equipped = DataManager.SkillData.EquippedIndex;
        for (var i = 0; i < equipped.Count; i++)
        {
            if (equipped[i] < 0 || equipped[i] >= TBL_SKILL.CountEntities)
            {
                SkillSlots[i].InitSkill(null, DataManager.SkillData.IsSlotUnlock(i));
                continue;
            }

            SkillSlots[i].InitSkill(TBL_SKILL.GetEntity(equipped[i]), DataManager.SkillData.IsSlotUnlock(i));
        }
    }
}