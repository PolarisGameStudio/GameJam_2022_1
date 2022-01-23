using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Popup_Reward : UI_BaseContent<UI_Popup_Reward,UI_Popup_Reward_Slot>
{
    private Queue<List<Reward>> _waitingQueue = new Queue<List<Reward>>();

    protected override void Awake()
    {
        base.Awake();
        SafeSetActive(false);
    }

    public void Open(List<Reward> rewards)
    {
        if (isActiveAndEnabled)
        {
            _waitingQueue.Enqueue(rewards);
            return;
        }
        
        UIManager.Instance.Push(UIType.Popup_Reward);
        
        SafeSetActive(true);

        if (rewards.Count - m_SlotList.Count > 0)
        {
            Expand(rewards.Count - m_SlotList.Count);
        }

        for (int i = 0; i < m_SlotList.Count; i++)
        {
            if (i >= rewards.Count)
            {
                m_SlotList[i].SafeSetActive(false);
                continue;
            }

            m_SlotList[i].SafeSetActive(true);
            m_SlotList[i].Init(rewards[i]);
        }
    }

    public override void Close()
    {
        UIManager.Instance.Pop(UIType.Popup_Reward);
        
        SafeSetActive(false);

        if (_waitingQueue.Count != 0)
        {
            var rewards = _waitingQueue.Dequeue();

            Open(rewards);
            
            return;
        }
        
        _waitingQueue.Clear();
    }

    protected override void Refresh(){}
}
