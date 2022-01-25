using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossRemainTime : GameBehaviour, GameEventListener<BattleEvent>
{
    [SerializeField] private Slider _remainTimeSlider;
    [SerializeField] private Text _remainTimeText;

    private BossDungeonBattle _bossDungeonBattle;
    private TreasureDungeonBattle _treasureDungeonBattle;
    

    public void Start()
    {
        _bossDungeonBattle = BattleManager.Instance.GetBattle<BossDungeonBattle>();
        _treasureDungeonBattle = BattleManager.Instance.GetBattle<TreasureDungeonBattle>();
        
        this.AddGameEventListening<BattleEvent>();
        
        Refresh();
    }
    
    public void OnGameEvent(BattleEvent e)
    {
        if (e.Type != Enum_BattleEventType.BattleStart)
        {
            return;
        }
        
        Refresh();
    }

    private void Refresh()
    {
        if (BattleManager.Instance.CurrentBattleType != Enum_BattleType.TreasureDungeon &&
                BattleManager.Instance.CurrentBattleType != Enum_BattleType.BossDungeon)
        {
            SafeSetActive(false);
            return;
        }

        SafeSetActive(true);
    }

    private void Update()
    {
        if (BattleManager.Instance.CurrentBattle == _bossDungeonBattle)
        {
            _remainTimeSlider.value = _bossDungeonBattle.RemainTime / SystemValue.BOSS_DUNGEON_LIMIT_TIME;
            _remainTimeText.text = $"{_bossDungeonBattle.RemainTime:N1}s";   
        }
        else if (BattleManager.Instance.CurrentBattle == _treasureDungeonBattle)
        {
            _remainTimeSlider.value = _treasureDungeonBattle.RemainTime / SystemValue.TREASURE_DUNGEON_LIMIT_TIME;
            _remainTimeText.text = $"{_treasureDungeonBattle.RemainTime:N1}s";
        }
    }
}
