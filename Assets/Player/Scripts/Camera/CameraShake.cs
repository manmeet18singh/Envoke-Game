#region CameraShake
////////////////////////////////////////////////////////////////////////////////
/// Purpose: Used to dynamically shake the camera via a static funtion call
/// that instantiates a new instance of this class and cleanly destroys itself
/// after the shake is over (if enabled).
///
/// Author: Matthew Steinhardt
////////////////////////////////////////////////////////////////////////////////
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    public Vector3 mAmount = new Vector3(1f, 1f, 0);
    [SerializeField]
    public float mDuration = 1f;
    [SerializeField]
    public float mSpeed = 10;
    [SerializeField]
    public float mPerlinMagnitude = 2;
    [SerializeField]
    public AnimationCurve mAnimCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    protected Camera mCamera;

    // State Variables
    protected float mTimeRemaining = 0;
    protected Vector3 mLastPos;
    protected Vector3 mNextPos;
    protected float mLastFoV;
    protected float mNextFoV;
    protected bool mDestroyAfterPlay;

    /// ShakeOneshot
    /// 
    /// <summary>
    /// Shakes the given camera for the given duration. By making this a static function
    /// you can call this from any script without having to worry about instantiating
    /// and keeping track of the shaking logic.
    /// </summary>
    /// <param name="_camera">The camera to shake. Will assume mainCamera if null.</param>
    /// <param name="_duration">How long to shake camera.</param>
    /// <param name="_speed">How fast to shake camera.</param>
    /// <param name="_amount">How hard to shake camera in each direction.</param>
    /// <param name="_AnimCurve">Interpolator to use when shaking.</param>
    public static void ShakeOneshot(Camera _camera = null, float _duration = 1f, float _speed = 10f, Vector3? _amount = null,
    AnimationCurve _AnimCurve = null)
    {
        var shakeInstance = ((_camera == null ? GameManager.Instance.mMainCam : _camera).gameObject.AddComponent<CameraShake>());
        shakeInstance.mDuration = _duration;
        shakeInstance.mSpeed = _speed;

        if (_amount != null)
            shakeInstance.mAmount = (Vector3)_amount;
        if (_AnimCurve != null)
            shakeInstance.mAnimCurve = _AnimCurve;
        shakeInstance.mDestroyAfterPlay = true;
        shakeInstance.Shake();
    }

    private void Awake()
    {
        mCamera = GetComponent<Camera>();
    }

    public void Shake()
    {
        Reset();
        mTimeRemaining = mDuration;
    }


    private void Reset()
    {
        mCamera.transform.Translate(-mLastPos);
        mCamera.fieldOfView -= mLastFoV;

        mLastPos = Vector3.zero;
        mNextPos = Vector3.zero;
        mLastFoV = 0f;
        mNextFoV = 0f;
    }

    private void LateUpdate()
    {
        if (mTimeRemaining > 0)
        {
            mTimeRemaining -= Time.deltaTime;
            if (mTimeRemaining > 0)
            {
                GenerateShakeEffect();

                mCamera.fieldOfView += (mNextFoV - mLastFoV);
                mCamera.transform.Translate(mNextPos - mLastPos);

                mLastPos = mNextPos;
                mLastFoV = mNextFoV;
            }
            else
            {
                Reset();
                if (mDestroyAfterPlay)
                    Destroy(this);
            }
        }
    }

    private void GenerateShakeEffect()
    {
        mNextPos =
            (Mathf.PerlinNoise(mTimeRemaining * mSpeed, mTimeRemaining * mSpeed * mPerlinMagnitude) - 0.5f)
                * mAmount.x * transform.right * mAnimCurve.Evaluate(1f - mTimeRemaining / mDuration)
            +
            (Mathf.PerlinNoise(mTimeRemaining * mSpeed * mPerlinMagnitude, mTimeRemaining * mSpeed) - 0.5f)
                * mAmount.y * transform.up * mAnimCurve.Evaluate(1f - mTimeRemaining / mDuration);

        mNextFoV = (Mathf.PerlinNoise(mTimeRemaining * mSpeed * mPerlinMagnitude, mTimeRemaining * mSpeed * mPerlinMagnitude) - 0.5f)
                * mAmount.z * mAnimCurve.Evaluate(1f - mTimeRemaining / mDuration);
    }
}
