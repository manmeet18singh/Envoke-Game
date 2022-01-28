using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    EventSystem mEventSystem = null;
    [SerializeField]
    GameObject mFirstSelected = null;

    GameObject mPrevSelected;

    private void OnEnable()
    {
        mPrevSelected = mEventSystem.currentSelectedGameObject;
        mEventSystem.SetSelectedGameObject(mFirstSelected);
    }

    private void OnDisable()
    {
        mEventSystem.SetSelectedGameObject(mPrevSelected);
    }
}
