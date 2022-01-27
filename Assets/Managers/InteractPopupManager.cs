using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPopupManager : MonoBehaviour
{
    [SerializeField]
    GameObject mInteractPopup = null;

    public static InteractPopupManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void DisplayPopup(Vector3 _pos)
    {
        mInteractPopup.transform.position = _pos;
        mInteractPopup.SetActive(true);
    }

    public void HidePopup()
    {
        mInteractPopup.SetActive(false);
    }
}
