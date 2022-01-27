using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Game Event", fileName = "New Game Event")]
public class GameEvent : ScriptableObject
{
    HashSet<GameEventListener> m_Listeners = new HashSet<GameEventListener>();

    public void Invoke()
    {
        foreach (var globalEventListener in m_Listeners)
        {
            globalEventListener.RaisedEvent();
        }
    }

    public void Register(GameEventListener gameEventListener) => m_Listeners.Add(gameEventListener);

    public void Deregister(GameEventListener gameEventListener) => m_Listeners.Remove(gameEventListener);
}

