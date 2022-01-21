using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FollowerEvent 
{
    public Enum_FollowerEventType Type;
    public int Value;
    
    private static FollowerEvent e;

    public static void Trigger(Enum_FollowerEventType type)
    {
        e.Type = type;
        e.Value = -1;
        
        GameEventManager.TriggerGameEvent(e);
    }
}

public enum Enum_FollowerEventType
{
    Equip,
}