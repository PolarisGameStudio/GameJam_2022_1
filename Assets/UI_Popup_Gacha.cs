using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Gacha : UI_BasePopup<UI_Popup_Gacha>
{
    public List<UI_Popup_Reward_Slot> SlotList;

    public GameObject Buttons;
    public GameObject BtnMore;
    public Text _txtPrice;


    private List<Reward> _rewards;
    private Coroutine _coroutine;

    public void Open(List<Reward> rewards)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        _rewards = rewards;
        
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
        var period = new WaitForSecondsRealtime(0.05f);

        for (var i = 0; i < SlotList.Count; i++)
        {
            if (i >= _rewards.Count)
            {
                SlotList[i].SafeSetActive(false);
            }
            else
            {
                SlotList[i].SafeSetActive(true);
                SlotList[i].Init(_rewards[i]);

                yield return period;
            }
        }
        
        yield return new WaitForSecondsRealtime(0.5f);
        
        Buttons.SetActive(true);
    }

    public void OnClickMore()
    {
        Close();
        GachaManager.Instance.ReGacha();
    }
}
