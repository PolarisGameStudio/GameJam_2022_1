using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Shop_Gacha : MonoBehaviour , GameEventListener<ShopEvent>
{
    public UI_Shop_Gacha_Slot WeaponGachaSlot;
    public UI_Shop_Gacha_Slot RingGachaSlot;
    public UI_Shop_Gacha_Slot SkillGachaSlot;

    private void OnEnable()
    {
        this.AddGameEventListening<ShopEvent>();
        
        Refresh();
    }

    private void OnDisable()
    {
        this.RemoveGameEventListening<ShopEvent>();
    }

    public void OnToggle(bool isOn)
    {
        gameObject.SetActive(isOn);
    }

    private void Start()
    {
        WeaponGachaSlot.Init(TBL_GACHA_DATA.GetEntityByKeyWithType(GachaType.Weapon));
        RingGachaSlot.Init(TBL_GACHA_DATA.GetEntityByKeyWithType(GachaType.Ring));
        SkillGachaSlot.Init(TBL_GACHA_DATA.GetEntityByKeyWithType(GachaType.Skill));

        Refresh();
    }

    public void Refresh()
    {
        WeaponGachaSlot.Refresh();
        RingGachaSlot.Refresh();
        RingGachaSlot.Refresh();
    }

    public void OnGameEvent(ShopEvent e)
    {
        Refresh();
    }
}
