using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Shop_Currency : SingletonBehaviour<UI_Shop_Currency>, GameEventListener<ShopEvent>
{
    public List<UI_Shop_Gem_Slot> GemSlots;

    private void OnEnable()
    {
        this.AddGameEventListening<ShopEvent>();
    }

    private void OnDisable()
    {
        this.RemoveGameEventListening<ShopEvent>();
    }

    public void OnGameEvent(ShopEvent e)
    {
        Refresh();
    }

    public void Refresh()
    {
        for (var i = 0; i < GemSlots.Count; i++)
        {
            GemSlots[i].Init(TBL_SHOP.GetEntity(i));
        }
    }

    private void Start()
    {
        Refresh();
    }
}