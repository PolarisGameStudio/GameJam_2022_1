using System;

[Serializable]
public abstract class StatData : SaveDataBase
{    
    [NonSerialized] public Stat Stat = new Stat();

    protected abstract void InitStat();

    protected abstract void CalculateStat();
}
