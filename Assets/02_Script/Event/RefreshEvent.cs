using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RefreshEvent
{
    public Enum_RefreshEventType Type;

    private static RefreshEvent e;

    public static void Trigger(Enum_RefreshEventType type)
    {
        e.Type = type;
        
        GameEventManager.TriggerGameEvent(e);
    }
}
