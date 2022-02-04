using UnityEngine.UI;

public class UI_Popup_Dungeon_Smith : UI_BasePopup<UI_Popup_Dungeon_Smith>
{
    public Text _txtBestScore;

    public Text _txtStoneAmount;
    
    public Text _txtPriceEnter;
    public Text _txtPriceSkip;

    public void OnClickChallenge()
    {
        if (BattleManager.Instance.CurrentBattleType != Enum_BattleType.Stage)
        {
            UI_Popup_OK.Instance.Open("도전", "스테이지에서만 도전 가능합니다.");
            return;
        }
        
        if (DataManager.DungeonData.TryChallenge(Enum_BattleType.SmithDungeon))
        {
            Close();
        }
    }

    public void OnClickSkip()
    {    
        if (DataManager.DungeonData.TrySkipDungeon(Enum_BattleType.SmithDungeon))
        {
        }
    }

    protected override void Refresh()
    {
        _txtBestScore.text = $"현재 층 수 : {DataManager.DungeonData.SmithDungeonHighLevel}";
        
        _txtStoneAmount.text = $"x {TBL_DUNGEON_SMITH.GetEntity(DataManager.DungeonData.SmithDungeonHighLevel).EquipmentStonCount}";
        
        _txtPriceEnter.text = $"{DataManager.CurrencyData.GetAmount(Enum_CurrencyType.Ticket_Smith)} / 1";
        _txtPriceSkip.text = $"{DataManager.CurrencyData.GetAmount(Enum_CurrencyType.Ticket_Smith)} / 1";
    }
}