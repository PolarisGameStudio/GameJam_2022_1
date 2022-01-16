public abstract class UI_BaseSlot<T> : GameBehaviour
{
    protected T _data;
    public abstract void Init(T data);
}