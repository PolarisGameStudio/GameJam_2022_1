using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StageWave : GameBehaviour
{
    [SerializeField] [Header("웨이브 레벨")] private int _waveLevel;
    public int WaveLevel => _waveLevel;
    
    [SerializeField] [Header("스폰 위치들")] private List<Transform> _spawnPositions;

    public Vector3 GetSpawnPosition(int layerIndex)
    {
        if (_spawnPositions == null || _spawnPositions.Count <= layerIndex)
        {
            Debug.LogError($"{_waveLevel}레벨 웨이브에는 {layerIndex} 레이어 포지션이 없습니다.");
            return _spawnPositions[0].position;
        }
        
        return _spawnPositions[layerIndex].position;
    }

    public void SetLevel(int level)
    {
        _waveLevel = level;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        
        if (BattleManager.Instance.CurrentBattleType != Enum_BattleType.Stage)
        {
            return;
        }
        
        BattleManager.Instance.GetBattle<StageBattle>().OnWaveEnter(_waveLevel);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (BattleManager.Instance.CurrentBattleType != Enum_BattleType.Stage)
        {
            return;
        }
        
        BattleManager.Instance.GetBattle<StageBattle>().OnWaveExit(_waveLevel);
    }
}
