using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Gacha : UI_BasePopup<UI_Popup_Gacha>
{
    public List<UI_Popup_Reward_Slot> SlotList;
    public List<ParticleSystem> NormalVFXList;
    public List<ParticleSystem> SpecialVFXList;

    public GameObject Buttons;
    public GameObject BtnMore;
    public Text _txtPrice;


    private List<Reward> _rewards;
    private Coroutine _coroutine;

    private Enum_ItemGrade _highestGrade;
    
    public void Open(List<Reward> rewards, Enum_ItemGrade highestGrade)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        _rewards = rewards;
        _highestGrade = highestGrade;
        
        Buttons.SetActive(false);
        SlotList.ForEach(x => x.gameObject.SetActive(false));

        var price = GachaManager.Instance.GetLastGachaPrice();
        
        _txtPrice.text = price.ToString();
        BtnMore.SetActive(price != 0);
        
        base.Open();
    }
    

    protected override void Refresh()
    {
        _coroutine = StartCoroutine(OpenSlotCoroutine());
    }

    IEnumerator OpenSlotCoroutine()
    {
        var period = new WaitForSecondsRealtime(0.02f);

        for (var i = 0; i < SlotList.Count; i++)
        {
            if (i >= _rewards.Count)
            {
                SlotList[i].SafeSetActive(false);
            }
            else
            {
                NormalVFXList[i].Stop();
                SpecialVFXList[i].Stop();
                
                SlotList[i].SafeSetActive(true);
                SlotList[i].Init(_rewards[i]);

                bool isHighestGrade = false;
                
                switch (_rewards[i].RewardType)
                {
                    case RewardType.Skill:
                        isHighestGrade = TBL_SKILL.GetEntity(_rewards[i].Value).ItemGrade == _highestGrade;
                        break;
                    case RewardType.Equipment:
                        isHighestGrade = TBL_EQUIPMENT.GetEntity(_rewards[i].Value).Grade == _highestGrade;
                        break;
                }

                if (!isHighestGrade)
                {
                    NormalVFXList[i].Play();
                }
                else
                {
                    SpecialVFXList[i].Play();
                }
                
                yield return period;
            }
        }
        
        yield return new WaitForSecondsRealtime(0.1f);
        
        Buttons.SetActive(true);
    }

    public void OnClickMore()
    {
        Close();
        GachaManager.Instance.ReGacha();
    }
}
