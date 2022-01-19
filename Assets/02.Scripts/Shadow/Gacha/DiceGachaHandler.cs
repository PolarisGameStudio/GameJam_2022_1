using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceGachaHandler : GachaHandler
{
    public override List<int> Gacha(int count, bool skipChest = false)
    {
        List<int> diceGachaResults = GetGachaResultList(count);

        GachaResultAction(diceGachaResults);

        return diceGachaResults;
    }

    public override int GetRandomIndex()
    {
        float sum = 0;

        List<float> percents = TBL_GACHA_DICE.GetEntity(0).Percents;

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