using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MonsterEvent
{
    public Enum_MonsterEventType Type;

    private static MonsterEvent e;

    public static void Trigger(Enum_MonsterEventType type)
    {
        e.Type = type;
        
        GameEventManager.TriggerGameEvent(e);
    }
}
