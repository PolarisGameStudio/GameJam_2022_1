public struct BattleEvent
{
    public Enum_BattleEventType Type;
    public Enum_BattleType BattleType;
    
    private static BattleEvent e;

    public static void Trigger(Enum_BattleEventType type, Enum_BattleType battleType)
    {
        e.Type = type;
        e.BattleType = battleType;
        
        GameEventManager.TriggerGameEvent(e);
    }
}
