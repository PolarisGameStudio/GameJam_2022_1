using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Battle : MonoBehaviour
{
    [SerializeField] [Header("전투 타입")] 
    private Enum_BattleType _battleType;
    public Enum_BattleType BattleType => _battleType;

    protected int _level;
    public int Level => _level;
    
    protected List<CharacterObject> _monsterObjects = new List<CharacterObject>(3);
    public List<CharacterObject> MonsterObjects => _monsterObjects;

    public double DamageFactor;
    public double HealthFactor;
    public double GoldAmount;
    public double ExpAmount;
    
    protected PlayerObject _player;
    
    public virtual void BattleInit(int level)
    {
        _level = level;

        _monsterObjects.Clear();
        
        OnBattleInit();

        _player = BattleManager.Instance.PlayerObject;
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Battle);

    }
    protected abstract void OnBattleInit();

    
    public virtual void BattleStart()
    {
        OnBattleStart();
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Battle);
    }
    protected abstract void OnBattleStart();


    public virtual void BattleClear()
    {
        OnBattleClear();
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Battle);
    }
    protected abstract void OnBattleClear();

    
    public virtual void BattleOver()
    {
        OnBattleOver();
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Battle);
    }
    protected abstract void OnBattleOver();
    

    public virtual void BattleEnd()
    {
        MonsterObjectFactory.Instance.HideAll();
        
        _monsterObjects.Clear();
        
        OnBattleEnd();
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Battle);
    }
    protected abstract void OnBattleEnd();

}
