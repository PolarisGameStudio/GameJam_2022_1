using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_World : UI_BasePopup<UI_Popup_World>
{
    public List<UI_Popup_World_Slot> SlotList;

    private TBL_WORLD _selectWorld;
    
    protected override void Refresh()
    {
        var curStage = TBL_STAGE.GetEntity(DataManager.StageData.StageLevel);

        _selectWorld = curStage.World;

        SetWorldStageList();
    }

    public void OnClickNext()
    {
        if (_selectWorld.Index >= TBL_WORLD.CountEntities - 1)
        {
            return;
        }
        _selectWorld = TBL_WORLD.GetEntity(_selectWorld.Index + 1);
        SetWorldStageList();
        
        SoundManager.Instance.PlaySound("ui_common_button");
    }
    
    public void OnClickPre()
    {
        if (_selectWorld.Index <= 0)
        {
            return;
        }
        _selectWorld = TBL_WORLD.GetEntity(_selectWorld.Index - 1);
        SetWorldStageList();
        
        SoundManager.Instance.PlaySound("ui_common_button");
    }
    
    private void SetWorldStageList()
    {
        var worldStageList = TBL_STAGE.GetEntitiesByKeyWithWorld(_selectWorld);

        for (int i = 0; i < SlotList.Count; i++)
        {
            if (worldStageList.Count <= i)
            {
                SlotList[i].SafeSetActive(false);
                continue;
            }
            
            SlotList[i].SafeSetActive(true);
            SlotList[i].Init(worldStageList[i]);
        }
    }
}