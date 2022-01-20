using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : SingletonBehaviour<TimeManager>
{
    private Action _onNextDay;
    private Action _onTick;

    private Coroutine _timerCoroutine;
    private Coroutine _tickerCoroutine;

    private bool _isPaused;

    private void Start()
    {
        CheckDateTime();

        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }

        if (_tickerCoroutine != null)
        {
            StopCoroutine(_tickerCoroutine);
        }

        _timerCoroutine = StartCoroutine(CheckDateTime_Coroutine());
        _tickerCoroutine = StartCoroutine(Ticket_Coroutine());
    }

    public void AddOnNextDayCallback(Action callback)
    {
        _onNextDay -= callback;
        _onNextDay += callback;
    }

    public void AddOnTickCallback(Action callback)
    {
        _onTick -= callback;
        _onTick += callback;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            _isPaused = true;

            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
            }

            if (_tickerCoroutine != null)
            {
                StopCoroutine(_tickerCoroutine);
            }
        }
        else
        {
            if (_isPaused)
            {
                _isPaused = false;

                CheckDateTime();
                _timerCoroutine = StartCoroutine(CheckDateTime_Coroutine());
                _tickerCoroutine = StartCoroutine(Ticket_Coroutine());
            }
        }
    }

    private IEnumerator CheckDateTime_Coroutine()
    {
        while (true)
        {
            var now = DateTime.Now;
            var tomorrow = DateTime.Today.AddDays(1).AddMinutes(1);
            var diff = tomorrow - now;
            var second = (int) diff.TotalSeconds;

            second = Mathf.Max(0, second);

            Debug.LogErrorDev($"다음날 까지 남은시간 : {second / 60}분");

            yield return new WaitForSecondsRealtime(second);

            CheckDateTime();
        }
    }

    private IEnumerator Ticket_Coroutine()
    {
        var second = new WaitForSecondsRealtime(1f);
        while (true)
        {
            yield return second;

            _onTick?.Invoke();
        }
    }


    private void CheckDateTime()
    {
        DateTime today = DateTime.Today;

        var diff = today - DataManager.Container.LastDateTime;

        if (diff.TotalDays >= 1)
        {
            DataManager.Container.LastDateTime = today;

            _onNextDay?.Invoke();

            DataManager.Instance.Save();
        }
    }
}