using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowManager : SingletonBehaviour<SlowManager> , GameEventListener<BattleEvent>
{
    [SerializeField] private SlowPreset _onStageEndPreset;

    protected override void Awake()
    {
        base.Awake();
        
        this.AddGameEventListening<BattleEvent>();
    }

    public void PlaySlowMotion(SlowPreset preset, float speed = 1f)
    {
        StopAllCoroutines();
        StartCoroutine(SlowMotionCoroutine(preset, speed));
    }

    IEnumerator SlowMotionCoroutine(SlowPreset preset, float speed = 1f)
    {
        yield return new WaitForSecondsRealtime(preset.SlowDelay / speed);

        Time.timeScale = preset.SlowValue;

        yield return new WaitForSecondsRealtime(preset.SlowTime / speed);

        if (preset.IsLerpRecovery)
        {
            float t = 0;
            while (preset.RecoveryTime > t)
            {
                t += Time.unscaledDeltaTime * speed;

                Time.timeScale = Mathf.Lerp(preset.SlowValue,1, t / preset.RecoveryTime);

                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    
    private void OnStateClear()
    {
       // PlaySlowMotion(_onStageEndPreset);
    }
    
    
    public void OnGameEvent(BattleEvent e)
    {
        if (e.Type == Enum_BattleEventType.BattleClear)
        {
            OnStateClear();
        }
    }
}
