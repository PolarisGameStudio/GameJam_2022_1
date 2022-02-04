
using UnityEngine.UI;

public class UI_Popup_Dungeon_Boss : UI_BasePopup<UI_Popup_Dungeon_Boss>
{
    public Text _txtBestScore;

    public Text _txtPriceEnter;
    public Text _txtPriceSkip;

    public void OnClickChallenge()
    {
        if (BattleManager.Instance.CurrentBattleType != Enum_BattleType.Stage)
        {
            UI_Popup_OK.Instance.Open("도전", "스테이지에서만 도전 가능합니다.");
            return;
        }
        
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
        _txtBestScore.text = $"최대 데미지 : {DataManager.DungeonData.BossDungeonHighestDamage.ToCurrencyString()}";
        _txtPriceEnter.text = $"{DataManager.CurrencyData.GetAmount(Enum_CurrencyType.Ticket_Boss)} / 1";
        _txtPriceSkip.text = $"{DataManager.CurrencyData.GetAmount(Enum_CurrencyType.Ticket_Boss)} / 1";
    }
}