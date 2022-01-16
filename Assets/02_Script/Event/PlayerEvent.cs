using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct PlayerEvent
{
    public Enum_PlayerEventType Type;

    private static PlayerEvent e;

    public static void Trigger(Enum_PlayerEventType type)
    {
        e.Type = type;
        
        GameEventManager.TriggerGameEvent(e);
    }
}