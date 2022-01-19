using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_Test_Stat : MonoBehaviour , GameEventListener<StatEvent>
{
    public Transform TextParent;
    public List<Text> Texts;

    private void Awake()
    {
        this.AddGameEventListening<StatEvent>();
        Texts = TextParent.GetComponentsInChildren<Text>().ToList();
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        for (var i = 0; i < Texts.Count; i++)
        {
            if (i >= (int)Enum_StatType.Count)
            {
                return;
            }
            Texts[i].text = DataManager.Container.Stat[(Enum_StatType)i].ToCurrencyString();
        }
    }
    

    public void OnGameEvent(StatEvent e)
    {
        if (e.Type == Enum_StatEventType.StatCalculate)
        {
            Refresh();
        }
    }
}
