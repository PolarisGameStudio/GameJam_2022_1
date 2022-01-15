using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : SingletonBehaviour<TimeManager>
{
    //private TimeInfo _timeInfo;
    
    public Action OnNextDay;

    private Coroutine m_Timer;
    private bool m_IsPaused;

    private bool _dirty;
    
    private void Init()
    {
        CheckDateTime();
        
        if (m_Timer != null)
        {
            StopCoroutine(m_Timer);
        }
        
        m_Timer = StartCoroutine(CheckDateTime_Coroutine());
    }

    public void AddOnNextCallback(Action callback)
    {
        OnNextDay -= callback;
        OnNextDay += callback;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            m_IsPaused = true;

            if (m_Timer != null)
            {
                Debug.LogErrorDev($"타임매니저 일시정지");
                StopCoroutine(m_Timer);
            }
        }
        else
        {
            if (m_IsPaused)
            {
                m_IsPaused = false;

                CheckDateTime();
                m_Timer = StartCoroutine(CheckDateTime_Coroutine());
            }
        }
    }
    
    private IEnumerator CheckDateTime_Coroutine()
    {
        while (true)
        {
            // var now = ServerManager.ServerDateTimeNow;
            // var tomorrow = ServerManager.ServerDateTimeToday.AddDays(1).AddMinutes(1);
            // var diff = tomorrow - now;
            // var second = (int)diff.TotalSeconds;
            //
            // second = Mathf.Max(0, second);
            //
            // Debug.LogErrorDev($"다음날 까지 남은시간 : {second / 60}분");
            //
            // yield return new WaitForSecondsRealtime(second);
            
            CheckDateTime();
        }
    }
    
    
    private void CheckDateTime()
    {
        // if (_timeInfo == null)
        // {
        //     return;
        // }
        // else
        // {
        //     DateTime today = ServerManager.ServerDateTimeToday;
        //
        //     var diff = today - _timeInfo.LastDateTime; // 0
        //
        //     if (diff.TotalDays >= 1)
        //     {
        //         _timeInfo.LastDateTime = today;
        //
        //         _timeInfo.DayCount += 1;
        //
        //         OnNextDay?.Invoke();
        //
        //         _dirty = true;
        //         Save();
        //     }
        // }
    }
    
    private const string SaveKey = "TimeInfo";
    
    private long m_LastSaveTime;
    private Coroutine m_SaveCoroutine;
    
    public void Save(bool force = false)
    {
        if (!_dirty && !force) return;
        //
        // if (ServerManager.ServerUnixTimeInterpolation - m_LastSaveTime < 1 && !force)
        // {
        //     if (m_SaveCoroutine != null)
        //     {
        //         m_SaveCoroutine = StartCoroutine(Save_Coroutine());
        //     }
        //     
        //     return;
        // }
        //
        // m_LastSaveTime = ServerManager.ServerUnixTimeInterpolation;
        // _dirty = false;
        //
        //
        // ServerManager.Instance.Save(_timeInfo.ToNetData(SaveKey));
    }
    //
    // public void Load(TimeInfo info)
    // {
    //     // _timeInfo = info;
    //     //
    //     // if (_timeInfo == null)
    //     // {
    //     //     Debug.LogErrorDev("DailyRewardInfo: 새로 만듬");
    //     //     _timeInfo = new TimeInfo();
    //     // }
    //     //
    //     // _timeInfo.ValidCheck();
    //
    //     Init();
    // }

}
