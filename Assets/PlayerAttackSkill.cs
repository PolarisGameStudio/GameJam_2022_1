using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSkill : PlayerActiveSkill
{
    private bool _isActive;

    public override void OnPlayerAttack(CharacterObject target)
    {
        if (_isActive && UtilCode.GetChance(_data.SubValue))
        {
            target.TryTakeHit(_playerObject.Stat[Enum_StatType.Damage] * _data.Value, Enum_DamageType.Normal);
            PlaySkillSound();
        }
    }

    protected override IEnumerator SkillActiveCoroutine(List<CharacterObject> targets)
    {
        _isActive = true;

        transform.position = _playerObject.transform.position;

        yield return new WaitForSecondsRealtime(_data.Time);

        _isActive = false;
        
        Hide();
    }
}
