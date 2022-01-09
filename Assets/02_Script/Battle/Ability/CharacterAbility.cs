using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAbility : MonoBehaviour
{
    protected CharacterObject _onwerObject;
    
    public virtual void Init()
    {
        _onwerObject = GetComponent<CharacterObject>();
    }
    
    public virtual void EarlyProcessAbility()
    {
        
    }
    
    public virtual void ProcessAbility(float deltaTime)
    {
        
    }

    public virtual void LateProcessAbility()
    {
        
    }
}
