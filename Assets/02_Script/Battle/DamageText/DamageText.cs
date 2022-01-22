using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DamageText : MultiPoolItem
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Text _damageText;
    public Enum_DamageType Type;

    private void Awake()
    {
        InitComponent();
    }

    private void InitComponent()
    {
        if (!_animator)
        {
            _animator = GetComponent<Animator>();
        }

        if (!_damageText)
        {
            _damageText = GetComponentInChildren<Text>();
        }
    }

    public void Show(Vector3 startPosition, float height, float width, double damage, Enum_DamageType damageType , bool useRandomPosition)
    {
        startPosition.z = 0;
        
        if (!useRandomPosition)
        {
            startPosition.y += height;
        }
        else
        {    
            var randomHeight = Random.Range(height / 2f, height);

            var thirdWidth = width / 3;
            var randomWidth = Random.Range(-thirdWidth, thirdWidth);

            startPosition += new Vector3(randomWidth, randomHeight, 0f);
        }

    
        InitComponent();

        transform.position = startPosition;
        transform.SetAsLastSibling();

        _damageText.text = damage.ToDamageString();

        SafeSetActive(true);
    }

    public void Hide()
    {
        SafeSetActive(false);
    }
}