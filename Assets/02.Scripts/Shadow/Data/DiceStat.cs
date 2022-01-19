using System;

[Serializable]
public class DiceStat
{
    public int Index = -1; //주사위 스탯 테이블 인덱스
    public int AddValue = -1; //MinStatValue + AddValue => RealValue

    public bool IsLock;

    private bool IsActive = false;

    public void SetActivation(bool isActive)
    {
        IsActive = isActive;
    }

    public Enum_ItemGrade GetGrade()
    {
        return TBL_UPGRADE_DICE.GetEntity(Index).Grade;
    }

    public void ToggleLock()
    {
        IsLock = !IsLock;
    }

    public void InitStat(int index, int addValue)
    {
        if (IsLock || !IsActive)
        {
            return;
        }
        
        Index = index;
        AddValue = addValue;
    }
}
