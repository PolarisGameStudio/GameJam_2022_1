using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StatEvent 
{
    public Enum_StatEventType Type;
    public int Value;
    
    private static StatEvent e;

    public static void Trigger(Enum_StatEventType type)
    {
        e.Type = type;
        e.Value = -1;
        
        GameEventManager.TriggerGameEvent(e);
    }
}
