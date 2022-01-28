using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public struct CursorAnimation
{
    public CursorType cursorType;
    public Vector2 offset;
    //public Texture2D mCursor;
    public Texture2D[] textureArray;
    public float frameRate;
}

public enum CursorType
{
    Regular = 0,
    BasicAttack,
    SpellQueued,
    SpellQueuedInvalid,
}

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    [SerializeField]
    CursorAnimation[] mCursorAnimations = null;

    public Ray ScreenToRay { get; private set; }
    private CursorAnimation mCursorAnimation;
    private int mCurrentFrame = 0;
    private float mFrameTimer = 0f;
    private int mFrameCount = 0;
    private Vector3 mMouseInput = Vector3.zero;


    private void Awake()
    {
        Instance = this;
        InputManager.controls.Player.Aim.performed += ctx => mMouseInput = ctx.ReadValue<Vector2>();

    }

    void Start()
    {
        mCursorAnimation.cursorType = CursorType.SpellQueuedInvalid;
        SetActiveCursorAnimation(CursorType.Regular);
        //Cursor.SetCursor(mCursor, Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        ScreenToRay = GameManager.Instance.mMainCam.ScreenPointToRay(mMouseInput);

        if (!Cursor.visible)
            return;

        mFrameTimer -= Time.deltaTime;
        if (mFrameTimer <= 0f)
        {
            mFrameTimer += mCursorAnimation.frameRate;
            mCurrentFrame = (mCurrentFrame + 1) % mFrameCount;
            Cursor.SetCursor(mCursorAnimation.textureArray[mCurrentFrame], mCursorAnimation.offset, CursorMode.Auto);
        }
    }

    public void SetActiveCursorAnimation(CursorType _cursorType)
    {
        if (mCursorAnimation.cursorType == _cursorType)
            return;

        Cursor.visible = true;
        mCursorAnimation = mCursorAnimations[(int)_cursorType];
        Cursor.SetCursor(mCursorAnimation.textureArray[0], mCursorAnimation.offset, CursorMode.Auto);
        mCurrentFrame = 0;
        mFrameTimer = mCursorAnimation.frameRate;
        mFrameCount = mCursorAnimation.textureArray.Length;
    }

    public void EnableCursor()
    {
        Cursor.visible = true;
    }

    public void DisableCursor()
    {
        Cursor.visible = false;
    }
}
