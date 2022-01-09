using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public enum Enum_KnockbackType
{
    Model,
    Transform,
}

public class KnockbackAbility : CharacterAbility
{
    [Header("넉백 위치")] public float Value = 0.5f;
    [Header("넉백 속도")] public float Duration = 0.2f;
    [Header("제자리로 돌아오는 속도(모델 타입에서만 쓰임)")] public float ResetDuration = 0.1f;


    [Header("넉백 타입")] public Enum_KnockbackType KnockbackType = Enum_KnockbackType.Model;

    private TweenerCore<Vector3, Vector3, VectorOptions> _tweener;
    public void Knockback()
    {
        _tweener.Kill();

        // 모델 타입인 경우에는 모델만 넉백
        if (KnockbackType == Enum_KnockbackType.Model)
        {
            _tweener = _onwerObject.Model.DOLocalMove(new Vector3(Value, 0f), Duration).OnComplete(() =>
            {
                if (ResetDuration <= 0f)
                {
                    _onwerObject.Model.localPosition = Vector3.zero;
                    return;
                }
            
                _tweener = _onwerObject.Model.DOLocalMove(Vector3.zero, ResetDuration).OnComplete(() =>
                {
                    _onwerObject.Model.localPosition = Vector3.zero;

                });
            });
        }
        else
        {
            var position = _onwerObject.Position;
            position.x += Value;
            
            _tweener = _onwerObject.transform.DOLocalMove(position, Duration);

        }
    }
}
