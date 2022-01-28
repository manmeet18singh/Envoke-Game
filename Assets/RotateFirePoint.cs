using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateFirePoint : MonoBehaviour
{
    private LayerMask mAimLayerMask;
    private Camera mCamera;

    //private Vector2 mMousePosition;

    private void Awake()
    {
        mCamera = GameManager.Instance.mMainCam;

        // This will need to change unless there is always going to be a Ground Layer
        mAimLayerMask = GameManager.Instance.mPlayer.GetComponent<PlayerMovement>().GetLayer;

        //InputManager.controls.Player.Aim.performed += ctx => mMousePosition = ctx.ReadValue<Vector2>();
    }

    private void LateUpdate()
    {
        //Ray ray = mCamera.ScreenPointToRay(mMousePosition);

        if (Physics.Raycast(CursorManager.Instance.ScreenToRay, out var hitInfo, Mathf.Infinity, mAimLayerMask))
        {
            var direction = hitInfo.point - transform.position;
            direction.y = 0f;
            direction.Normalize();
            transform.forward = direction;
        }
    }
    
/*    private void UpdateLookPosition(InputAction.CallbackContext _context)
        {
            mMousePosition = _context.ReadValue<Vector2>();
        }*/
}
