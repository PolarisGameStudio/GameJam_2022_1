using System.Collections;
using System.Collections.Generic;
using EnhancedScrollerDemos.Chat;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player_Upgrade_Promotion_Slot : UI_BaseSlot<TBL_PROMOTION>,GameEventListener<RefreshEvent>
{
    [SerializeField] private Image _imgPromotionIcon;

    [SerializeField] private Text _txtPromotionName;
    [SerializeField] private Text _txtPromotionStatValue;
    //[SerializeField] private Text _txtRecommendLevel;

    [SerializeField]  private Button _btnChallange;
    [SerializeField] private GameObject _onDisableObject;
    

    private void OnEnable()
    {
        this.AddGameEventListening<RefreshEvent>();

        if (_data != null)
        {
            Refresh();
        }
    }

    private void OnDisable()
    {
        this.RemoveGameEventListening<RefreshEvent>();
    }
    
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

        _btnChallange.interactable = DataManager.PromotionData.IsEnableChallenge(_data.Index);
    }

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Battle)
        {
            Refresh();
        }
    }

    public void OnClickChallengeButton()
    {
        DataManager.PromotionData.TryChallenge(_data.Index);
    }
}
