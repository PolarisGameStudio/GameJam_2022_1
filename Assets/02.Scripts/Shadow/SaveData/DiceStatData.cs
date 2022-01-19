using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;


[Serializable]
public class DiceStatData : StatData
{
    public List<DiceStat> DiceStatList;

    public void Init(int diceCount)
    {
        base.ValidCheck();
        
        //todo: 승급이면 승급 갯수 만큼 일반
        var gachaTypeCount = diceCount;

        var saveCount = DiceStatList.Count;

        if (gachaTypeCount > saveCount)
        {
            for (int i = saveCount; i < gachaTypeCount; i++)
            {
                DiceStatList.Add(new DiceStat());
            }
        }

        CalculateStat();
    }

    protected override void CalculateStat()
    {
        Stat.Init();

        foreach (var diceStat in DiceStatList)
        {
            var data = TBL_UPGRADE_DICE.GetEntity(diceStat.Index);
            Stat[data.StatType] += diceStat.AddValue + data.MinStatValue;
        }
    }

    public int GetRollPrice()
    {
        int price = 0;

        DiceStatList.ForEach(diceStat =>
        {
            if (diceStat.IsLock)
            {
                price += 100;
            }
            else
            {
                price += 10;
            }
        });

        return price;
    }

    public bool IsEnableRoll()
    {
        return DataManager.CurrencyData.IsEnough(Enum_CurrencyType.Dice, GetRollPrice()) &&
               DiceStatList.Find(diceStat => !diceStat.IsLock) != null;
    }

    public void TryRoll()
    {
        if (IsEnableRoll())
        {
            var result = GachaManager.Instance.Gacha(GachaType.Dice, DiceStatList.Count);

            for (int i = 0; i < result.Count; i++)
            {
                Enum_ItemGrade grade = (Enum_ItemGrade) result[i];

                var targets = TBL_UPGRADE_DICE.GetEntitiesByKeyWithGrade(grade);

                var targetData = targets[Random.Range(0, targets.Count - 1)];

                var addValue = Random.Range(0, (targetData.MaxStatValue + 1) - targetData.MinStatValue);

                DiceStatList[i].InitStat(targetData.Index, addValue);
            }
        }
    }
}