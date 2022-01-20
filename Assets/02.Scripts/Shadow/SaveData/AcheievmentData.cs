using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public enum Enum_AchivementType
{
    Loop,
    Daily,
}

public enum Enum_AchivementMission
{
    Daily_ClearAllDailyAchievement, // 모든 일일 퀘스트 완료
    Daily_GachaEquipment, // 장비 소환
    Daily_SessionTime, // 플레이 타임 (30분)
    Daily_AdWatch, // 광고 시청 (1회)
    Daily_MergeEquipment, // 장비 합성
    Daily_EnterDungeon, // 던전 입장 

    Loop_LevelUpEquipment, // 장비 강화
    Loop_MergeEquipment, // 장비 합성
    Loop_KillMonster, // 몬스터 처치
    Loop_GachaEquipment, // 장비 뽑기 (무기 or 반지)
    Loop_GachaSkill, // 스킬 뽑기
    Loop_SessionTime, // 게임 플레이 시간
    Loop_LevelUpSkill, // 스킬 레벨 업
    Loop_LevelUpGoldGrowth, // 골드 성장 (아무 골드 성장 버튼)
    
    Count,
}

public class AcheievmentData : SaveDataBase
{
    // 일일 업적
    // public List<int> Daily_Progress = new List<int>();
    // public List<bool> Daily_IsClear = new List<bool>();
    // // 반복 업적
    // public List<int> Loop_Progress = new List<int>();
    // public List<bool> Loop_IsClear = new List<bool>();
    
    public List<int> Progress = new List<int>();
    public List<bool> IsClear = new List<bool>();
    public List<bool> IsDaily = new List<bool>();


    public override void ValidCheck()
    {
        base.ValidCheck();

        var acheiveCount = TBL_ACHIEVEMENT.CountEntities;
        
        var saveCount = Progress.Count;
        
        if (acheiveCount > saveCount)
        {
            for (int i = saveCount; i < acheiveCount; i++)
            {
                Progress.Add(0);
                IsClear.Add(false);
                IsDaily.Add(false);
            }
        }
    }

    private void InitLoopAcheievement()
    {
        // var dataList = TBL_ACHIEVEMENT.GetEntitiesByKeyWithAchievementKind(Enum_AchivementType.Loop);
        // if (dataList == null) return;
        //
        // var typeCount = dataList.Count;
        //
        // var saveCount = Daily_Progress.Count;
        //
        // if (typeCount > saveCount)
        // {
        //     for (int i = saveCount; i < typeCount; i++)
        //     {
        //         Loop_Progress.Add(0);
        //         Loop_IsClear.Add(false);
        //     }
        // }
    }

    private void InitDailyAcheievement()
    {
        // var dataList = TBL_ACHIEVEMENT.GetEntitiesByKeyWithAchievementKind(Enum_AchivementType.Daily);
        // if (dataList == null) return;
        //
        // var typeCount = dataList.Count;
        //
        // var saveCount = Daily_Progress.Count;
        //
        // if (typeCount > saveCount)
        // {
        //     for (int i = saveCount; i < typeCount; i++)
        //     {
        //         Daily_Progress.Add(0);
        //         Daily_IsClear.Add(false);
        //     }
        // }
    }

    public void ProgressAchievement(Enum_AchivementMission mission, int count = 1)
    {
        int index = (int) mission;

        if (index < Progress.Count)
        {
            Progress[index] += count;
        }
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Acheieve);
    }

    public bool IsEnableClear(Enum_AchivementMission mission)
    {
        int index = (int) mission;

        var data = TBL_ACHIEVEMENT.GetEntity(index);
        
        return true;
    }

    public void ClearAchieve(Enum_AchivementMission mission)
    {
        
    }

    public Enum_AchivementType GetAchivementType(Enum_AchivementMission mission)
    {
        switch (mission)
        {
            case Enum_AchivementMission.Daily_ClearAllDailyAchievement:
            case Enum_AchivementMission.Daily_GachaEquipment:
            case Enum_AchivementMission.Daily_SessionTime:
            case Enum_AchivementMission.Daily_AdWatch:
            case Enum_AchivementMission.Daily_MergeEquipment:
            case Enum_AchivementMission.Daily_EnterDungeon:
                return Enum_AchivementType.Daily;

            case Enum_AchivementMission.Loop_LevelUpEquipment:
            case Enum_AchivementMission.Loop_MergeEquipment:
            case Enum_AchivementMission.Loop_KillMonster:
            case Enum_AchivementMission.Loop_GachaEquipment:
            case Enum_AchivementMission.Loop_GachaSkill:
            case Enum_AchivementMission.Loop_SessionTime:
            case Enum_AchivementMission.Loop_LevelUpSkill:
            case Enum_AchivementMission.Loop_LevelUpGoldGrowth:
                return Enum_AchivementType.Loop;

            default:
                throw new Exception($"{mission} 얘는 업적 없음");
                return Enum_AchivementType.Daily;
        }
    }

    public override void OnNextDay()
    {
        // for (var i = 0; i < Daily_Progress.Count; i++)
        // {
        //     Daily_Progress[i] = 0;
        //     Daily_IsClear[i] = false;
        // }
    }
}