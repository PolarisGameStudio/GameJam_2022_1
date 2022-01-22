using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_World_Slot : UI_BaseSlot<TBL_STAGE>
{
    public Text _txtStageName;

    public Text _txtGoldReward;
    public Text _txtStoneReward;
    public Text _txtWeaponReward;
    public GameObject _onDiable;

    public override void Init(TBL_STAGE data)
    {
        _data = data;
        _txtStageName.text = $"{_data.World.name}{_data.Index % 20 + 1}";
        _txtGoldReward.text = $"{_data.Gold}";
        _txtStoneReward.text = $"{_data.UpgradeStone}";
        _txtWeaponReward.text = $"{_data.EquipmentGrade}";

        _onDiable.SetActive(_data.Index > DataManager.StageData.HighestStageLevel);
    }

    public void OnClickStart()
    {
        UI_Popup_World.Instance.Close();
        DataManager.StageData.TryStartStage(_data.Index);
    }
}