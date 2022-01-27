using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class ClickToMove : MonoBehaviour
{
    #region Properties
    [Header("General Properties")]
    // How fast the player falls
    [SerializeField] private float mGravityValue = -9.81f;

    //[SerializeField] private float mBaseSpeed = 8f;

    [Header("Animation Properties")]
    [SerializeField] private float mDampTime = .1f;

    [Header("Mouse Properties")]
    // Which layers the mouse should check for collisions on
    [SerializeField] LayerMask mAimLayers;

    // Determines how far off the nav mesh samples for each movement click
    [SerializeField] float mClickAccuracy = 2;
    #endregion

    #region References
    private NavMeshAgent mPlayerNavMeshAgent;
    private Camera mPlayerCamera;
    private Animator mAnimator;
    private CharacterController mController;
    #endregion

    #region Constants
    private int moveZHash = Animator.StringToHash("MoveZ");
    private int moveXHash = Animator.StringToHash("MoveX");
    #endregion

    #region State Variables
    private bool mDestinationSet = false;
    [HideInInspector] public Vector3 mPushback;
    private Vector3 mVelocity = Vector3.zero;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        InputManager.controls.Player.ClickToMove.performed += context => MoveToMouse();
        //InputManager.controls.Player.DragToMove.performed += context => DragToMove();
        InputManager.controls.Player.CastSpell.performed += context => TurnToSpellCast();

        mPlayerCamera = GameManager.Instance.mMainCam;
        mController = GetComponent<CharacterController>();
        mPlayerNavMeshAgent = GetComponent<NavMeshAgent>();
        mAnimator = GetComponent<Animator>();
    }

    private void MoveToMouse()
    {
        //mDragging = !mDragging;

        Vector2 mouseLocation = InputManager.controls.Player.Aim.ReadValue<Vector2>();
        //Ray clickRay = mPlayerCamera.ScreenPointToRay(mouseLocation);

        if (Physics.Raycast(CursorManager.Instance.ScreenToRay, out RaycastHit hitInfo))
        {
            if (NavMesh.SamplePosition(hitInfo.point, out NavMeshHit navHitInfo, mClickAccuracy, 1))
            {
                Debug.Log("Point Found: " + navHitInfo.position);
                mPlayerNavMeshAgent.SetDestination(navHitInfo.position);
                mDestinationSet = true;
            }
            Debug.Log("Click to Move: " + navHitInfo.position);
        }
    }

    private void TurnToSpellCast()
    {
        //mPlayerNavMeshAgent.SetDestination(transform.position);
        Vector2 mouseLocation = InputManager.controls.Player.Aim.ReadValue<Vector2>();
        //Ray clickRay = mPlayerCamera.ScreenPointToRay(mouseLocation);

        if (Physics.Raycast(CursorManager.Instance.ScreenToRay, out RaycastHit hitInfo))
        {
            Vector3 lookAtVector = hitInfo.point;
            lookAtVector.y = transform.position.y;
            transform.LookAt(lookAtVector);
        }
    }

    private void Update()
    {
        // Cancel downward velocity if on the ground
        if (mController.isGrounded && mVelocity.y < 0)
            mVelocity.y = 0f;

        // This is used to display proper movement animation
        Vector3 movement = Vector3.zero;
        if (mDestinationSet)
        {
            movement = transform.position - mPlayerNavMeshAgent.destination;
            mDestinationSet = movement.magnitude > 1;
        }

        // Apply pushback
        mPushback.x = Mathf.Lerp(mPushback.x, 0, Time.deltaTime * 5);
        mPushback.y = Mathf.Lerp(mPushback.y, 0, Time.deltaTime * 200);
        mPushback.z = Mathf.Lerp(mPushback.z, 0, Time.deltaTime * 5);
        Vector3 pushback = mPushback * Time.deltaTime;
        //mController.Move(pushback);

        movement += pushback;

        // Apply gravity
        mVelocity.y += mGravityValue * Time.deltaTime;
        Vector3 velocity = mVelocity * Time.deltaTime;
        //mController.Move(velocity);

        movement += velocity;

        // Animating
        mAnimator.SetFloat(moveZHash, Vector3.Dot(movement.normalized, transform.forward), mDampTime, Time.deltaTime);
        mAnimator.SetFloat(moveXHash, Vector3.Dot(movement.normalized, transform.right), mDampTime, Time.deltaTime);
    }
}
