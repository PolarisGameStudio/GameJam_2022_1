using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackoutManager : MonoBehaviour , GameEventListener<BattleEvent>
{
    public static BlackoutManager Instance;

    public SpriteRenderer BehindBackground;
    public SpriteRenderer FrontBackground;

    private SpriteRenderer _targetBackground;

    public float InOutTime = 0.1f;
    public float BlackoutAlphaValue = 0.7f;

    [SerializeField] private BlackoutPreset _onStageEndPreset;

    [SerializeField] private Color _defaultColor;

    private void Awake()
    {
        Instance = this;
        this.AddGameEventListening<BattleEvent>();
    }

    public void PlayBlackOut(BlackoutPreset preset, float speed = 1f)
    {
        BehindBackground.color = _defaultColor;
        FrontBackground.color = _defaultColor;

        _targetBackground = preset.IsFront ? FrontBackground : BehindBackground;
        
        StopAllCoroutines();
        StartCoroutine(BlackOutCoroutine(preset.BlackoutDelay, preset.BlackoutValue, preset.BlackoutTime, speed , preset.BlackoutImmediately));
    }

    IEnumerator BlackOutCoroutine(float delay, float alpha, float duration, float speed = 1f , bool immediately = false)
    {
        yield return new WaitForSecondsRealtime(delay / speed);

        Color color = _targetBackground.color;

        float inTime = 0;

        if (immediately)
        {
            color.a = alpha;

            _targetBackground.color = color;
            
            inTime = int.MaxValue;
        }

        while (InOutTime > inTime)
        {
            inTime += Time.unscaledDeltaTime * speed;

            color.a = Mathf.Lerp(0, alpha, inTime / InOutTime);

            _targetBackground.color = color;

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSecondsRealtime(duration / speed);

        float outTime = 0;
        while (InOutTime > outTime)
        {
            outTime += Time.unscaledDeltaTime * speed;

            color.a = Mathf.Lerp(alpha, 0, outTime / InOutTime);

            _targetBackground.color = color;

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnStageStart()
    {
        PlayBlackOut(_onStageEndPreset);
    }

    public void OnGameEvent(BattleEvent e)
    {
        if (e.Type == Enum_BattleEventType.BattleStart)
        {
            OnStageStart();
        }
    }
}