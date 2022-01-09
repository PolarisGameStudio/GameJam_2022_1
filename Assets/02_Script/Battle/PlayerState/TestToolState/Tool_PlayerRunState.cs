using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_PlayerRunState : PlayerRunState
{
    public Tool_PlayerRunState(CharacterObject owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
    }

    public override void LogicUpdate(float deltaTime)
    {
        _owner.GetAbility<MovementAbility>().MoveToRight(deltaTime);
    }
}
