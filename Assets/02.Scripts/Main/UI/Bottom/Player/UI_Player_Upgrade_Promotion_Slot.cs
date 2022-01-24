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
        
        //_imgPromotionIcon.sprite = null;
        
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
        DataManager.PromotionData.TryChallenge(_data.Index);
    }
}
