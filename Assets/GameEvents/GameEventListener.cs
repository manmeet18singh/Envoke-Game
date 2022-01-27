using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] GameEvent m_GameEvent = null;
    [SerializeField] UnityEvent m_UnityEvent = null;

    void Awake() => m_GameEvent.Register(gameEventListener: this);

    private void OnDestroy() => m_GameEvent.Deregister(gameEventListener: this);

    public void RaisedEvent() => m_UnityEvent.Invoke();
}
