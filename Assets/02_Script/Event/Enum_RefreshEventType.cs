
public enum Enum_RefreshEventType
{
    Battle,
    Berserk,
    Buff,
    Currency,
    
    
    Skill,
    LevelUp,
    
    Follower,
    Equipment,
    
    GoldGrowth,
    StatGrowth,
    Promotion,
    Rune,
    Acheieve,
}

public struct RefreshEvent
{
    public Enum_RefreshEventType Type;
    public int Value;
    
    private static RefreshEvent e;

    public static void Trigger(Enum_RefreshEventType type)
    {
        e.Type = type;
        e.Value = -1;
        
        GameEventManager.TriggerGameEvent(e);
    }
    
    public static void Trigger(Enum_RefreshEventType type, int value)
    {
        e.Type = type;
        e.Value = value;
        
        GameEventManager.TriggerGameEvent(e);
    }
}