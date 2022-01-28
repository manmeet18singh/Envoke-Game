using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCombosUI : MonoBehaviour
{
    [SerializeField]
    GameObject mComboPanel = null;

    private void Awake()
    {
        InputManager.controls.UI.Tab.performed += ctx => mComboPanel.SetActive(true);
        InputManager.controls.UI.Tab.canceled += ctx => mComboPanel.SetActive(false);
    }
}
