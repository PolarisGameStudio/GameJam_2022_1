using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OptionSetting
{
    public bool SleepOn = true;
    public bool EffectOn = true;
    public bool ChatOn = true;
}


public class OptionManager : SingletonBehaviour<OptionManager>
{
    private OptionSetting m_OptionSetting = new OptionSetting();
    public bool IsSleepOn => m_OptionSetting.SleepOn;

    public bool IsEffectOn => m_OptionSetting.EffectOn;
    public bool IsChatOn => m_OptionSetting.ChatOn;

    protected override void Awake()
    {
        base.Awake();

        LoadOptionSetting();
    }

    public void SetSleepOption(bool isOn)
    {
        m_OptionSetting.SleepOn = isOn;
        SaveOptionSetting();
    }
    internal void SetEffectOption(bool value)
    {
        m_OptionSetting.EffectOn = value;
        SaveOptionSetting();
    }
    
    internal void SetChatOption(bool value)
    {
        m_OptionSetting.ChatOn = value;
        SaveOptionSetting();

       
    }

    const string SaveKey = "OptionSetting";
    public void SaveOptionSetting()
    {
        ES3.Save<OptionSetting>(SaveKey, m_OptionSetting);
    }

    public void LoadOptionSetting(Action succeedAction = null, Action failedAction = null)
    {
        m_OptionSetting = ES3.Load<OptionSetting>(SaveKey, new OptionSetting());
    }
}
