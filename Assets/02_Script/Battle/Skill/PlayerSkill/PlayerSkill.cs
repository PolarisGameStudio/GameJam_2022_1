using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : GameBehaviour
{
    // 데이터 변수
    protected PlayerObject _playerObject;

    protected PlayerSkillData _data;
    public PlayerSkillData Data => _data;

    protected float _coolTimer;

    public float RemainCoolTime => _data.CoolTime - _coolTimer;

    public float RemainCoolTimeNormalized => _coolTimer / _data.CoolTime;

    private float _value;
    
    public virtual void InitSkill(PlayerSkillData data, PlayerObject playerObject)
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
        _value = _data.Value1 + _data.Value2;
    }

    public virtual bool TryUseSkill()
    {
        return false;
    }
}
