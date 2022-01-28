using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputMaster controls;
    //private static InputActionMap mPrevActionMap;

    private void Awake()
    {
        controls = new InputMaster();
        //mPrevActionMap = controls.UI;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

/*    public static void ToggleActionMap(InputActionMap _actionMap)
    {
        mPrevActionMap.Disable();
        _actionMap.Enable();
        mPrevActionMap = _actionMap;
    }*/
}

