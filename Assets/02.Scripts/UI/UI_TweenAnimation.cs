using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum TweenAnimationType
{
    Fade,
    Scale,
    MochiScale,
    SlideFromLeft,
    SlideFromRight,
}

interface ITweenAnimation
{
    void OnEnable(UI_TweenAnimation uiTweenAnimation);
    void Close(UI_TweenAnimation uiTweenAnimation);
    void OnDisable(UI_TweenAnimation uiTweenAnimation);
}

public delegate void TweenAnimationCallback();

public class UI_TweenAnimation : GameBehaviour
{

    [Header("트윈 애니메이션 타입")] 
    public TweenAnimationType m_TweenAnimationType = TweenAnimationType.Fade;
    private ITweenAnimation m_ITweenAnimation;

    [Header("---MochiScale때는 상관 없음---")] [SerializeField]
    public Ease openEasy = Ease.OutBack;
    public Ease closeEasy = Ease.InBack;

    [Header("-----------------------------")]
    public float m_OpenTime = 0.2f;

    public float m_OpenDalay = 0f;

    public float m_CloseTime = 0.2f;

    private CanvasGroup m_CanvasGroup;
    private bool m_CloseProceeding = false;

    public TweenAnimationCallback OpenCallback = null;
    public TweenAnimationCallback CloseCallback = null;

    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();

        switch (m_TweenAnimationType)
        {
            case TweenAnimationType.Fade:
                m_ITweenAnimation = FadeAnim.instance;
                break;
            case TweenAnimationType.Scale:
                m_ITweenAnimation = ScaleAnim.instance;
                break;
            case TweenAnimationType.MochiScale:
                m_ITweenAnimation = MochiAnim.instance;
                break;
            case TweenAnimationType.SlideFromLeft:
                m_ITweenAnimation = SlideFromLeftAnim.instance;
                break;
            case TweenAnimationType.SlideFromRight:
                m_ITweenAnimation = SlideFromRightAnim.instance;
                break;
        }
    }

    private void OnEnable()
    {
        m_ITweenAnimation.OnEnable(this);
    }

    private void OnDisable()
    {
        m_ITweenAnimation.OnDisable(this);
    }

    protected virtual void OnOpenComplete()
    {
        OpenCallback?.Invoke();
    }

    public void Open()
    {
        SafeSetActive(true);
    }
    
    public void Close()
    {
        if (m_CloseProceeding) return;
        m_CloseProceeding = true;
        
        m_ITweenAnimation.Close(this);
    }

    private void OnCloseComplete()
    {
        m_CloseProceeding = false;
        
        SafeSetActive(false);

        CloseCallback?.Invoke();
    }
    
    

    /// <summary>
    /// 트윈 애니메이션을 정의하는 내부 클래스
    /// </summary>
    class FadeAnim : ITweenAnimation
    {
        public static FadeAnim instance = new FadeAnim();

        public void OnEnable(UI_TweenAnimation uiTweenAnimation)
        {
            uiTweenAnimation.m_CanvasGroup.alpha = 0f;
            uiTweenAnimation.m_CanvasGroup.DOFade(1, uiTweenAnimation.m_OpenTime)
                .SetDelay(uiTweenAnimation.m_OpenDalay)
                .OnComplete(uiTweenAnimation.OnOpenComplete)
                .SetUpdate(true);
        }

        public void Close(UI_TweenAnimation uiTweenAnimation)
        {
            uiTweenAnimation.m_CanvasGroup.alpha = 1f;
            uiTweenAnimation.m_CanvasGroup.DOFade(0, uiTweenAnimation.m_CloseTime)
                .OnComplete(uiTweenAnimation.OnCloseComplete)
                .SetUpdate(true);
        }

        public void OnDisable(UI_TweenAnimation uiTweenAnimation)
        {
            uiTweenAnimation.m_CanvasGroup.DOKill();
        }
    }

    class ScaleAnim : ITweenAnimation
    {
        public static ScaleAnim instance = new ScaleAnim();

        public void OnEnable(UI_TweenAnimation uiTweenAnimation)
        {
            uiTweenAnimation.transform.localScale = Vector3.zero;
            uiTweenAnimation.transform.DOScale(Vector3.one, uiTweenAnimation.m_OpenTime)
                .SetDelay(uiTweenAnimation.m_OpenDalay)
                .SetEase(uiTweenAnimation.openEasy)
                .OnComplete(uiTweenAnimation.OnOpenComplete)
                .SetUpdate(true);
        }

        public void Close(UI_TweenAnimation uiTweenAnimation)
        {
            uiTweenAnimation.transform.localScale = Vector3.one;
            uiTweenAnimation.transform.DOScale(Vector3.zero, uiTweenAnimation.m_CloseTime)
                .SetEase(uiTweenAnimation.closeEasy)
                .OnComplete(uiTweenAnimation.OnCloseComplete)
                .SetUpdate(true);
        }

        public void OnDisable(UI_TweenAnimation uiTweenAnimation)
        {
            uiTweenAnimation.transform.DOKill();
        }
    }

    class MochiAnim : ITweenAnimation
    {
        public static MochiAnim instance = new MochiAnim();

        public void OnEnable(UI_TweenAnimation uiTweenAnimation)
        {
            uiTweenAnimation.transform.localScale = new Vector3(0f, 0f, 1f);

            uiTweenAnimation.transform.DOScaleX(1f, uiTweenAnimation.m_OpenTime)
                .SetDelay(uiTweenAnimation.m_OpenDalay)
                .SetEase(Ease.OutCirc)
                .OnComplete(uiTweenAnimation.OnOpenComplete)
                .SetUpdate(true);

            uiTweenAnimation.transform.DOScaleY(1f, uiTweenAnimation.m_OpenTime)
                .SetDelay(uiTweenAnimation.m_OpenDalay)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);
        }

        public void Close(UI_TweenAnimation uiTweenAnimation)
        {
            uiTweenAnimation.transform.localScale = Vector3.one;
            uiTweenAnimation.transform.DOScale(Vector3.zero, uiTweenAnimation.m_CloseTime)
                .SetEase(uiTweenAnimation.closeEasy)
                .OnComplete(uiTweenAnimation.OnCloseComplete)
                .SetUpdate(true);
        }

        public void OnDisable(UI_TweenAnimation uiTweenAnimation)
        {
            uiTweenAnimation.transform.DOKill();
        }
    }

    class SlideFromLeftAnim : ITweenAnimation
    {
        public static SlideFromLeftAnim instance = new SlideFromLeftAnim();

        public void OnEnable(UI_TweenAnimation uiTweenAnimation)
        {
            RectTransform rectTransform = uiTweenAnimation.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector3(Screen.width * -1, rectTransform.anchoredPosition.y, 0);

            rectTransform.DOAnchorPos(new Vector3(0, rectTransform.anchoredPosition.y, 0), uiTweenAnimation.m_OpenTime)
                .SetDelay(uiTweenAnimation.m_OpenDalay)
                .SetEase(Ease.OutCirc)
                .OnComplete(uiTweenAnimation.OnOpenComplete)
                .SetUpdate(true);
        }

        public void Close(UI_TweenAnimation uiTweenAnimation)
        {
            RectTransform rectTransform = uiTweenAnimation.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector3(0, rectTransform.anchoredPosition.y, 0);

            rectTransform.DOAnchorPos(new Vector3(Screen.width * -1, rectTransform.anchoredPosition.y, 0), uiTweenAnimation.m_CloseTime)
                .SetEase(uiTweenAnimation.closeEasy)
                .OnComplete(uiTweenAnimation.OnCloseComplete)
                .SetUpdate(true);
        }

        public void OnDisable(UI_TweenAnimation uiTweenAnimation)
        {
            uiTweenAnimation.transform.DOKill();
        }
    }
    class SlideFromRightAnim : ITweenAnimation
    {
        public static SlideFromRightAnim instance = new SlideFromRightAnim();

        public void OnEnable(UI_TweenAnimation uiTweenAnimation)
        {
            RectTransform rectTransform = uiTweenAnimation.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector3(Screen.width, rectTransform.anchoredPosition.y, 0);

            rectTransform.DOAnchorPos(new Vector3(0, rectTransform.anchoredPosition.y, 0), uiTweenAnimation.m_OpenTime)
                .SetDelay(uiTweenAnimation.m_OpenDalay)
                .SetEase(Ease.OutCirc)
                .OnComplete(uiTweenAnimation.OnOpenComplete)
                .SetUpdate(true);
        }

        public void Close(UI_TweenAnimation uiTweenAnimation)
        {
            RectTransform rectTransform = uiTweenAnimation.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector3(0, rectTransform.anchoredPosition.y, 0);

            rectTransform.DOAnchorPos(new Vector3(Screen.width, rectTransform.anchoredPosition.y, 0), uiTweenAnimation.m_CloseTime)
                .SetEase(uiTweenAnimation.closeEasy)
                .OnComplete(uiTweenAnimation.OnCloseComplete)
                .SetUpdate(true);
        }

        public void OnDisable(UI_TweenAnimation uiTweenAnimation)
        {
            RectTransform rectTransform = uiTweenAnimation.GetComponent<RectTransform>();
            rectTransform.DOKill();
        }
    }
}