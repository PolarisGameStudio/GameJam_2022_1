public struct ShopEvent
{
    private static ShopEvent e;
    
    public static void Trigger()
    {
        GameEventManager.TriggerGameEvent(e);
    }
}