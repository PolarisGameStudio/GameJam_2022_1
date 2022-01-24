using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Reward_Slot : UI_BaseSlot<Reward>
{
    [Header("아이템 아이콘")] public Image _icon;
    [Header("아이템 프레임")] public Image _frame;
    [Header("아이템 프레임")] public Image _category;
    [Header("아이템 갯수")] public Text _amountText;

    private Sprite _defaultFrame;

    private bool _ignoreFrame;
    
    public override void Init(Reward data)
    {
        _data = data;

        if (!_defaultFrame)
        {
            _defaultFrame = _frame.sprite;
        }

        Set();
    }

    // public void Init(Reward data, bool ignoreFrame)
    // {
    //     _ignoreFrame = ignoreFrame;
    //     Init(data);
    // }

    protected virtual void Set()
    {
        if (_data.RewardType == RewardType.None)
        {
            SafeSetActive(false);
            return;
        }
        
        SafeSetActive(true);
        
        _category.enabled = false;
        
        _icon.sprite = AssetManager.Instance.GetSpriteWithRewardType(_data.RewardType, _data.Value);
        _amountText.text = _data.Count == 0 ? "" : $"x{_data.Count.ToCurrencyString()}";
        _frame.sprite = AssetManager.Instance.GetFrameWithItemGrade(_data.RewardType, _data.Value);

        if (_data.RewardType == RewardType.Equipment)
        {
            var equip = TBL_EQUIPMENT.GetEntity(_data.Value);

            if (equip.Type == Enum_EquipmentType.Ring)
            {
                return;
            }
            else
            {
                _category.enabled = true;
                _category.sprite = AssetManager.Instance.EquipmentTypeIcon[(int)equip.Type];
            }
        }
    }
}
