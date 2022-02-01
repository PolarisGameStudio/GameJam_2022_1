using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop_Gacha_Percents : UI_BasePopup<UI_Shop_Gacha_Percents>
{
    public Text _txtLevel;
    public List<Text> _txtGachaPercentList;

    public GameObject _btnArrowLeft;
    public GameObject _btnArrowRight;

    private int _selectedLevel;

    private GachaType _selectedType;
    

    public void Open(GachaType gachaType)
    {
        _selectedType = gachaType;
        _selectedLevel = DataManager.GachaData.GetGachaLevel(_selectedType);
        
        Open();
    }

    protected override void Refresh()
    {
        _txtLevel.text = $"{_selectedLevel + 1}단계";
        
        _btnArrowLeft.SetActive(_selectedLevel != 0);
        _btnArrowRight.SetActive(_selectedLevel != DataManager.GachaData.GetGachaMaxLevel(_selectedType) - 1);

        List<float> percents = new List<float>();
        
        switch (_selectedType)
        {
            case GachaType.Weapon:
            case GachaType.Ring:
                percents = TBL_GACHA_EQUIPMENT.GetEntity(_selectedLevel).Percents;
                break;

            case GachaType.Skill:
                percents = TBL_GACHA_SKILL.GetEntity(_selectedLevel).Percents;
                break;
        }

        if (percents.Count == 0)
        {
            Close();
        }

        for (var i = 0; i < percents.Count; i++)
        {
            if (_txtGachaPercentList.Count <= i)
            {
                break;
            }

            _txtGachaPercentList[i].color = percents[i] == 0 ? Color.gray : Color.white;
            _txtGachaPercentList[i].text = $"{percents[i].ToString("N3")}%";
        }
    }

    public void OnClickArrow(bool isLeft)
    {
        if (isLeft)
        {
            _selectedLevel--;
        }
        else
        {
            _selectedLevel++;
        }

        _selectedLevel = Mathf.Clamp(_selectedLevel, 0, DataManager.GachaData.GetGachaMaxLevel(_selectedType) - 1);

        Refresh();
    }
}