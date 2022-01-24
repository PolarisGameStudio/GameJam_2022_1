using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarObject : MultiPoolItem
{
    public Enum_CharacterType CharacterType;
    
    private Healthbar _healthbar;

    private CharacterObject _owner;

    private float _height;

    private float _prevPositionX = -1000;

    private void Awake()
    {
        _healthbar = GetComponent<Healthbar>();
    }

    public void Show(CharacterObject owner, float height)
    {
        _owner = owner;

        _height = height;

        _prevPositionX = -1000;

        _healthbar.InitOwner(owner);
        
        UpdatePosition();

        SafeSetActive(true);
    }

    public void LateUpdate()
    {
        if (!_owner || _owner.IsDeath)
        {
            SafeSetActive(false);
            return;
        }

        _healthbar.UpdateHealth();
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        var ownerPosition = _owner.Model.position;

        ownerPosition.y -= 3f;
        ownerPosition.z = 0;

        transform.position = ownerPosition;
    }
}