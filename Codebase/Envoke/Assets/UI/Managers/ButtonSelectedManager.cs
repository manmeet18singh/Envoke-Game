using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectedManager : MonoBehaviour
{
    public static ButtonSelectedManager Instance;

    [SerializeField]
    EventSystem mEventSystem = null;

    private void Awake()
    {
        Instance = this;
    }

    public void SetSelectedButton(GameObject _selected)
    {
        mEventSystem.SetSelectedGameObject(_selected);
    }

}
