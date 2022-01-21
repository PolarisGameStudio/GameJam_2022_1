using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;


[Serializable]
public class DiceStatData : StatData
{
    public List<DiceStat> DiceSlotList = new List<DiceStat>();

    public void Init(int diceCount)
    {
        base.ValidCheck();

        if (DiceSlotList == null)
        {
            DiceSlotList = new List<DiceStat>();
        }
        
        //todo: 승급이면 승급 갯수 만큼 일반
        var gachaTypeCount = diceCount;

        var saveCount = DiceSlotList.Count;

        if (gachaTypeCount > saveCount)
        {
            for (int i = saveCount; i < gachaTypeCount; i++)
            {
                DiceSlotList.Add(new DiceStat());
            }
        }

        CalculateStat();
    }

    public void ActiveDiceSlot(int activeCount)
    {
        for (var i = 0; i < DiceSlotList.Count; i++)
        {
            DiceSlotList[i].SetActivation(i < activeCount);
        }
    }

    protected override void CalculateStat()
    {
        Stat.Init();

        foreach (var diceStat in DiceSlotList)
        {
            if (diceStat.Index == -1)
            {
                continue;
            }
            var data = TBL_UPGRADE_DICE.GetEntity(diceStat.Index);
            Stat[data.StatType] += diceStat.AddValue + data.MinStatValue;
        }
    }

    public int GetRollPrice()
    {
        int price = 0;

        DiceSlotList.ForEach(diceStat =>
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
        #if UNITY_EDITOR
        return true;
        #endif
        
        return DataManager.CurrencyData.IsEnough(Enum_CurrencyType.Dice, GetRollPrice()) &&
               DiceSlotList.Find(diceStat => !diceStat.IsLock) != null;
    }

    public bool TryRoll()
    {
        if (IsEnableRoll())
        {
            var result = GachaManager.Instance.Gacha(GachaType.Dice, DiceSlotList.Count);

            for (int i = 0; i < result.Count; i++)
            {
                Enum_ItemGrade grade = (Enum_ItemGrade) result[i];

                var targets = TBL_UPGRADE_DICE.GetEntitiesByKeyWithGrade(grade);

                var targetData = targets[Random.Range(0, targets.Count - 1)];

                var addValue = Random.Range(0, (targetData.MaxStatValue + 1) - targetData.MinStatValue);

                DiceSlotList[i].InitStat(targetData.Index, addValue);
            }

            return true;
        }
        
        return false;
    }
}