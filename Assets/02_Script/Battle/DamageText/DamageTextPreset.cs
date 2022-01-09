using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core.Easing;

[System.Serializable]
[CreateAssetMenu(menuName = "Presets/DamageText Preset")]
public class DamageTextPreset : ScriptableObject
{
    [Header("폰트")]
    public Font Font;

    [Header("폰트 사이즈")]
    public int FontSize;

    [Header("폰트 컬러")] 
    public Color FontColor;

    [Header("기본 딜레이")]
    public float DefaultDelay = 0f;
    
    [Header("기본 지속 시간")]
    public float DefaultDuration = 1f;

    [Header("위치 오프셋")]
    public Vector3 Offset = Vector3.zero;
    
    // Random
    [Header("랜덤 위치 생성")]
    public bool RandomXEnabled;
    public Vector2 RandomXRange;
    public bool RandomYEnabled;
    public Vector2 RandomYRange;
    
    // Move
    [Header("Move(상대적)")]
    public bool MoveEnabled;
    public Vector3 MoveRelativeAmount;
    public float MoveDuration;
    public float MoveDelay;
    public Ease MoveEase = Ease.OutQuad;
    public bool MoveUseCurve;
    public AnimationCurve MoveCurve;
    
    // Scale
    [Header("Scale")]
    public bool ScaleEnabled;
    public float ScaleTo;
    public float ScaleDelay;
    public float ScaleDuration;
    public Ease ScaleEase = Ease.OutQuad;
    public bool ScaleUseCurve;
    public AnimationCurve ScaleCurve;
    
    // Color
    [Header("Color")]
    public bool ColorEnabled;
    public Color ColorTo;
    public float ColorDelay;
    public float ColorDuration;
    public Ease ColorEase = Ease.OutQuad;

    public float Duration => Mathf.Max
    (
        DefaultDuration,
            MoveEnabled  ? MoveDuration   : 0f,
            ScaleEnabled ? ScaleDuration : 0f,
            ColorEnabled ? ColorDuration : 0f
    );
}
