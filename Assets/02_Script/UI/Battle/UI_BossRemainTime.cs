using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossRemainTime : GameBehaviour, GameEventListener<RefreshEvent>
{
    [SerializeField] private Slider _remainTimeSlider;
    [SerializeField] private Text _remainTimeText;
    
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

        if (!_stageBattle.IsBossTime)
        {
            SafeSetActive(false);
            return;
        }

        SafeSetActive(true);
    }

    private void Update()
    {
        if (_stageBattle && _stageBattle.IsBossTime)
        { 
            _remainTimeSlider.value =  _stageBattle.CurrentBossRemainTime / _stageBattle.MaxBossRemainTime;
            _remainTimeText.text = $"{_stageBattle.CurrentBossRemainTime:N1}s";
        }
    }
}
