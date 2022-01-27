using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class PlayerMovementClassic : MonoBehaviour
{
    [SerializeField]
    private LayerMask mAimLayerMask = 0;
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private float dampTime = .1f;

    private InputMaster controls;

    private Vector2 mouseInput;
    private Vector3 worldMousePosition;
    private Vector3 movement;
    private Animator mAnimator;

    private float inputX;
    private float inputZ;





    private void Awake()
    {

        mAnimator = GetComponent<Animator>();
        controls = InputManager.controls;

        controls.Player.Move_X.performed += ctx => inputX = ctx.ReadValue<float>();
        controls.Player.Move_Z.performed += ctx => inputZ = ctx.ReadValue<float>();
        controls.Player.Aim.performed += ctx => mouseInput = ctx.ReadValue<Vector2>();

    }

    private void Update()
    {
        worldMousePosition = new Vector3(mouseInput.x, 0f, mouseInput.y);

        AimTowardMouse();

        movement = new Vector3(inputX, 0f, inputZ).normalized;

        Move(movement);
        Animate();
    }

    private void Move(Vector3 _input)
    {
        if (movement.magnitude > 0)
        {
            movement.Normalize();
            movement *= moveSpeed * Time.deltaTime;
        }
        else
        {
            movement = Vector3.zero;
        }

#if UNITY_EDITOR
        Debug.Log("Move in: " + _input);
#endif
    }

    private void Animate()
    {
        float velocityZ = Vector3.Dot(movement.normalized, transform.forward);
        float velocityX = Vector3.Dot(movement.normalized, transform.right);

        mAnimator.SetFloat("MoveX", velocityX, dampTime, Time.deltaTime);
        mAnimator.SetFloat("MoveZ", velocityZ, dampTime, Time.deltaTime);
    }

    private void AimTowardMouse()
    {
        //Ray ray = Camera.main.ScreenPointToRay(mouseInput);

        if (Physics.Raycast(CursorManager.Instance.ScreenToRay, out var hitInfo, Mathf.Infinity, mAimLayerMask))
        {
            var direction = hitInfo.point - transform.position;
            direction.y = 0f;
            direction.Normalize();
            transform.forward = direction;
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
