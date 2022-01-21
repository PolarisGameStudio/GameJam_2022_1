using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingGachaHandler : GachaHandler
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

        var level = DataManager.GachaData.GetGachaLevel(GachaType.Ring);

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
        
        
        var filterList = list.FindAll(x => x.Type == Enum_EquipmentType.Ring && x.Star == randomStar);
        
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
        List<TBL_EQUIPMENT> equipmentList = new List<TBL_EQUIPMENT>();
        
        for (int i = 0; i< resultList.Count; i++)
        {
            var result = GachaByGrade((Enum_ItemGrade) resultList[i]);
            var equipment = TBL_EQUIPMENT.GetEntity(result);
            
            equipmentList.Add(equipment);
        }
        
        DataManager.EquipmentData.AddEquipmentList(equipmentList);
        
        DataManager.AcheievmentData.ProgressAchievement(Enum_AchivementMission.Daily_GachaEquipment, resultList.Count);
        DataManager.AcheievmentData.ProgressAchievement(Enum_AchivementMission.Loop_GachaEquipment, resultList.Count);
    }
}