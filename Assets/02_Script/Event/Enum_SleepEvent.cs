public enum Enum_SleepEventType
{
    SleepOn,
    SleepOff,
}

public struct SleepEvent
{
    public Enum_SleepEventType Type;
    
    private static SleepEvent e;

    public static void Trigger(Enum_SleepEventType type)
    {
        e.Type = type;
        
        GameEventManager.TriggerGameEvent(e);
    }
}