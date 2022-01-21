using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private State _currentState;
    public State CurrentState => _currentState;

    private Coroutine _enterCoroutine;
    private Coroutine _innerEnterCoroutine;
    private bool _runEnterCoroutine = false;
    
    public void Initialize(State startingState)
    {
        _currentState = startingState;

        if (!startingState.Inited)
        {
            startingState.Init();
        }
        
        if (startingState is CoroutineState)
        {
            _runEnterCoroutine = true;

            _enterCoroutine = StartCoroutine(_EnterCoroutine());
        }
        else
        {
            _runEnterCoroutine = false;
            
            startingState.Enter();
        }
    }

    public void ChangeState(State newState)
    {
        if (_enterCoroutine != null && _runEnterCoroutine)
        {
            StopCoroutine(_enterCoroutine);
            _enterCoroutine = null;
            _runEnterCoroutine = false;
        }

        if (_innerEnterCoroutine != null)
        {
            StopCoroutine(_innerEnterCoroutine);
        }
        
        _currentState.Exit();

        Initialize(newState);
    }

    private IEnumerator _EnterCoroutine()
    {
         _innerEnterCoroutine = StartCoroutine(_currentState.Enter_Coroutine());

         yield return _innerEnterCoroutine;
         
        _enterCoroutine = null;
        _runEnterCoroutine = false;
    }
    
    public void LogicUpdate(float deltaTime)
    {
        if (_currentState == null)
        {
            Debug.Log("상태가 할당되지 않았습니다.");
            return;
        }
        
        if (_runEnterCoroutine)
        {
            return;
        }
        
        _currentState.LogicUpdate(deltaTime);
    }
}
