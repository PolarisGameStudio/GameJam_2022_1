using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop_Gacha_Slot : UI_BaseSlot<TBL_GACHA_DATA>
{
    [SerializeField] private Text _txtLevel;
    [SerializeField] private Text _txtExp;

    [SerializeField] private Text _txtAdDailyLimit;
    [SerializeField] private Text _txtPriceSmall;
    [SerializeField] private Text _txtPriceBig;

    [SerializeField] private Image _imgKey;
    [SerializeField] private GameObject _btnKey;

    [SerializeField] private Slider _sliderGauge;

    public override void Init(TBL_GACHA_DATA data)
    {
        _data = data;

        Refresh();
    }

    public void Refresh()
    {
        if (_data == null)
        {
            return;
        }

        _txtLevel.text = $"소환 레벨{DataManager.GachaData.GetGachaLevel(_data.GachaType) + 1}";

        var curExp = DataManager.GachaData.GetGachaCount(_data.GachaType);
        var preRequireExp = DataManager.GachaData.GetPreRequireExp(_data.GachaType);
        var nextExp = DataManager.GachaData.GetNextRequireExp(_data.GachaType);

        var requireExp = nextExp == int.MaxValue ? preRequireExp : nextExp;

        if (nextExp == int.MaxValue)
        {
            _txtExp.text = $"Max";

            _sliderGauge.minValue = 0;
            _sliderGauge.maxValue = 1;
            _sliderGauge.value = 1;
        }
        else
        {
            _txtExp.text = $"{curExp} / {requireExp}";

            _sliderGauge.minValue = preRequireExp;
            _sliderGauge.maxValue = requireExp;
            _sliderGauge.value = curExp;
        }

        _txtPriceSmall.text = $"{_data.Price_Small}";
        _txtPriceSmall.color = DataManager.CurrencyData.IsEnough(Enum_CurrencyType.Gem, _data.Price_Small)
            ? ColorValue.ENABLE_TEXT_COLOR
            : ColorValue.DISABLE_TEXT_COLOR;

        bool isEnough = DataManager.CurrencyData.IsEnough(_data.KeyType, 1);

        _btnKey.SetActive(isEnough);

        if (isEnough)
        {
            _imgKey.sprite = AssetManager.Instance.CurrencyIcon[(int) _data.KeyType];
        }
        else
        {
            _txtPriceBig.text = $"{_data.Price_Big}";
            _txtPriceBig.color = DataManager.CurrencyData.IsEnough(Enum_CurrencyType.Gem, _data.Price_Big)
                ? ColorValue.ENABLE_TEXT_COLOR
                : ColorValue.DISABLE_TEXT_COLOR;
        }

        _txtAdDailyLimit.text =
            $"일일제한 {DataManager.GachaData.DailyLimit[(int) _data.GachaType]}/{SystemValue.GACHA_DAILY_LIMIT}";
    }

    public void OnClickSmallGacha()
    {
        if (DataManager.CurrencyData.TryConsume(Enum_CurrencyType.Gem, _data.Price_Small))
        {
            GachaManager.Instance.Gacha(_data.GachaType, Enum_CurrencyType.Gem, _data.Price_Small, _data.Count_Small);
        }
    }

    public void OnClickBigGacha()
    {
        bool isEnough = DataManager.CurrencyData.IsEnough(_data.KeyType, 1);

        if (isEnough)
        {
            if (DataManager.CurrencyData.TryConsume(_data.KeyType, 1))
            {
                GachaManager.Instance.Gacha(_data.GachaType, _data.KeyType, 1, _data.Count_Big);
            }
        }
        else
        {
            if (DataManager.CurrencyData.TryConsume(Enum_CurrencyType.Gem, _data.Price_Big))
            {
                GachaManager.Instance.Gacha(_data.GachaType, Enum_CurrencyType.Gem, _data.Price_Big, _data.Count_Big);
            }
        }
    }

    public void OnClickAdGacha()
    {
        if (DataManager.GachaData.DailyLimit[(int) _data.GachaType] >= SystemValue.GACHA_DAILY_LIMIT)
        {
            return;
        }

        AdManager.Instance.TryShowRequest(ADType.Gacha, () =>
        {
            DataManager.GachaData.DailyLimit[(int) _data.GachaType]++;
            GachaManager.Instance.Gacha(_data.GachaType, Enum_CurrencyType.Gem, 0 , _data.Count_Small);
        });
    }

    public void OnClickPercent()
    {
        UI_Shop_Gacha_Percents.Instance.Open(_data.GachaType);
    }
}