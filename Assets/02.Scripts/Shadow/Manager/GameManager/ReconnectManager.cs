using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReconnectManager : SingletonBehaviour<ReconnectManager>
    // , GameEventListener<FocusEvent>,
    // GameEventListener<ADSeeEvent>, GameEventListener<GameEvent>
{
    private int m_Minutes;
    public int Minutes => m_Minutes;

    private Coroutine m_CheckCoroutine;

    protected override void Awake()
    {
        base.Awake();

        Init();

        // this.AddGameEventListening<FocusEvent>();
        // this.AddGameEventListening<ADSeeEvent>();
        // this.AddGameEventListening<GameEvent>();
    }

    private void Init()
    {
      
    }

    // public void OnGameEvent(FocusEvent e)
    // {
    //     switch (e.Type)
    //     {
    //         case FocusEventType.FocusOff:
    //             PlayerManager.Instance.PlayerInfo.LastReconnectSaveTime = ServerManager.ServerUnixTime;
    //             break;
    //
    //         case FocusEventType.FocusOn:
    //             Check();
    //             break;
    //     }
    // }
    //
    // public void OnGameEvent(ADSeeEvent e)
    // {
    //     if (e.ADType == ADType.ReconnectionDouble)
    //     {
    //         GetReward(true);
    //     }
    // }

    private void Start()
    {
        Check();

        ///Save_Coroutine();
    }

    private void Check()
    {
        if (m_CheckCoroutine != null)
        {
            StopCoroutine(m_CheckCoroutine);
            m_CheckCoroutine = null;
        }

        m_CheckCoroutine = StartCoroutine(CheckReward_Coroutine());
    }

    private IEnumerator CheckReward_Coroutine()
    {
        yield return new WaitForSeconds(0.6f);

        CheckReward();
    }

    private void CheckReward()
    {
    }

    public void GetReward(bool isAdditional = false)
    {
    }

    // public void OnGameEvent(GameEvent e)
    // {
    //     // if (e.Type == GameEventType.Quit)
    //     // {
    //     //     PlayerManager.Instance.PlayerInfo.LastReconnectSaveTime = UtilCode.UnixTimeNow();
    //     // }
    // }
}