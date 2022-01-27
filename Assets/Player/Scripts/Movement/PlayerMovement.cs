using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent( typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth mHealth;

    [Header("Mouse Aiming Properties")]
    [SerializeField] public LayerMask mAimLayers = 0;
    [SerializeField] private Vector3 mAimRotationOffset = new Vector3(0,0,0);

    [Header("Movement Properties")]
    // How fast the player falls
    [SerializeField] private float mGravityValue = -9.81f;
    [SerializeField] public float moveSpeed = 8f;
    [SerializeField] private float dampTime = .1f;

    private CharacterController mController;
    private InputMaster controls;

    private Vector2 mouseInput;
    private Vector3 movement;
    [SerializeField]
    private Animator mAnimator;

    [SerializeField] public float inputX;
    [SerializeField] public float inputZ;

    private int moveXHash;
    private int moveZHash;

    // State variables
    public Vector3 mPushback;
    private Vector3 mVelocity = Vector3.zero;
    private Vector3 mKnockback = Vector3.zero;
    private float mBaseSpeed = 0f;

    public LayerMask GetLayer { get => mAimLayers; }

    private void Awake()
    {
        //mAnimator = GetComponent<Animator>();
        controls = InputManager.controls;
        mController = GetComponent<CharacterController>();
        mBaseSpeed = moveSpeed;
       
        controls.Player.Move_X.performed += ctx => inputX = ctx.ReadValue<float>();
        controls.Player.Move_Z.performed += ctx => inputZ = ctx.ReadValue<float>();
        controls.Player.Aim.performed += ctx => mouseInput = ctx.ReadValue<Vector2>();

    }

    private void Start()
    {
        moveXHash = Animator.StringToHash("MoveX");
        moveZHash = Animator.StringToHash("MoveZ");
    }

    private void OnEnable()
    {
        moveSpeed = mBaseSpeed;
    }

    private void Update()
    {
        if (mHealth != null && mHealth.CurrentHealth <= 0)
            return;

        //FOR KNOCKBACK:
        if (mKnockback.magnitude > 0.2F)
        {
            Debug.Log("KNOCKING BACK");
            mController.Move(mKnockback * Time.deltaTime);
            mKnockback = Vector3.Lerp(mKnockback, Vector3.zero, 5 * Time.deltaTime);

        }
        // consumes the impact energy each cycle:


        // Cancel downward velocity if on the ground
        if (mController.isGrounded && mVelocity.y < 0)
            mVelocity.y = 0f;

        movement = new Vector3(-inputZ, 0f, inputX).normalized;

        AimTowardMouse();

        // Apply pushback
        mPushback.x = Mathf.Lerp(mPushback.x, 0, Time.deltaTime * 5);
        mPushback.y = Mathf.Lerp(mPushback.y, 0, Time.deltaTime * 200);
        mPushback.z = Mathf.Lerp(mPushback.z, 0, Time.deltaTime * 5);
        mController.Move(mPushback * Time.deltaTime);

        //movement = transform.TransformDirection(movement);

        mController.Move(movement * moveSpeed * Time.deltaTime);

        // Apply gravity
        mVelocity.y += mGravityValue * Time.deltaTime;
        mController.Move(mVelocity * Time.deltaTime);

#if UNITY_EDITOR
        //Debug.Log("Move in: " + _input);
#endif
        Animate();
    }

    private void Animate()
    {
        float velocityZ = Vector3.Dot(movement.normalized, transform.forward);
        float velocityX = Vector3.Dot(movement.normalized, transform.right);

        mAnimator.SetFloat(moveXHash, velocityX, dampTime, Time.deltaTime);
        mAnimator.SetFloat(moveZHash, velocityZ, dampTime, Time.deltaTime);

        if (velocityZ == 0 || velocityX == 0)
            AudioManager.instance.Stop("Walking");
        else { 
            if(!AudioManager.instance.IsThisPlaying("Walking"))
                AudioManager.instance.Play("Walking");
        }
    }

    private void AimTowardMouse()
    {
        //Ray ray = Camera.main.ScreenPointToRay(mouseInput);

        if (Physics.Raycast(CursorManager.Instance.ScreenToRay, out var hitInfo, Mathf.Infinity, mAimLayers))
        {
            transform.LookAt(new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z));
            transform.Rotate(mAimRotationOffset);
        }
    }

    public void Teleport(Vector3 _location)
    {
        CharacterController characterController = gameObject.GetComponent<CharacterController>();
        characterController.enabled = false;
        transform.position = _location;
        characterController.enabled = true;
    }

    public void AddKnockback(Vector3 dir, float force)
    {
        Debug.Log("FORCE: " + force);
        dir.Normalize();
        //if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        dir.y = 0;
        mKnockback += dir.normalized * force;
    }
}
