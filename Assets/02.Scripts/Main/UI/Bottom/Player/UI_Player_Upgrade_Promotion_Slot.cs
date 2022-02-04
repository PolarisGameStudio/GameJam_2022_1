using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player_Upgrade_Promotion_Slot : UI_BaseSlot<TBL_PROMOTION>
{
    [SerializeField] private Image _imgPromotionIcon;

    [SerializeField] private Text _txtPromotionName;
    [SerializeField] private Text _txtPromotionStatValue;
    //[SerializeField] private Text _txtRecommendLevel;

    [SerializeField]  private Button _btnChallange;
    [SerializeField] private GameObject _onDisableObject;
    

    public override void Init(TBL_PROMOTION data)
    {
        _data = data;
        
        _imgPromotionIcon.sprite = AssetManager.Instance.PromotionIcon[_data.Index];
        _txtPromotionName.text = $"{_data.name}";

        _txtPromotionStatValue.text =
            $"공격력 x{_data.DamageMultipleValue} , 체력 x{_data.HealthMultipleValue}";

  //      _txtRecommendLevel.text = "추천레벨 ?";
        
        Refresh();
    }

    private void Refresh()
    {
        _onDisableObject.gameObject.SetActive(!DataManager.PromotionData.IsAlreadyClear(_data.Index));

        _btnChallange.gameObject.SetActive(DataManager.PromotionData.IsEnableChallenge(_data.Index));
    }
    public void OnClickChallengeButton()
    {
        if (BattleManager.Instance.CurrentBattleType != Enum_BattleType.Stage)
        {
            UI_Popup_OK.Instance.Open("도전", "스테이지에서만 도전 가능합니다.");
            return;
        }
        
        DataManager.PromotionData.TryChallenge(_data.Index);
    }
}
