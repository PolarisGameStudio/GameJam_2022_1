using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StageWaveEvent
{
    public Enum_StageWaveEventType Type;
    public int WaveLevel;

    private static StageWaveEvent e;

    public static void Trigger(Enum_StageWaveEventType type, int waveLevel)
    {
        e.Type = type;
        e.WaveLevel = waveLevel;
        
        GameEventManager.TriggerGameEvent(e);
    }
}
