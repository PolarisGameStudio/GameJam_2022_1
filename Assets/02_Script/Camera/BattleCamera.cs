using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Rendering;

public class BattleCamera : SingletonBehaviour<BattleCamera> , GameEventListener<BattleEvent>
{
    private Camera _camera;
    private ProCamera2D _proCamera2D;
    private ProCamera2DShake _proCamera2DShake;
    private ProCamera2DContentFitter _proCamera2DContentFitter;

    private Enum_CameraZoomType _cameraZoomType = Enum_CameraZoomType.Bottom;
    public Enum_CameraZoomType CameraZoomType => _cameraZoomType;
    
    [SerializeField] Transform _pivotTransform;
    [SerializeField] float _pivotThreshold;

    private const float BASE_WIDTH = 1080;

    private bool _firstResized = true;
    private float _originOrthographicSize;
    public float OriginOrthographicSize => _originOrthographicSize;
    
    public float OrthographicSize => _camera.orthographicSize;

    private TweenerCore<float, float, FloatOptions> _tweener;
    
   // [SerializeField] private ShakePreset _onStageEndPreset;
    
    protected override void Awake()
    {
        base.Awake();

        _camera           = GetComponent<Camera>();
        _proCamera2D      = GetComponent<ProCamera2D>();
        _proCamera2DShake = GetComponent<ProCamera2DShake>();
        _proCamera2DContentFitter = GetComponent<ProCamera2DContentFitter>();
        
        //_proCamera2D.OnCameraResize = OnCameraResize;
        
        _originOrthographicSize = OrthographicSize;
        
        this.AddGameEventListening<BattleEvent>();
    }
    //
    // private void OnCameraResize(Vector2 size)
    // {
    //     if (_firstResized)
    //     {
    //         //_originOrthographicSize = OrthographicSize;
    //         //Debug.LogError("카메라 사이즈: " + _originOrthographicSize);
    //         
    //         RefreshEvent.Trigger(Enum_RefreshEventType.Camera);
    //
    //         _firstResized = false;
    //     }
    // }
    
    public void SetPosition(Vector3 position)
    {
        _proCamera2D.MoveCameraInstantlyToPosition(position);
    }

    public void Shake(ShakePreset shakePreset)
    {
        // 옵션 추가
        StopShaking();
        _proCamera2DShake.Shake(shakePreset);
    }

    public void StopShaking()
    {
        _proCamera2DShake.StopShaking();
    }
    

    private void OnStateClear()
    {
       // Shake(_onStageEndPreset);
    }

    public void ResetOrthographicSize()
    {
        _tweener.Kill(complete: false);
        
        SetOrthographicSize(_originOrthographicSize);
    }

    public void ResetOrthographicSizeSmooth(float duration)
    {
        _tweener.Kill(complete: false);
        
        SetOrthographicSizeSmooth(_originOrthographicSize, duration);
    }

    public void SetOrthographicSizeSmooth(float size, float duration)
    {
        _tweener.Kill(complete: false);
        
        if (duration <= 0f)
        {
            SetOrthographicSize(size);
            return;
        }

        _tweener = DOTween.To(() => OrthographicSize, x => SetOrthographicSize(x), size, duration).OnComplete(() =>
        {
            SetOrthographicSize(size);
        });
    }
    
    
    public void SetOrthographicSize(float size)
    {
        _camera.orthographicSize = size;
        
        var currentPosition = transform.position;

        switch (_cameraZoomType)
        {
            case Enum_CameraZoomType.Bottom:
            {
                float diff = size - _originOrthographicSize;
                currentPosition.y = diff; 

                break;
            }

            case Enum_CameraZoomType.Center:
            {
                currentPosition.y = 0;
                
                break;
            }

            case Enum_CameraZoomType.Pivot:
            {
                float diff = size - _originOrthographicSize;

                if (_pivotTransform == null || Mathf.Abs(diff) < _pivotThreshold)
                {
                    currentPosition.y = 0;
                    break;
                }

                float offset = _pivotTransform.localPosition.y;
                
                currentPosition.y = offset; 
                
                break;
            }
        }
      
        SetPosition(currentPosition);
    }
    
    public void SetZoomType(Enum_CameraZoomType zoomType)
    {
        _cameraZoomType = zoomType;
        
        SetOrthographicSize(OrthographicSize);
    }

    public void OnGameEvent(BattleEvent e)
    {
        if (e.Type == Enum_BattleEventType.BattleClear)
        {
            OnStateClear();
        }
    }
    
}
