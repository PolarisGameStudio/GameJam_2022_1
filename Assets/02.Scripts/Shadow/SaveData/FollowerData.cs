using System.Collections.Generic;

public class FollowerData : StatData
{
    public List<int> Levels;
    public List<int> Counts;

    public override void ValidCheck()
    {
        base.ValidCheck();


        CalculateStat();
    }
    
    protected override void InitStat()
    {
    }

    protected override void CalculateStat()
    {
    }
}

public class Follower
{
    public int Level;
    public int PieceCount;
    
    // public DiceStatData Dice; 
}
