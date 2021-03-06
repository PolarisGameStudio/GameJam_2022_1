using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : GameBehaviour
{
    // 데이터 변수
    protected PlayerObject _playerObject;

    protected TBL_SKILL _data;
    public TBL_SKILL Data => _data;

    protected float _coolTimer;

    public float RemainCoolTime => _data.CoolTime - _coolTimer;

    public float RemainCoolTimeNormalized => _coolTimer / _data.CoolTime;

    public float Value => _data.Value + _data.IncreaseValue * (DataManager.SkillData.Levels[_data.Index] - 1);
    
    public virtual void InitSkill(TBL_SKILL data, PlayerObject playerObject)
    {
        _data = data;
        _playerObject = playerObject;
        
        _coolTimer = _data.CoolTime;
    }
    
    public bool CanUseSKill()
    {
        return _data.CoolTime <= _coolTimer;
    }
    
    public void UpdateCoolTime(float deltaTime)
    {
// #if UNITY_EDITOR
//         _coolTimer += deltaTime * (_data.CoolTime / 4f);
//         return;
// #endif
        _coolTimer += deltaTime;
    }

    public virtual bool TryUseSkill()
    {
        return false;
    }

    public List<CharacterObject> FindTargetFromPlayer(int distance)
    {
        if (_playerObject == null)
        {
            Debug.LogError("플레이어가 없슴");
            return null;
        }
        
        List<CharacterObject> targets = new List<CharacterObject>();

        var position = (_playerObject.Position.x + (float) _playerObject.Stat[Enum_StatType.AttackRange]);

        foreach (var monsterObject in BattleManager.Instance.CurrentBattle.MonsterObjects)
        {
            float x = monsterObject.Position.x - position;

            if (x <= distance * SystemValue.SKILL_DISTANCE_BLOCK_SIZE)
            {
                targets.Add(monsterObject);
            }
        }

        return targets;
    }    
    
    public List<CharacterObject> FindTargetFromRandomPoint(Vector3 target, int distance)
    {
        if (_playerObject == null)
        {
            Debug.LogError("플레이어가 없슴");
            return null;
        }
        
        List<CharacterObject> targets = new List<CharacterObject>();

        var monsters = BattleManager.Instance.CurrentBattle.MonsterObjects;
        
        foreach (var monsterObject in monsters)
        {
            float x = monsterObject.Position.x - target.x;

            if (Mathf.Abs(x) * 2 <= distance * SystemValue.SKILL_DISTANCE_BLOCK_SIZE)
            {
                targets.Add(monsterObject);
            }
        }

        return targets;
    }

    public virtual void OnPlayerAttack(CharacterObject target)
    {
        
    }

    public virtual void PlaySkillSound()
    {
        SoundManager.Instance.PlaySound($"skill_index{_data.Index}");
    }
    
}
