using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SkillEvent
{
    public Enum_SkillEventType Type;
    public int Index;

    private static SkillEvent e;

    public static void Trigger(Enum_SkillEventType type,int index)
    {
        e.Type = type;
        e.Index = index;
        
        GameEventManager.TriggerGameEvent(e);
    }
}
