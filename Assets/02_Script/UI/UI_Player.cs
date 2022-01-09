using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Player : SingletonBehaviour<UI_Player>
{
    protected override void Awake()
    {
        base.Awake();
        Close();
    }

    public void Close()
    {
        SafeSetActive(false);
    }

    public void Open()
    {
        SafeSetActive(true);
    }
}
