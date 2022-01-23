using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum UIType
{
    Popup_Equipment,
    Popup_Follower,
    Popup_World,
    Popup_Dungeon,
    Popup_Skill,
    Popup_Skill_Select,
    Popup_Follower_Select,
    Popup_Reward,
    Popup_Dungeon_Boss,
    Popup_Dungeon_Smith,
    Popup_Dungeon_Treasure,
    Popup_Gacha,
    Popup_Achieve,
    Popup_Rune,
    Popup_Option,
    Null,
}



public class UIManager : SingletonBehaviour<UIManager>
{
    private Stack<UIType> m_Stack = new Stack<UIType>(8);

    private DateTime m_LastTime;
    private bool m_Disable;

    public bool IsOpen => m_Stack.Count > 0;

    public UIType Peek => m_Stack.Count == 0 ? UIType.Null : m_Stack.Peek();

    public void Push(UIType uiType)
    {
        if (m_Stack.Count > 0 && m_Stack.Peek() == uiType) return;

        m_Stack.Push(uiType);

      // SoundManager.Instance.PlayPopupOpen();
    }

    public void SetBackButtonEnable(bool enable)
    {
        m_Disable = !enable;
    }

    public void Pop(UIType uiType)
    {
        if (m_Stack.Count == 0)
        {
            return;
        }

        UIType currentType = m_Stack.Peek();

        if (currentType != uiType)
        {
            // todo: 삭제 예정
            Debug.LogWarning(string.Format("[X] 현재: {0}, 시도: {1}", currentType, uiType));
        }
    //    SoundManager.Instance.PlayPopupClose();

        m_Stack.Pop();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && m_Disable == false)
        {
            //if (SceneManager.GetActiveScene().name != MainSceneName) return;

            var current = DateTime.Now;

            if ((current - m_LastTime).TotalMilliseconds < 300) return;

            m_LastTime = current;

            if (m_Stack.Count == 0)
            {
//                     GameEvent.Trigger(GameEventType.Quit);
//
//                     SaveManager.Instance.ForceSave();;
//
// #if UNITY_EDITOR
//                     UnityEditor.EditorApplication.isPlaying = false;
// #else
//                 Application.Quit();
// #endif
//                 });
            }
            else
            {
                CloseCurrentPopUp();
            }
        }
    }

    public void CloseCurrentPopUp()
    {
        UIType currentType = m_Stack.Peek();

        switch (currentType)
        {
         
        }
    }


    public void CloseAllPopUp()
    {
        int tryCount = 0;
        
        while (m_Stack.Count > 0)
        {
            tryCount++;

            if (tryCount > 100)
            {
                Debug.LogError("모든 팝업 닫기 실패");
                Debug.LogError(m_Stack.Peek());
                
                break;
            }
            
            CloseCurrentPopUp();
        }
    }
}
