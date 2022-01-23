using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PassiveSkillBar : MonoBehaviour
{ 
    [SerializeField] private List<UI_SkillBarIcon> _skillBarIcons;

    private void Start()
    {
        RefreshSkillBarIcons();
    }

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Skill)
        {
            RefreshSkillBarIcons();
        }
    }

    private void RefreshSkillBarIcons()
    {
        // var skillIndexList = PlayerSkillManager.Instance.EquippedPassiveSkillIndex;
        //
        // if (skillIndexList.Count > _skillBarIcons.Count)
        // {
        //     Debug.LogError("스킬 슬롯 갯수가 모자랍니다");
        // }
        //
        // for (var i = 0; i < skillIndexList.Count; i++)
        // {
        //     var skill =  PlayerSkillManager.Instance.GetPassiveSkill(skillIndexList[i]);
        //     
        //     _skillBarIcons[i].InitSkill(skill);
        // }
    }
    
}
