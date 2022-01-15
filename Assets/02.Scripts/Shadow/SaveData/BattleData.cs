using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BattleData
{
    public int StageLevel;



    public void BossStageClear()
    {
        StageLevel++;
    }
}
