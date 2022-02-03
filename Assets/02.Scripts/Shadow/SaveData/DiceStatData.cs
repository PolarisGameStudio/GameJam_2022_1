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

    public bool IsHighGradeExist()
    {
        var check = DiceSlotList.Find(diceStat => !diceStat.IsLock && diceStat.GetGrade() > Enum_ItemGrade.Legendary) !=
                    null;

        return check;
    }

    public bool TryRoll(bool force = false)
    {
        if (!force && IsHighGradeExist())
        {
            UI_Popup_Buy.Instance.Open("추가 능력", "높은 등급의 추가 능력이 있습니다. 그래도 초기화하시겠습니까?", Enum_CurrencyType.Dice,
                GetRollPrice(), () => { TryRoll(true); });

            return false;
        }

        if (IsEnableRoll())
        {
            if (!DataManager.CurrencyData.TryConsume(Enum_CurrencyType.Dice, GetRollPrice()))
            {
                return false;
            }

            var result = GachaManager.Instance.Gacha(GachaType.Dice, 0, DiceSlotList.Count);

            for (int i = 0; i < result.Count; i++)
            {
                Enum_ItemGrade grade = (Enum_ItemGrade) result[i];

                var targets = TBL_UPGRADE_DICE.GetEntitiesByKeyWithGrade(grade);

                var targetData = targets[Random.Range(0, targets.Count - 1)];

                var addValue = Random.Range(0, (targetData.MaxStatValue + 1) - targetData.MinStatValue);

                DiceSlotList[i].InitStat(targetData.Index, addValue);
            }

            CalculateStat();

            return true;
        }

        return false;
    }
}