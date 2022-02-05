using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionData : SaveDataBase
{
    public bool Sleep = true;
    public bool BGM = true;
    public bool SFX = true;
    public bool VFX = true;


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
