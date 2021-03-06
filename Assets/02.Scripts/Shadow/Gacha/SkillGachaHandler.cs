using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGachaHandler : GachaHandler
{
     public override List<int> Gacha(int count, bool skipChest = false)
    {
        List<int> skillGachaResults = GetGachaResultList(count);

        GachaResultAction(skillGachaResults);

        return skillGachaResults;
    }

    public override int GetRandomIndex()
    {
        float sum = 0;

        var level = DataManager.GachaData.GetGachaLevel(GachaType.Skill);

        List<float> percents = TBL_GACHA_SKILL.GetEntity(level).Percents;

        percents.ForEach(percent => sum += percent);

        float currentSum = 0;

        float randomFloat = Random.Range(0, sum);

        for (var i = 0; i < percents.Count; i++)
        {
            currentSum += percents[i];

            if (randomFloat <= currentSum)
            {
                return i;
            }
        }

        return 0;
    }

    public override int GachaByGrade(Enum_ItemGrade grade)
    {
        var list = TBL_SKILL.GetEntitiesByKeyWithGrade(grade);
        
        int randomIndex = Random.Range(0, list.Count);
        
        var data = list[randomIndex];
        
        return data.Index;
    }

    public override List<int> GetGachaResultList(int gachaCount)
    {
        List<int> gachaResults = new List<int>(gachaCount);

        for (int i = 0; i < gachaCount; ++i)
        {
            gachaResults.Add(GetRandomIndex());
        }

        return gachaResults;
    }

    public override void GachaResultAction(List<int> resultList)
    {
        List<int> skillList = new List<int>();
        
        for (int i = 0; i< resultList.Count; i++)
        {
            var result = GachaByGrade((Enum_ItemGrade) resultList[i]);
            
            skillList.Add(result);
        }
        
        DataManager.SkillData.AddSkillList(skillList);
        
        DataManager.AchievementData.ProgressAchievement(Enum_AchivementMission.Loop_GachaSkill, resultList.Count);
        
        List<Reward> rewards = new List<Reward>();

        for (int i = 0; i < skillList.Count; i++)
        {
            rewards.Add(new Reward(RewardType.Skill,skillList[i],1));
        }

        var data = TBL_GACHA_SKILL.GetEntity(DataManager.GachaData.GetGachaLevel(GachaType.Skill));

        Enum_ItemGrade hightest = Enum_ItemGrade.Myth;

        for (int i = data.Percents.Count - 1 ; i > 0 ; i--)
        {
            if (data.Percents[i] > 0)
            {
                hightest = (Enum_ItemGrade) i;
                break;
            }
        }

        UI_Popup_Gacha.Instance.Open(rewards, hightest);
    }
}
