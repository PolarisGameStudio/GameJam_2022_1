using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowAbility : CharacterAbility
{
    [Header("그림자 트랜스폼")] [SerializeField] private Transform _shadowTransform;

    [SerializeField] private float _width = 2f;
    [SerializeField] private float _height = 0.5f;

    public void SetSize(float width)
    {
        if (!_shadowTransform)
        {
            return;
        }

        // Todo: 그림자 나온 후 조정해봐야함
        //_shadowTransform.localScale = new Vector3(width, width / 2f, 1f);

        //todo: 프로토타입은 몬스터 1종이므로 수치 고정 
        // 정식버전에서는 몬스터별 그림자수치 or 스파인파일에 넣는 식으로 처리
        _shadowTransform.localScale = new Vector3(_width, _height, 1f);
    }
}