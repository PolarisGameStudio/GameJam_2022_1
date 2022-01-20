using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop_Gacha_Slot : UI_BaseSlot<TBL_GACHA_DATA>
{
    [SerializeField] private Text _txtLevel;
    [SerializeField] private Text _txtExp;

    [SerializeField] private Text _txtPriceSmall;
    [SerializeField] private Text _txtPriceBig;
    
    [SerializeField] private Slider _sliderGauge;

    public override void Init(TBL_GACHA_DATA data)
    {
        _data = data;

        Refresh();
    }

    public void Refresh()
    {
        _txtLevel.text = $"소환 레벨{DataManager.GachaData.GetGachaLevel(_data.GachaType)}";

        var curExp = DataManager.GachaData.GetGachaCount(_data.GachaType);
        var preRequreExp = DataManager.GachaData.GetPreRequireExp(_data.GachaType);
        var requireExp = DataManager.GachaData.GetNextRequireExp(_data.GachaType);
        
        _txtExp.text = $"{curExp} / {requireExp}";

        _sliderGauge.minValue = preRequreExp;
        _sliderGauge.maxValue = requireExp;
        _sliderGauge.value = curExp;

        _txtPriceSmall.text = $"{_data.Price_Small}";
        _txtPriceBig.text = $"{_data.Price_Big}";
    }

    public void OnClickSmallGacha()
    {
        if (DataManager.CurrencyData.TryConsume(Enum_CurrencyType.Gem, _data.Price_Small))
        {
            GachaManager.Instance.Gacha(_data.GachaType, _data.Count_Small);
        }
    }

    public void OnClickBigGacha()
    {
        if (DataManager.CurrencyData.TryConsume(Enum_CurrencyType.Gem, _data.Price_Big))
        {
            GachaManager.Instance.Gacha(_data.GachaType, _data.Count_Big);
        }
    }

    public void OnClickAdGacha()
    {
        AdManager.Instance.TryShowRequest(ADType.Gacha, () =>
        {
            GachaManager.Instance.Gacha(_data.GachaType, _data.Count_Small);
        });
    }
}