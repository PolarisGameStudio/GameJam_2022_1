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

        int randomIndex = Random.Range(0, list.Count);

        var data = list[randomIndex];

        while (true)
        {
            if (data.Type == Enum_EquipmentType.Ring)
            {
                break;
            }
        }

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
    }
}