using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHealthbar : SingletonBehaviour<UI_BossHealthbar>
{
    public Text _txtBossDungeonLevel;
    private Healthbar _healthbar;

    private CharacterObject _owner;

    protected override void Awake()
    {
        base.Awake();
        
        _healthbar = GetComponent<Healthbar>();
    }

    public void Show(CharacterObject owner)
    {
        _owner = owner;
        
        _healthbar.InitOwner(owner);

        SafeSetActive(true);
    }

    public void Hide()
    {
        _owner = null;

        SafeSetActive(false);
    }

    private void LateUpdate()
    {
        if (!_owner || _owner.IsDeath)
        {
            SafeSetActive(false);
            return;
        }

        if (BattleManager.Instance.CurrentBattle.BattleType == Enum_BattleType.BossDungeon)
        {
            _txtBossDungeonLevel.text = $"Lv.{BattleManager.Instance.CurrentBattle.Level+1} 탐관오리";    
        }
        else
        {
            _txtBossDungeonLevel.text = "";
        }
        
        _healthbar.UpdateHealth();
    }
}