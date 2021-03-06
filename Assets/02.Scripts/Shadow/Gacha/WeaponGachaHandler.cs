using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGachaHandler : GachaHandler
{
    public override List<int> Gacha(int count, bool skipChest = false)
    {
        List<int> equipmentGachaResult = GetGachaResultList(count);

        GachaResultAction(equipmentGachaResult);

        return equipmentGachaResult;
    }

    public override int GetRandomIndex()
    {
        float sum = 0;

        var level = DataManager.GachaData.GetGachaLevel(GachaType.Weapon);

        List<float> percents = TBL_GACHA_EQUIPMENT.GetEntity(level).Percents;

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
        var list = TBL_EQUIPMENT.GetEntitiesByKeyWithGrade(grade);
        var randomStar = GachaManager.Instance.GetRandomStarCount();
        
        var filterList = list.FindAll(x => x.Type != Enum_EquipmentType.Ring && x.Star == randomStar);
        
        int randomIndex = Random.Range(0, filterList.Count);

        var data = filterList[randomIndex];

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
        List<int> indexList = new List<int>();
        for (int i = 0; i< resultList.Count; i++)
        {
            var result = GachaByGrade((Enum_ItemGrade) resultList[i]);
            
            indexList.Add(result);
        }
        
        DataManager.EquipmentData.AddEquipmentList(indexList);
        
        DataManager.AchievementData.ProgressAchievement(Enum_AchivementMission.Daily_GachaEquipment, resultList.Count);
        DataManager.AchievementData.ProgressAchievement(Enum_AchivementMission.Loop_GachaEquipment, resultList.Count);

        List<Reward> rewards = new List<Reward>();

        for (int i = 0; i < indexList.Count; i++)
        {
            rewards.Add(new Reward(RewardType.Equipment,indexList[i],1));
        }
        

        var data = TBL_GACHA_EQUIPMENT.GetEntity(DataManager.GachaData.GetGachaLevel(GachaType.Weapon));

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