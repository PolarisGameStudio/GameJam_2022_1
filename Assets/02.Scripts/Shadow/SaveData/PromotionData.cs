using System;

public class PromotionData : StatData
{
    public int CurrenctPromotionIndex { get; set; }

    [NonSerialized] public bool IsMaxPromotion;

    public PromotionData()
    {
        CurrenctPromotionIndex = -1;
    }

    public override void ValidCheck()
    {
        base.ValidCheck();

        CalculateStat();
    }

    public void OnClearPromotionBattle()
    {
        CurrenctPromotionIndex++;
        
        CalculateStat();
    }
    
    protected override void CalculateStat()
    {
        Stat.Init();

        Stat[Enum_StatType.Damage] = 1;
        Stat[Enum_StatType.MaxHealth] = 1;
        
        for (int i = 0; i <= CurrenctPromotionIndex; i++)
        {
            if (i >= TBL_PROMOTION.CountEntities)
            {
                break;
            }

            var data = TBL_PROMOTION.GetEntity(i);

            if (data.DamageMultipleValue >= 1)
            {
                Stat[Enum_StatType.Damage] *= data.DamageMultipleValue;
            } 
            
            if (data.HealthMultipleValue >= 1)
            {            
                Stat[Enum_StatType.MaxHealth] *= data.HealthMultipleValue;
            }
        }
        
        RefreshEvent.Trigger(Enum_RefreshEventType.StatChange);
    }
}
