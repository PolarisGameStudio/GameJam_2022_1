using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatGrowthData : StatData
{
    protected override void InitStat()
    {
        throw new System.NotImplementedException();
    }

    protected override void CalculateStat()
    {
        throw new System.NotImplementedException();
    }

    public void CalculateStatPoint()
    {
        var usedPoint = 0;
        //._darkStatList.ForEach(stat => usedPoint += stat.UsedPoint);

        // m_KingInfo.DarkPoint = Level - usedPoint;
    }
}
