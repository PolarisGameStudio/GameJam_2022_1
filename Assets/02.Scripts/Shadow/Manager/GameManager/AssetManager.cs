using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class AssetManager : SingletonBehaviour<AssetManager>
{
    [PreviewField] [Header("장비 아이콘")] public List<Sprite> EquipmentIcon;
    [PreviewField] [Header("장비타입 아이콘")] public List<Sprite> EquipmentTypeIcon;
    [PreviewField] [Header("슬롯 아이콘")] public List<Sprite> ItemFrameIcon;
    [PreviewField] [Header("몸종 아이콘")] public List<Sprite> FollowerIcon;
    [PreviewField] [Header("스탯 아이콘")] public List<Sprite> StatIcon;
    [PreviewField] [Header("스킬 아이콘")] public List<Sprite> SkillIcon;
    [PreviewField] [Header("재화 아이콘")] public List<Sprite> CurrencyIcon;
    [PreviewField] [Header("승급 아이콘")] public List<Sprite> PromotionIcon;
    [PreviewField] [Header("몸종 전신")] public List<Sprite> FollowerBodyIcon;

    public Sprite GetSpriteWithRewardType(RewardType type, int value)
    {
        switch (type)
        {
            case RewardType.Currency:
                return CurrencyIcon[value];
            case RewardType.Skill:
                return SkillIcon[value];
            case RewardType.Equipment:
                return EquipmentIcon[value];
            case RewardType.Follower:
                return FollowerIcon[value];
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        return null;
    }

    public Sprite GetFrameWithItemGrade(RewardType type, int value)
    {  
        switch (type)
        {
            case RewardType.Skill:
                return ItemFrameIcon[(int)TBL_SKILL.GetEntity(value).ItemGrade];
            case RewardType.Equipment:
                return ItemFrameIcon[(int)TBL_EQUIPMENT.GetEntity(value).Grade];
            default:
                return ItemFrameIcon[0];
        }
    }
}