using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectAbility : DetectAbility
{
    private PlayerObject _playerObject;
    
    public override void Init()
    {
        base.Init();
        
        _playerObject = BattleManager.Instance.PlayerObject;
    }
    
    protected override void Detect()
    {
        float playerX = _playerObject.transform.position.x;
        float myX = transform.position.x;

        if (Mathf.Abs(playerX - myX) <= _onwerObject.Stat[Enum_StatType.DetectRange])
        {
            if (_targets.Count == 0)
            {
                _targets.Add(_playerObject);
            }
        }
        else
        {
            if (_targets.Count > 0)
            {
                _targets.Remove(_playerObject);
            }
        }
    }
}
