using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WeaponEvent 
{
    public Enum_WeaponEventType Type;
    public int Value;
    
    private static WeaponEvent e;

    public static void Trigger(Enum_WeaponEventType type)
    {
        e.Type = type;
        e.Value = -1;
        
        GameEventManager.TriggerGameEvent(e);
    }
}

public enum Enum_WeaponEventType
{
    Equip,
}