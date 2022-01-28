using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    [Header("Follow Properties")]
    // Camera will follow this game object
    [SerializeField]
    private GameObject mFollowTarget = null;

    // How smoothly the camera pans to object
    [SerializeField]
    private float mSmoothingValue = 0.3f;

    // How much dead zone before following the target
    [SerializeField]
    private Vector2 mLeeway = Vector2.zero;

    // Reference to the actual camera
    private Camera mMainCamera;

    // The different views the camera can switch between
    [Header("Camera Views")]
    [SerializeField]
    private Camera mOccludedView = null;

    [Header("Occlusion Settings")]
    // Layers to check for occlusion on (include the player layer as well)
    [SerializeField]
    private LayerMask mOcclusionLayers = 0;
    // Number of seconsd to wait before swapping views
    [SerializeField]
    private float mOccludedViewDelay = 0.5f;

    // Local state variables
    private float mOcclusionSwapDelayTime = 0f;
    private float mAngularVelocity = 0f;

    private Vector3 mCameraVelocity = Vector3.zero;

    private Vector3 mMainOffset;
    private Vector3 mOcclusionOffset;

    private Vector3 mOccludedViewPosition;
    private Quaternion mOccludedViewRotation;

    private Vector3 mMainViewPosition;
    private Quaternion mMainViewRotation;

    private Vector3 mDoorViewPosition;
    private Quaternion mDoorViewRotation;

    private bool mIsOccluded = false;

    private void Awake()
    {
        mMainCamera = Camera.main;
        if (mFollowTarget == null)
            mFollowTarget = GameManager.Instance.mPlayer;

        // Record the initial offets from the two different camera views
        mMainOffset = mMainCamera.transform.position - mFollowTarget.transform.position;
        mOcclusionOffset = mOccludedView.transform.position - mFollowTarget.transform.position;

        // Record the initial rotation from the main camera
        mMainViewRotation = mMainCamera.transform.rotation;

        // Get the occluded view's position and rotation from the dummy camera
        mOccludedViewPosition = mOccludedView.transform.position;
        mOccludedViewRotation = mOccludedView.transform.rotation;

        // Remove uneeded camera object after getting the position and rotation
        Destroy(mOccludedView.gameObject);
    }

    private void Start()
    {
        //Subscribing to when all enemies are defeated event
        GameManager.Instance.onRoomCompleted += () => StartCoroutine(RoomCompleted());

    }

    private void OnDestroy()
    {
        //Unsubscribing to event when object is destroyed
        GameManager.Instance.onRoomCompleted -= () => StartCoroutine(RoomCompleted());
    }

    private void Update()
    {
        if (mIsOccluded)
        {
            mOcclusionSwapDelayTime += Time.deltaTime;
            if (mOcclusionSwapDelayTime >= mOccludedViewDelay)
                PanToOccludedView();
            else
                PanToMainView();
        }
        else
        {
            mOcclusionSwapDelayTime = 0f;
            PanToMainView();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // Must update mainView to check for wall occlusion
        mMainViewPosition = GetNewPositionFromTarget(mMainViewPosition, mMainOffset);
        mIsOccluded = IsOccluded(mMainViewPosition);
    }

    private void PanToMainView()
    {
        // Smoothly transition between current camera view and updated main view
        mMainCamera.transform.position = Vector3.SmoothDamp(mMainCamera.transform.position,
            mMainViewPosition, ref mCameraVelocity, mSmoothingValue);
        mMainCamera.transform.rotation = SmoothRotate(mMainCamera.transform.rotation,
            mMainViewRotation);
    }

    private void PanToOccludedView()
    {
        // Update occluded camera view to current position
        mOccludedViewPosition = GetNewPositionFromTarget(mOccludedViewPosition, mOcclusionOffset);

        // Smoothly transition between current camera view and updated occluded view
        mMainCamera.transform.position = Vector3.SmoothDamp(mMainCamera.transform.position,
            mOccludedViewPosition, ref mCameraVelocity, mSmoothingValue);
        mMainCamera.transform.rotation = SmoothRotate(mMainCamera.transform.rotation,
            mOccludedViewRotation);
    }

    // Provides a smoothly interpolated rotation between the two angles
    private Quaternion SmoothRotate(Quaternion _from, Quaternion _to)
    {
        float delta = Quaternion.Angle(_from, _to);
        if (delta > 0f)
        {
            float interpolationRatio = Mathf.SmoothDampAngle(delta, 0.0f, ref mAngularVelocity, 
                mSmoothingValue);
            interpolationRatio = 1.0f - (interpolationRatio / delta);
            return Quaternion.Slerp(_from, _to, interpolationRatio);
        }

        return _from;
    }

    private bool IsOccluded(Vector3 _cameraPosition)
    {
        Vector3 targetLine = mFollowTarget.transform.position;
        targetLine.y += 2.3f;
        Physics.Linecast(_cameraPosition, targetLine, out RaycastHit hitInfo, mOcclusionLayers);

        return hitInfo.collider == null || !hitInfo.collider.CompareTag("Player");
    }

    // Updates the camera's position by the offset, taking into consideration the deadzone
    private Vector3 GetNewPositionFromTarget(Vector3 _cameraPosition, Vector3 _offset)
    {
        Vector3 delta;
        delta.x = Mathf.Abs(mFollowTarget.transform.position.x - _cameraPosition.x + _offset.x);
        delta.z = Mathf.Abs(mFollowTarget.transform.position.z - _cameraPosition.z + _offset.z);
        delta.y = 0;

        Vector3 targetPosition = mFollowTarget.transform.position + _offset;

        if (delta.x <= mLeeway.x)
            targetPosition.x = _cameraPosition.x;

        // Don't move in z direction if within deadzone
        if (delta.z <= mLeeway.y)
            targetPosition.z = _cameraPosition.z;

        return targetPosition;
    }

    IEnumerator RoomCompleted()
    {
        mDoorViewPosition = GameManager.Instance.mDoorCamPos;
        mDoorViewRotation = GameManager.Instance.mDoorCamRot;

        GameManager.Instance.StopGame();
        float time = 0f;
        Vector3 startPos = mMainCamera.transform.position;
        Quaternion startRot = mMainCamera.transform.rotation;

         while (time < 1f)
         {
            time += Time.unscaledDeltaTime;
            mMainCamera.transform.position = Vector3.Lerp(startPos, mDoorViewPosition, time);
            mMainCamera.transform.rotation = Quaternion.Lerp(startRot, mDoorViewRotation, time);
            yield return null;
         }

        yield return new WaitForSecondsRealtime(1f);

        time = 0f;
        startPos = mMainCamera.transform.position;
        startRot = mMainCamera.transform.rotation;


        while (time < 1f)
        {
            time += Time.unscaledDeltaTime;
            mMainCamera.transform.position = Vector3.Lerp(startPos, mMainViewPosition, time);
            mMainCamera.transform.rotation = Quaternion.Lerp(startRot, mDoorViewRotation, time);
            yield return null;
        }

        GameManager.Instance.UnStopGame();

    }
}
