using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnhancedUI.EnhancedScroller
{
    public abstract class UI_BaseInifiniteScroll<T, S, U> : SingletonBehaviour<T>, IEnhancedScrollerDelegate
        where T : GameBehaviour where S : UI_BaseInfiniteScrollSlot<U>
    {
        [SerializeField] protected EnhancedScroller _scroll;

        [SerializeField] protected S _cellView;
        
        [SerializeField] protected List<U> _data;

        [SerializeField] protected bool _isPopup;

        protected CanvasGroup _canvasGroup;


        protected bool _open;

        protected bool _dirty;

        protected override void Awake()
        {
            base.Awake();

            _data = new List<U>();
            _scroll.Delegate = this;

            if (_isPopup)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
                _canvasGroup.alpha = 1;
            }
        }


        public virtual void Open()
        {
            SafeSetActive(true);

            _open = true;
            
            Refresh();
        }

        public virtual void Close()
        {
            SafeSetActive(false);
        }

        protected virtual void OnEnable()
        {
            _open = true;

            _canvasGroup.alpha = 1;

            Enable();
        }

        protected virtual void Enable()
        {
        }

        protected virtual void OnDisable()
        {
            _open = false;

            Disable();
        }

        protected virtual void Disable()
        {
        }

        protected bool IsEnableRefresh()
        {
            if (!_open)
            {
                return false;
            }

            if (!_dirty)
            {
                return false;
            }

            _dirty = false;

            return true;
        }

        protected abstract void Refresh();

        public abstract int GetNumberOfCells(EnhancedScroller scroller);


        public virtual float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            if (scroller.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical)
            {
                return _cellView.GetComponent<RectTransform>().rect.height;
            }
            else
            {
                return _cellView.GetComponent<RectTransform>().rect.width;
            }
        }

        public abstract EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex);
    }
}