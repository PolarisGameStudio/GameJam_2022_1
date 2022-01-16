
using System.Collections.Generic;
using UnityEngine;

namespace EnhancedUI.EnhancedScroller
{
    public abstract class UI_BaseInfiniteScrollSlot<T> : EnhancedScrollerCellView
    {
        protected T m_Data;

        public virtual int GetItemCount()
        {
            return 1;
        }
        

        public virtual void Init(T data)
        {
            
        }

        public virtual void Init(List<T> data, int dataIndex)
        {
        }
    }
}