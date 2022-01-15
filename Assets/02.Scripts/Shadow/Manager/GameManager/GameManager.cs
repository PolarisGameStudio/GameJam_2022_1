using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public enum GameState
{
    Awake,
    Sleep,
}

public class GameManager : SingletonBehaviour<GameManager>
{
    private bool m_IsMainScene = false;
    
    private const string MainSceneName = "02_Main";

    [Header("커스텀 ID: 절대 저장하지 말것")] public string CustomID;

    public bool IsMainScene => m_IsMainScene;

    private int m_SleepModeTime;
    private int m_SleepModeTimer = 0;

    private GameState m_GameState = GameState.Awake;
    public GameState GameState => m_GameState;

    public void SetToMainScene()
    {
        m_IsMainScene = true;

        StartCoroutine(CheckSleep_Coroutine());
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public void InitSleepTime()
    {
        m_SleepModeTimer = 0;
    }

    private IEnumerator CheckSleep_Coroutine()
    {
        int checkSecond = 10;
        var wait = new WaitForSecondsRealtime(checkSecond);
        
        while (true)
        {
            yield return wait;

            if (OptionManager.Instance.IsSleepOn)
            {
                m_SleepModeTimer += checkSecond;
            }

            if (m_SleepModeTimer >= m_SleepModeTime)
            {
                // if (!UI_Sleep.Instance.Opened && !GuideManager.Instance.IsGuideProgressing)
                // {
                //     UI_Sleep.Instance.Open();
                // }
            }
        }
    }

    public void Start()
    {
      //  m_SleepModeTime = SystemValue.SLEEP_MODE_TIME;
        //
        // Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0))
        //     .Subscribe(_ =>
        //     {
        //         if (m_IsMainScene)
        //         {
        //             Vector2 position = UICamera.Instance.Camera.ScreenToWorldPoint(Input.mousePosition);
        //             TouchEffectFactory.Instance.Show(position);
        //
        //             m_SleepModeTimer = 0;
        //         }
        //         else
        //         {
        //             Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //             TouchEffectFactory.Instance.Show(position);
        //         }
        //     });
    }
}
