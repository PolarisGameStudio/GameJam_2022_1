using System;
using UnityEngine;

[Serializable]
public class PlayerData : StatData
{
    public int Level = 0;
    public double Exp = 0;

    public bool IsReviewShown;

    public override void ValidCheck()
    {
	    base.ValidCheck();
	    
	    InitStat();
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public double GetRequireExp()
    {
	    if (TBL_PLAYER_LEVEL.CountEntities <= Level)
	    {
		    return Double.MaxValue;
	    }

	    var data = TBL_PLAYER_LEVEL.GetEntity(Level);
	    
	    return data.NextLevelExperience;
    }

    public void LevelUp()
    {
        Level++;

        if (!IsReviewShown && Level >= 10)
        {
	        UI_Popup_Review.Instance.Open();
        }
        
        DataManager.StatGrowthData.CalculateStatPoint();
        
        //RefreshEvent.Trigger(Enum_RefreshEventType.StatChange);
        PlayerEvent.Trigger(Enum_PlayerEventType.LevelUp);
    }

    public bool TryLevelUp()
    {
	    return CheckLevelUp();
    }

    public float GetExpPercents()
    {
	    return (float)(Exp / GetRequireExp());
    }

    public void AddExp(double exp)
    {
	    Exp += exp;
	 //   CheckLevelUp();
	    
	    PlayerEvent.Trigger(Enum_PlayerEventType.Exp);
    }

    private bool CheckLevelUp()
    {
	    double requireExp = GetRequireExp();
        
        double remainExp = Exp - requireExp;
        if (remainExp >= 0)
        {
	        Exp = remainExp;
	        
	        LevelUp();
        	
        	//CheckLevelUp();	// 재귀로 다시 검사. : TODO : 레벨업 여러단계 한 번에 되게 하려면 수정 필요.
            
            return true;
        }

        return false;
    }

    protected override void InitStat()
    {
	    Stat.Init();
	    
	    TBL_PLAYER.ForEachEntity(x => Stat[x.StatType] += x.Value);
    }

    protected override void CalculateStat()
    {
    }
}
