using System.Collections.Generic;

public class SkillData : SaveDataBase
{
    public List<int> EquippedIndex = new List<int>();

    public List<int> Levels = new List<int>();
    public List<int> Counts = new List<int>();

    public override void ValidCheck()
    {
        base.ValidCheck();

        int maximumSlotCount = SystemValue.SKILL_MAX_SLOT_COUNT;
        int saveSlotCount = EquippedIndex.Count;

        if (maximumSlotCount > saveSlotCount)
        {
            for (int i = saveSlotCount; i < maximumSlotCount; i++)
            {
                EquippedIndex.Add(-1);
            }
        }

        int skillCount = TBL_SKILL.CountEntities;
        int saveCount = Levels.Count;

        if (skillCount > saveCount)
        {
            for (int i = saveCount; i < skillCount; i++)
            {
                Levels.Add(0);
                Counts.Add(0);
            }
        }
    }

    public bool IsEnableLevelUp(int index)
    {
        var data = TBL_SKILL.GetEntity(index);
        var level = Levels[index];

        if (level >= SystemValue.SKILL_MAX_LEVEL)
        {
            return false;
        }

        return Counts[index] >= GetLevelUpCost(index);
    }

    public int GetLevelUpCost(int index)
    {
        var data = TBL_SKILL.GetEntity(index);
        var level = Levels[index];

        return level == 0 ? data.UnlockCost : data.LevelUpCost + data.IncreaseCost * (level - 1);
    }

    public bool TryLevelUp(int index)
    {
        if (!IsEnableLevelUp(index))
        {
            return false;
        }

        Counts[index] -= GetLevelUpCost(index);
        Levels[index] += 1;
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Skill);
        
        return true;
    }

    public void TryEquip(int index, int changeSlotIndex)
    {     
        var alreadyEquip = EquippedIndex.FindIndex(x => x == index);
        if (alreadyEquip != -1)
        {
            EquippedIndex[alreadyEquip] = -1;
        }
        
        EquippedIndex[changeSlotIndex] = index;
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Skill);
        PlayerEvent.Trigger(Enum_PlayerEventType.EquipSkill);
        RefreshEvent.Trigger(Enum_RefreshEventType.Quest);
    }

    public void AddSkill(int rewardValue, int rewardCount)
    {
        if (Counts.Count > rewardValue)
        {
            Counts[rewardValue] += rewardCount;
        }
        RefreshEvent.Trigger(Enum_RefreshEventType.Skill);
    } 
    
    public void AddSkillList(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int index = list[i];
            int count = 1;

            Counts[index] += count;
        }
        RefreshEvent.Trigger(Enum_RefreshEventType.Skill);
    }

    public void TryUnEquip(int index)
    {
        var equippedIndex = EquippedIndex.FindIndex(x => x == index);

        if (equippedIndex != -1 && EquippedIndex.Count < equippedIndex)
        {
            EquippedIndex[equippedIndex] = -1;
        }
        RefreshEvent.Trigger(Enum_RefreshEventType.Skill);
    }
    

    public bool IsSlotUnlock(int slotIndex)
    {
        #if UNITY_EDITOR
        return true;
        #endif
        return DataManager.PlayerData.Level >= GetUnlockCondition(slotIndex);
    }

    public int GetUnlockCondition(int slotIndex)
    {
        return (slotIndex - 1) * 10;
    }
}