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

    public TBL_QUEST CurrentQuest
    {
        get
        {
            if (CurrentQuestIndex < TBL_QUEST.CountEntities)
            {
                return TBL_QUEST.GetEntity(CurrentQuestIndex);
            }
            else
            {
                CheckAllQuestFinish();
                RefreshEvent.Trigger(Enum_RefreshEventType.Quest);
                Debug.LogError("All QuestComplete");
                return null;
            }
        }
    }

    [SerializeField] private bool _adWatch;
    [SerializeField] private bool _isAllQuestFinish;
    [SerializeField] public bool IsAllQuestFinish => _isAllQuestFinish;
    
    public override void ValidCheck()
    {
        base.ValidCheck();

        CheckAllQuestFinish();
    }

    public void CheckAllQuestFinish()
    {
        if (CurrentQuestIndex >= TBL_QUEST.CountEntities)
        {
            _isAllQuestFinish = true;
        }
    }

    public int GetProgress()
    {
        if (_isAllQuestFinish)
        {
            return int.MaxValue;
        }
        
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
            
            case QuestType.GachaSkill:
                return DataManager.GachaData.GetGachaCount(GachaType.Skill);
            
            case QuestType.ClearStage:
                return DataManager.StageData.HighestStageLevel + 1;
            
            case QuestType.EquipSkill:
                return DataManager.SkillData.EquippedIndex.FindAll(x => x != -1).Count > 0 ? int.MaxValue : 0;
            
            case QuestType.ClearTreasureDungeon:
                return DataManager.DungeonData.TreasureDungeonHighLevel;

            case QuestType.EquipmentLevel:
            {
                int highestLevel = 0;
                DataManager.EquipmentData.Levels.ForEach(level =>
                {
                    if (level > highestLevel)
                    {
                        highestLevel = level;
                    }
                });
                return highestLevel;
            }
            
            case QuestType.ADWatch:
                return _adWatch ? 1 : 0;
            
            case QuestType.Promotion:
                return DataManager.PromotionData.CurrentPromotionIndex;
            
            case QuestType.ClearBossDungeon:
                return DataManager.DungeonData.BossDungeonHighLevel;
            
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
        if (_isAllQuestFinish)
        {
            return false;
        }
        
        var progress = GetProgress();

        return progress >= CurrentQuest.CompleteCount;
    }

    public bool TryClearQuest()
    {       
        if (!IsEnableClear())
        {
            return false;
        }
        
        _adWatch = false;
        DataManager.CurrencyData.Add(Enum_CurrencyType.Gem, CurrentQuest.RewardCount);
        
        CurrentQuestIndex++;
        CheckAllQuestFinish();
        
        DataManager.Instance.Save(force:true);
        
        return true;
    }
}