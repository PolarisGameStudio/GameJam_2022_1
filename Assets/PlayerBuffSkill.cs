using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffSkill : PlayerActiveSkill
{
    public Enum_StatType StatType; // 게임잼용 , 한번에 여러 스킬 넣어야할수도있으니 이렇게하면 안댐

    private Stat _stat;
    
    private float _timer;
   
    protected override IEnumerator SkillActiveCoroutine(List<CharacterObject> targets)
    {
        _timer = 0;

        if (_stat == null)
        {
            _stat = new Stat();
        }

        _stat.Init();
        _stat[StatType] = Value;
        
        _playerObject.AddBuff(new Buff(_stat,_data.Time));

        while (_timer < _data.Time)
        {
            _timer += Time.deltaTime;
            _skillVFX.transform.position = _playerObject.Position;

            yield return null;
        }
        
        Hide();
    }
}
