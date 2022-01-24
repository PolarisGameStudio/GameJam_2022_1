using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageData : SaveDataBase
{
    public int StageLevel;
    public int HighestStageLevel;

    public override void ValidCheck()
    {
        base.ValidCheck();
        
#if UNITY_EDITOR
        HighestStageLevel = 999;
#endif
    }

    public void BossStageClear()
    {
        StageLevel = Mathf.Min(StageLevel + 1, TBL_STAGE.CountEntities - 1);

        if (HighestStageLevel < StageLevel)
        {
            HighestStageLevel = StageLevel;
        }
    }

    public void TryStartStage(int index)
    {
        if (HighestStageLevel < index)
        {
            return;
        }
        
        StageLevel = index;
        BattleManager.Instance.BattleStart(Enum_BattleType.Stage, StageLevel);
    }
}
