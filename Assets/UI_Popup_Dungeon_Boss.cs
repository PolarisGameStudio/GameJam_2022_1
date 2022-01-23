
using UnityEngine.UI;

public class UI_Popup_Dungeon_Boss : UI_BasePopup<UI_Popup_Dungeon_Boss>
{
    public Text _txtBestScore;

    public Text _txtPriceEnter;
    public Text _txtPriceSkip;

    public void OnClickChallenge()
    {
        if (DataManager.DungeonData.TryChallenge(Enum_BattleType.BossDungeon))
        {
            Close();
        }
    }

    public void OnClickSkip()
    {    
        if (DataManager.DungeonData.TrySkipDungeon(Enum_BattleType.BossDungeon))
        {
        }
    }

    protected override void Refresh()
    {
        _txtBestScore.text = $"최대 데미지 : {DataManager.DungeonData.BossDungeonHighestDamage}";
        _txtPriceEnter.text = $"{DataManager.CurrencyData.GetAmount(Enum_CurrencyType.Ticket_Boss)} / 1";
        _txtPriceSkip.text = $"{DataManager.CurrencyData.GetAmount(Enum_CurrencyType.Ticket_Boss)} / 1";
    }
}