
public interface GameEventListenerBase
{

}

public interface GameEventListener<T> : GameEventListenerBase
{
    void OnGameEvent(T gameEventType);
}