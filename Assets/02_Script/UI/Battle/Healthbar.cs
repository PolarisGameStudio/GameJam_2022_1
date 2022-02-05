using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : GameBehaviour
{
    [SerializeField] private Slider _backgroundSlider;
    [SerializeField] private Slider _foregroundSlider;
    [SerializeField] private Text _ownerHealthText;

    private UI_Shaking _uiShaking;

    private CharacterObject _owner;

    private float _lastSliderValue = 0;
    private float _lastHealth = 0;

    private float _fromLastHitTime = 0f;
    private float _fromLastHitTimer = 0;

    private float _sliderMoveTime = 0.5f;
    private float _sliderMoveTimer = 0;

    private bool _isSliderWaiting;
    private bool _isSliderUpdating;

    private void Awake()
    {
        _uiShaking = GetComponent<UI_Shaking>();
    }

    public void InitOwner(CharacterObject owner)
    {
        _owner = owner;

        _lastHealth = (float) _owner.CurrentHealth;
        
        UpdateHealth();
    }

    private void ResetHealthbarUpdating()
    {
        _isSliderWaiting = false;
        _isSliderUpdating = false;

        _fromLastHitTimer = 0;
        _sliderMoveTimer = 0;
    }

    public void UpdateHealth()
    {
        float maxHealth = (float) _owner.Stat[Enum_StatType.MaxHealth];
        float currentHealth = (float) _owner.CurrentHealth;

        if (_ownerHealthText != null)
        {
            _ownerHealthText.text = $"{currentHealth.ToHealthString()}/{maxHealth.ToHealthString()}";
        }

        if (currentHealth <= 0)
        {
            ResetHealthbarUpdating();
            _backgroundSlider.value = 0;
        }

        _foregroundSlider.value = currentHealth / maxHealth;

        var hit = !currentHealth.Equals(_lastHealth);

        if (hit)
        {
            if (_uiShaking != null)
            {
                _uiShaking.Shake();
            }
            
            _fromLastHitTimer = 0;

            if (!_isSliderWaiting)
            {
                _lastSliderValue = _lastHealth / maxHealth;
                _backgroundSlider.value = _lastSliderValue;

                ResetHealthbarUpdating();
                _isSliderWaiting = true;
            }
        }

        if (_isSliderWaiting || _isSliderUpdating)
        {
            UpdateBackgroundHealthbar();
        }

        _lastHealth = currentHealth;
    }

    private void UpdateBackgroundHealthbar()
    {
        _fromLastHitTimer += Time.deltaTime;

        if (_isSliderUpdating)
        {
            float maxHealth = (float) _owner.Stat[Enum_StatType.MaxHealth];
            float currentHealth = (float) _owner.CurrentHealth;

            var currentValue = currentHealth / maxHealth;

            _sliderMoveTimer += Time.deltaTime;
            _backgroundSlider.value = Mathf.Lerp(_lastSliderValue, currentValue, _sliderMoveTimer / _sliderMoveTime);

            if (_sliderMoveTimer >= _sliderMoveTime)
            {
                ResetHealthbarUpdating();
            }
        }
        else if (_fromLastHitTimer >= _fromLastHitTime)
        {
            _isSliderWaiting = false;

            _isSliderUpdating = true;
        }
    }
}