using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHealthbar : SingletonBehaviour<UI_BossHealthbar>
{
    private Healthbar _healthbar;

    private CharacterObject _owner;

    protected override void Awake()
    {
        base.Awake();
        
        _healthbar = GetComponent<Healthbar>();
    }

    public void Show(CharacterObject owner)
    {
        _owner = owner;
        
        _healthbar.InitOwner(owner);

        SafeSetActive(true);
    }

    public void Hide()
    {
        _owner = null;

        SafeSetActive(false);
    }

    private void LateUpdate()
    {
        if (!_owner || _owner.IsDeath)
        {
            SafeSetActive(false);
            return;
        }

        _healthbar.UpdateHealth();
    }
}