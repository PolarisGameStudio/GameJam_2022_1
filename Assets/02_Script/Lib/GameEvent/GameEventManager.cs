using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameEventManager
{
    private static readonly Dictionary<Type, List<GameEventListenerBase>> m_Listeners;

    static GameEventManager()
    {
        m_Listeners = new Dictionary<Type, List<GameEventListenerBase>>();
    }


    public static void AddListener<GEvent>(GameEventListener<GEvent> listener) where GEvent : struct
    {
        Type eventType = typeof(GEvent);

        if (m_Listeners.ContainsKey(eventType) == false)
        {
            m_Listeners[eventType] = new List<GameEventListenerBase>();
        }

        if (IsListenerExists(eventType, listener) == false)
        {
            m_Listeners[eventType].Add(listener);
        }
    }

    public static void RemoveListener<GEvent>(GameEventListener<GEvent> listener) where GEvent : struct
    {
        Type eventType = typeof(GEvent);

        List<GameEventListenerBase> listenerList = null;

        if (m_Listeners.TryGetValue(eventType, out listenerList) == false)
        {
            // Todo: 로그를 찍자.
            return;
        }

        bool listenerFound = false;

        foreach (GameEventListenerBase l in listenerList)
        {
            if (l == listener)
            {
                listenerFound = true;

                listenerList.Remove(l);

                if (listenerList.Count == 0)
                {
                    m_Listeners.Remove(eventType);
                }

                return;
            }
        }

        if (listenerFound == false)
        {
            // Todo: 로그를 찍자.
        }
    }

    public static void TriggerGameEvent<GEvent>(GEvent gameEvent) where GEvent : struct
    {
        Type eventType = typeof(GEvent);

        List<GameEventListenerBase> listenerList = null;

        if (m_Listeners.TryGetValue(eventType, out listenerList) == false)
        {
            // Todo: 로그를 찍자.
            return;
        }

        foreach (GameEventListenerBase l in listenerList)
        {
            (l as GameEventListener<GEvent>).OnGameEvent(gameEvent);
        }
    }

    private static bool IsListenerExists(Type eventType, GameEventListenerBase listener)
    {
        List<GameEventListenerBase> listenerList = null;

        if (m_Listeners.TryGetValue(eventType, out listenerList) == false)
        {
            return false;
        }

        bool exists = false;

        foreach (GameEventListenerBase l in listenerList)
        {
            if (l == listener)
            {
                exists = true;
                break;
            }
        }

        return exists;
    }
}