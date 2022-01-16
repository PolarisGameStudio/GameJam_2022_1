using System;
using UnityEngine;

[Serializable]
public class PlayerLevelData : StatData
{
    public int Level = 0;
    public double Exp = 0;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private double GetRequireExp()
    {
	    return 100;
    }

    private void LevelUp()
    {
        Level++;
    }

    public void AddExp(double exp)
    {
	    Exp += exp;
	    CheckLevelUp();
    }

    private void CheckLevelUp()
    {

	    double requireExp = GetRequireExp();
        
        double remainExp = Exp - requireExp;
        if (remainExp >= 0)
        {
	        Exp = remainExp;
        	Level++;
        	
        	RefreshEvent.Trigger(Enum_RefreshEventType.PlayerStat);
        	RefreshEvent.Trigger(Enum_RefreshEventType.LevelUp);
        	
        	CheckLevelUp();	// 재귀로 다시 검사. : TODO : 레벨업 여러단계 한 번에 되게 하려면 수정 필요.
        }
    }

    protected override void InitStat()
    {
	    throw new NotImplementedException();
    }

    protected override void CalculateStat()
    {
	    throw new NotImplementedException();
    }
}
