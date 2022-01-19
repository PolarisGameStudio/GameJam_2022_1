using System;

[Serializable]
public abstract class StatData : SaveDataBase
{    
    [NonSerialized] public Stat Stat = new Stat();

    protected virtual void InitStat()
    {
    }

    protected abstract void CalculateStat();
}
