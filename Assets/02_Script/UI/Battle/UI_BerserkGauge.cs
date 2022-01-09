using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BerserkGauge : GameBehaviour, GameEventListener<RefreshEvent>
{
    [SerializeField] private Image _berserkGaugeImage;

    private const string _berserkGaugePropertyName = "_FillLevel"; 
        
    private void Start()
    {
        _berserkGaugeImage.material = Instantiate(_berserkGaugeImage.material);
        
        this.AddGameEventListening<RefreshEvent>();
        
        Refresh();
    }
    
    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type != Enum_RefreshEventType.Berserk)
        {
            return;
        }

        Refresh();
    }

    private void Refresh()
    {
        var value = BerserkManager.Instance.CurrentGauge / BerserkManager.Instance.MaxGauge;

        SetBerserkGauge(value);
    }

    private void Update()
    {
        if (!BerserkManager.Instance.IsBerserkMode)
        {
            return;
        }
        
        SetBerserkGauge(BerserkManager.Instance.CurrentRemainTime / BerserkManager.Instance.MaxRemainTime);
    }

    private void SetBerserkGauge(float value)
    {
       // _berserkGaugeImage.material.SetFloat(Shader.PropertyToID(_berserkGaugePropertyName), value);
       _berserkGaugeImage.fillAmount = value;
    }

    public void OnClickButton()
    {
        BerserkManager.Instance.TryBerserkMode();
    }

    public void OnToggleAutoBerserk(bool isOn)
    {
        OptionManager.Instance.ToggleAutoBerserkMode(isOn);
    }
    
    public void OnToggleAutoSkill(bool isOn)
    {
        OptionManager.Instance.ToggleAutoSkill(isOn);
    }
}
