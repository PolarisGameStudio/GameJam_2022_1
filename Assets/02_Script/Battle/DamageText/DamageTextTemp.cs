using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextTemp : MultiPoolItem
{
    [SerializeField]
    private Text _text;

    public Enum_DamageType DamageType;
    
    public DamageTextPreset Preset;
    
    protected double _damage;

    protected Vector3 _startPosition;

    private RectTransform _rectTransform;
    
    private Coroutine _corotuine;
    private TweenerCore<Vector3, Vector3, VectorOptions> _moveTweener;
    private TweenerCore<Vector3, Vector3, VectorOptions> _scaleTweener;
    private TweenerCore<Color, Color, ColorOptions> _colorTweener;
    
    public void Show(Vector3 startPosition, float height, double damage)
    {
        startPosition.y += height;
        
        if (!_text)
        {
            _text = GetComponent<Text>();
        }

        if (!_rectTransform)
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        _damage = damage;
        _text.text     = $"{_damage}";
        _text.color    = Preset.FontColor;
        _text.fontSize = Preset.FontSize;
        
        _startPosition = startPosition + Preset.Offset;

        if (Preset.RandomXEnabled)
        {
            _startPosition.x += UnityEngine.Random.Range(Preset.RandomXRange.x, Preset.RandomXRange.y);
        }

        if (Preset.RandomYEnabled)
        {
            _startPosition.y += UnityEngine.Random.Range(Preset.RandomYRange.x, Preset.RandomYRange.y);
        }

        _rectTransform.localScale = Vector3.one;

        _startPosition.z = 0;
        transform.position = _startPosition;
        transform.SetAsLastSibling();

        
        SafeSetActive(true);
        
        _corotuine = StartCoroutine(OnShow_Coroutine());
    }

    private IEnumerator OnShow_Coroutine()
    {
        if (Preset.DefaultDelay > 0)
        {
            yield return new WaitForSeconds(Preset.DefaultDelay);
        }
        
        SafeSetActive(true);

        // Move
        if (Preset.MoveEnabled)
        {
            _moveTweener = transform.DOMove(_startPosition + Preset.MoveRelativeAmount, Preset.MoveDuration)
                .SetDelay(Preset.MoveDelay);

            if (Preset.MoveUseCurve)
            {
                _moveTweener.SetEase(Preset.MoveCurve);
            }
            else
            {
                _moveTweener.SetEase(Preset.MoveEase);
            }
        }
        
        // Scale
        if (Preset.ScaleEnabled)
        {
            _scaleTweener = _rectTransform.DOScale(Preset.ScaleTo, Preset.ScaleDuration)
                .SetDelay(Preset.ScaleDelay);
           
            if (Preset.ScaleUseCurve)
            {
                _scaleTweener.SetEase(Preset.ScaleCurve);
            }
            else
            {
                _scaleTweener.SetEase(Preset.MoveEase);
            }
        }
        
        // Color
        if (Preset.ColorEnabled)
        {
           _colorTweener =  _text.DOColor(Preset.ColorTo, Preset.ColorDuration)
                .SetDelay(Preset.ColorDelay)
                .SetEase(Preset.ColorEase);
        }

        float startTime = Time.time;
        float totalDuration = Preset.Duration;
        float elapsedTime = Time.time - startTime;
        /**while (elapsedTime <= totalDuration)
        {
            Debug.LogError(elapsedTime);
            
            yield return null;
        }**/

        yield return new WaitForSeconds(totalDuration);
        
        yield return null;
        
        Hide();
    }
    
    public void Hide()
    {
        _moveTweener.Kill(true);
        _scaleTweener.Kill(true);
        _colorTweener.Kill(true);
        
        if (_corotuine != null)
        {
            StopCoroutine(_corotuine);
            _corotuine = null;
        }
        
        
        SafeSetActive(false);
    }
}
