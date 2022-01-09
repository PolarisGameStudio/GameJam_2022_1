using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAbility : CharacterAbility
{
    private Transform _transform;

    public override void Init()
    {
        base.Init();
        
        _transform = transform;
    }

    public void SetPosition(Vector3 newPosition)
    {
        _transform.position = newPosition;
    }

    public Vector3 GetPosition()
    {
        return _transform.position;
    }

    public void MoveToRight(float dt)
    {
        _transform.Translate(dt * (float)_onwerObject.Stat[Enum_StatType.MoveSpeed], 0f, 0f);
    }

    public void MoveToLeft(float dt)
    {
        _transform.Translate(-dt * (float)_onwerObject.Stat[Enum_StatType.MoveSpeed], 0f, 0f);
    }
}
