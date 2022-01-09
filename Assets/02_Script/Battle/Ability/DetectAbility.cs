using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DetectAbility : CharacterAbility
{ 
    protected readonly List<CharacterObject> _targets = new List<CharacterObject>(4);
    public List<CharacterObject> Targets => _targets;
    public CharacterObject Target => _targets[0];

    public bool HaveTarget => _targets.Count > 0;

    public override void Init()
    {
        base.Init();
        
        _targets.Clear();
    }
    
    public override void LateProcessAbility()
    {
        Detect();
    }

    protected abstract void Detect();
}
