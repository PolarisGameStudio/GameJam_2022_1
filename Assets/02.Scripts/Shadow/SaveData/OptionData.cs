using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionData : SaveDataBase
{
    public bool Sleep;
    public bool BGM;
    public bool SFX;
    public bool VFX;


    public void ToggleMusic()
    {
        BGM = !BGM;
    }

    public void ToggleSfx()
    {
        SFX = !SFX;
    }

    public void ToggleSleep()
    {
        Sleep = !Sleep;
    }

    public void ToggleVfx()
    {
        VFX = !VFX;
    }
}
