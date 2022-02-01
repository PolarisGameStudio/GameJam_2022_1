using System.Collections.Generic;

public class UI_Shop_Package : SingletonBehaviour<UI_Shop_Package>, GameEventListener<ShopEvent>
{
    public List<UI_Shop_Package_Slot> PackageSlots;

    private void OnEnable()
    {
        this.AddGameEventListening<ShopEvent>();
    }

    private void OnDisable()
    {
        this.RemoveGameEventListening<ShopEvent>();
    }

    public void OnGameEvent(ShopEvent e)
    {
        Refresh();
    }

    public void Refresh()
    {
        for (var i = 0; i < PackageSlots.Count; i++)
        {
            PackageSlots[i].Init(TBL_PACKAGE.GetEntity(i));
        }
    }


    private void Start()
    {
        Refresh();
    }
}