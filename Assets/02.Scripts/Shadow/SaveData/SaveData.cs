
using System;

[Serializable]
public abstract class SaveDataBase
{
    public bool IsLoaded = false;
    
    public virtual void ValidCheck()
    {
    }

    public virtual void OnNextDay()
    {
        
    }
}
