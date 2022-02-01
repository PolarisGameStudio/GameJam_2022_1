using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LoadingBlocker : UI_BasePopup<UI_LoadingBlocker>
{
    protected override void Refresh()
    {
    }

    public override void Close()
    {
        SafeSetActive(false);
    }

    public override void Open()
    {
        SafeSetActive(true);
    }
}
