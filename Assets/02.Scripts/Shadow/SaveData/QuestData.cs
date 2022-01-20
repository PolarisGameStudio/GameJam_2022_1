using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    DamageLevel,
    HealthLevel,
    CriticalChanceLevel,
    CriticalDamageLevel,

    GachaEquipment,
    ClearStage,

    GachaSkill,
    EquipSkill,

    ClearTreasureDungeon,
    EquipmentLevel,

    ADWatch,

    Promotion,

    ClearBossDungeon
}

public class QuestData : SaveDataBase
{
    public int CurrentQuestIndex = 0;

    public TBL_QUEST CurrentQuest => TBL_QUEST.GetEntity(CurrentQuestIndex);

    [SerializeField] private bool _adWatch;
    
    public override void ValidCheck()
    {
        base.ValidCheck();

        CurrentQuestIndex = Mathf.Min(CurrentQuestIndex, TBL_QUEST.CountEntities - 1);
    }

    public int GetProgress()
    {
        switch (CurrentQuest.QuestType)
        {
            case QuestType.DamageLevel:
                return DataManager.GoldGrowthData.GetLevel(TBL_UPGRADE_GOLD.GetEntityByKeyWithStatType(Enum_StatType.Damage));
            
            case QuestType.HealthLevel:
                return DataManager.GoldGrowthData.GetLevel(TBL_UPGRADE_GOLD.GetEntityByKeyWithStatType(Enum_StatType.MaxHealth));
            
            case QuestType.CriticalChanceLevel:
                return DataManager.GoldGrowthData.GetLevel(TBL_UPGRADE_GOLD.GetEntityByKeyWithStatType(Enum_StatType.CriticalChance));
            
            case QuestType.CriticalDamageLevel:
                return DataManager.GoldGrowthData.GetLevel(TBL_UPGRADE_GOLD.GetEntityByKeyWithStatType(Enum_StatType.CriticalDamage));
            
            case QuestType.GachaEquipment:
                return DataManager.GachaData.GetGachaCount(GachaType.Weapon) +
                       DataManager.GachaData.GetGachaCount(GachaType.Ring);
            
            case QuestType.ClearStage:
                return DataManager.StageData.HighestStageLevel;
            
            case QuestType.GachaSkill:
                return DataManager.GachaData.GetGachaCount(GachaType.Skill);
            
            case QuestType.EquipSkill:
                return DataManager.SkillData.EquippedIndex.FindAll(x => x != -1).Count > 0 ? int.MaxValue : 0;
            
            case QuestType.ClearTreasureDungeon:
                return DataManager.DungeonData.TreasureDungeonHighLevel;

            case QuestType.EquipmentLevel:
            {
                int highestLevel = 0;
                DataManager.EquipmentData.EquipmentGroups.ForEach((group) =>
                {
                    group.Levels.ForEach(level =>
                    {
                        if (level > highestLevel)
                        {
                            highestLevel = level;
                        }
                    });
                });
                return highestLevel;
            }
            
            case QuestType.ADWatch:
                return _adWatch ? int.MaxValue : 0;
            
            case QuestType.Promotion:
                return DataManager.PromotionData.CurrentPromotionIndex;
                break;
            
            case QuestType.ClearBossDungeon:
                return DataManager.DungeonData.BossDungeonHighLevel;
                break;
            
            default:
                Debug.LogError($"{CurrentQuest.QuestType}은 퀘스트 안넣음");
                return 0;
        }
    }

    public void OnAdWatch()
    {
        _adWatch = true;
    }

    public bool IsEnableClear()
    {
        if (CurrentQuest.QuestType == QuestType.ADWatch)
        {
            return false;
        }
        else
        {
            var progress = GetProgress();

            return progress >= CurrentQuest.CompleteCount;
        }
    }

    public bool TryClearQuest()
    {       
        if (!IsEnableClear())
        {
            return false;
        }
        
        _adWatch = false;
        DataManager.CurrencyData.Add(Enum_CurrencyType.Gem, CurrentQuest.RewardCount);
        
        CurrentQuestIndex = Mathf.Min(CurrentQuestIndex, TBL_QUEST.CountEntities - 1);
        
        return true;
    }
}