using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQueuedLumes : MonoBehaviour
{
    [SerializeField]
    float mLumeRotateSpeed = 0f;
    [SerializeField]
   int mNumLumePositions = 2;
    [SerializeField]
    GameObject[] mLumes = null;

    int[] mActiveLumes;
    Transform mPlayerTrans;
    Vector3 startPos;

    private void Awake()
    {
        startPos = transform.localPosition;
    }

    private void Start()
    {
        mActiveLumes = new int[mNumLumePositions];
        for (int i = 0; i < mActiveLumes.Length; ++i)
            mActiveLumes[i] = i + 2;
        SpellEvents.Instance.mQueuedLumeCallback += QueueLume;
        SpellEvents.Instance.mSpellCastCallback += ClearLumes;
        SpellEvents.Instance.mClearedLumesCallback += ClearLumes;
        mPlayerTrans = GameManager.Instance.mPlayer.transform;
    }

    private void OnDestroy()
    {
        SpellEvents.Instance.mQueuedLumeCallback -= QueueLume;
        SpellEvents.Instance.mSpellCastCallback -= ClearLumes;
        SpellEvents.Instance.mClearedLumesCallback -= ClearLumes;
    }

    private void LateUpdate()
    {
        transform.position = mPlayerTrans.position + startPos;
        transform.Rotate(transform.up * mLumeRotateSpeed * Time.deltaTime);
    }

    private void QueueLume(int _numLumes, int _lumeQueued)
    {
#if UNITY_WEBGL
        mLumes[mActiveLumes[_numLumes]].SetActive(false);
        mActiveLumes[_numLumes] = (_numLumes * 2) + (_lumeQueued + _numLumes);
        mLumes[mActiveLumes[_numLumes]].SetActive(true);
#else
        mLumes[mActiveLumes[_numLumes]].SetActive(false);
        mActiveLumes[_numLumes] = (_numLumes * 2) + (_lumeQueued + _numLumes);
        mLumes[mActiveLumes[_numLumes]].SetActive(true);

        if(!AudioManager.instance.IsThisPlaying("Lumes Spinning"))
        {
            AudioManager.instance.Play("Lumes Spinning");
        }
#endif
    }

    private void ClearLumes()
    {
        for(int i = 0; i < mActiveLumes.Length; ++i)
        {
            mLumes[mActiveLumes[i]].SetActive(false);
        }
        AudioManager.instance.Stop("Lumes Spinning");
    }
}
