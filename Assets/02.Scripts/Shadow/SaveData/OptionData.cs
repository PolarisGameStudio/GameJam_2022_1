using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionData : SaveDataBase
{
    public bool Sleep;
    public bool BGM;
    public bool SFX;
    public bool VFX;


    public void ToggleMusic(bool value)
    {
        BGM = value;
    }

    public void ToggleSfx(bool value)
    {
        SFX = value;
    }

    public void ToggleSleep(bool value)
    {
        Sleep = value;
    }

    public void ToggleVfx(bool value)
    {
        VFX = value;
    }
}
