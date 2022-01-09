
public struct BerserkEvent
{
    public Enum_BerserkEventType Type;

    private static BerserkEvent e;

    public static void Trigger(Enum_BerserkEventType type)
    {
        e.Type = type;
        
        GameEventManager.TriggerGameEvent(e);
    }
}
