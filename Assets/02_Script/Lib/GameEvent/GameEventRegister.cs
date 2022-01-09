
public static class GameEventRegister
{
    public static void AddGameEventListening<GEvent>(this GameEventListener<GEvent> caller) where GEvent : struct
    {
        GameEventManager.AddListener<GEvent>(caller);
    }

    public static void RemoveGameEventListening<GEvent>(this GameEventListener<GEvent> caller) where GEvent : struct
    {
        GameEventManager.RemoveListener<GEvent>(caller);
    }
}