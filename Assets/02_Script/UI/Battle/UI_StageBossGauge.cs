using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageBossGauge : GameBehaviour, GameEventListener<RefreshEvent>
{
    [SerializeField] private Slider _bossGaugeSlider;
    [SerializeField] private Text _bossGaugeAmountText;
    [SerializeField] private Button _bossChallengeButton;
    [SerializeField] private Text txtTitleStage;

    private StageBattle _stageBattle;

    public void Start()
    {
        _stageBattle = BattleManager.Instance.GetBattle<StageBattle>();
        
        this.AddGameEventListening<RefreshEvent>();
        
        Refresh();
    }
    
    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type != Enum_RefreshEventType.Battle)
        {
            return;
        }

        Refresh();
    }

    private void Refresh()
    {
        if (BattleManager.Instance.CurrentBattleType != Enum_BattleType.Stage)
        {
            SafeSetActive(false);
            return;
        }

        if (_stageBattle.IsBossTime)
        {
            SafeSetActive(false);
            return;
        }

        SafeSetActive(true);
        
        _bossGaugeSlider.value = _stageBattle.CurrentBossGauge / (float)_stageBattle.RequireBossGauge;
        _bossGaugeAmountText.text = $"{_stageBattle.CurrentBossGauge}/{_stageBattle.RequireBossGauge}";
        txtTitleStage.text = $"<{_stageBattle.StageName} wave :{_stageBattle.WaveLevel + 1}>";
        _bossChallengeButton.gameObject.SetActive(_stageBattle.IsEnableBossChallenge);
    }
    
    public void OnClickChallengeButton()
    {
        if (_stageBattle.TryBossChallenge())
        {
            
        }
        else
        {
            
        }
    }

    public void OnToggleAutoChallenge(bool isOn)
    {
        _stageBattle.TryBossChallenge();
        OptionManager.Instance.ToggleAutoBossChallenge(isOn);
    }
    
}
