using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DamageText : GameBehaviour
{
    [SerializeField] private string _animationName;
    [SerializeField] private Animator _animator;
    [SerializeField] private Text _damageText;

    private void Awake()
    {
        InitComponent();
    }

    private void InitComponent()
    {
        if (string.IsNullOrEmpty(_animationName))
        {
            Debug.LogError("DamageText.prefab의 애니메이션 이름이 비어있습니다. (채워주세요.)");
        }

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

        SetAnimationName(damageType);

        SafeSetActive(true);

        _animator.Play(_animationName, -1, 0f);
    }

    //todo: 프로토타입용 임시
    private void SetAnimationName(Enum_DamageType damageType)
    {
        switch (damageType)
        {
            case Enum_DamageType.Normal:
                _animationName = "NormalDamageTextAnim";
                break;
            case Enum_DamageType.Critical:
                _animationName = "NormalDamageTextAnim";
                break;
            case Enum_DamageType.BerserkNormal:
                _animationName = "BerserkNormalDamageTextAnim";
                break;
            case Enum_DamageType.BerserkCritical:
                _animationName = "BerserkNormalDamageTextAnim";
                break;
            case Enum_DamageType.Random:
                _animationName = "NormalDamageTextAnim";
                break;
        }
    }

    public void Hide()
    {
        SafeSetActive(false);
    }
}