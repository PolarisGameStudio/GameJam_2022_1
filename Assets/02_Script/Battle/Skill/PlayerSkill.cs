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

    public float Value;
    
    public virtual void InitSkill(TBL_SKILL data, PlayerObject playerObject)
    {
        _data = data;
        _playerObject = playerObject;
    }
    
    
    public bool IsSkillEnable()
    {
        return _data.CoolTime <= _coolTimer;
    }
    public void UpdateCoolTime(float deltaTime)
    {
        _coolTimer += deltaTime;
    }

    public void Calculate()
    {
        // todo: 어쩌고 공식
        Value = _data.Value + _data.IncreaseValue * DataManager.SkillData.Levels[_data.Index];
    }

    public virtual bool TryUseSkill()
    {
        return false;
    }

    public List<CharacterObject> FindTargetByDistance(int distance)
    {
        if (_playerObject == null)
        {
            Debug.LogError("플레이어가 없슴");
            return null;
        }
        
        List<CharacterObject> targets = new List<CharacterObject>();

        foreach (var monsterObject in BattleManager.Instance.CurrentBattle.MonsterObjects)
        {
            float x = monsterObject.Position.x - _playerObject.Position.x;

            if (x <= distance * SystemValue.SKILL_DISTANCE_BLOCK_SIZE)
            {
                targets.Add(monsterObject);
            }
        }

        return targets;
    }
}
