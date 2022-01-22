using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup_Follower_Equip : UI_BasePopup<UI_Popup_Follower_Equip>
{
    public List<UI_Follower_Slot> Slots;
    private TBL_FOLLOWER _data;

    public void Open(TBL_FOLLOWER data)
    {
        base.Open();
        _data = data;

        Refresh();
    }

    protected override void Refresh()
    {
        for (int i = 0; i < DataManager.FollowerData.EquippedIndex.Count; i++)
        {
            var index = DataManager.FollowerData.EquippedIndex[i];

            if (index < 0 || index >= TBL_FOLLOWER.CountEntities)
            {
                Slots[i].Init(null);
                continue;
            }

            Slots[i].Init(TBL_FOLLOWER.GetEntity(index));
        }
    }

    public void OnClickSelectSlot(int index)
    {
        DataManager.FollowerData.TryEquip(_data.Index, index);
        UI_Popup_Follower.Instance.Close();
        Close();
    }
}