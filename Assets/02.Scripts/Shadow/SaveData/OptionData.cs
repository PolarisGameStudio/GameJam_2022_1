using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionData : SaveDataBase
{
    public bool AutoSleep;

    public float BGMVolume;
    public float SFXVolume;

    public void ToggleAutoSleep(bool isOn)
    {
        AutoSleep = isOn;
    }

    public void SetBGMVolume(float value)
    {
        BGMVolume = value;
    }    
    
    public void SetSFXVolume(float value)
    {
        SFXVolume = value;
    }
}
