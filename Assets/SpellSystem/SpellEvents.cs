using System;
using UnityEngine;
using UnityEngine.UI;

public class SpellEvents : MonoBehaviour
{
    public static SpellEvents Instance { get; private set; }

    public event Action<Sprite> mQueuedSpellCallback;
    public event Action<int, int> mQueuedLumeCallback;
    public event Action mClearedLumesCallback;
    public event Action mSpellCastCallback;
    public event Action<int, int, int> mLumeAmountChangedCallback;
    public event Action<int, float> mLumeRegenStartedCallback;
    public event Action<int> mLumeRegenStoppedCallback;
    public event Action<int> mLumeRegenedCallback;
    public event Action<int> mUnlockLumeCallback;
    public event Action<int> mLockLumeCallback;
    public event Action<int> mConfirmLumeUnlockCallback;
    public event Action mConfirmedLumeUnlockCallback;

    private void Awake()
    {
        Instance = this;
    }

    public void QueuedSpell(Sprite _spellSprite)
    {
        mQueuedSpellCallback?.Invoke(_spellSprite);
    }


    public void LumeQueued(int _numLumes, int _lumeQueued)
    {
        mQueuedLumeCallback?.Invoke(_numLumes, _lumeQueued);
    }

    public void ClearedLumes()
    {
        mClearedLumesCallback?.Invoke();
    }

    public void SpellCast()
    {
        mSpellCastCallback?.Invoke();
    }

    public void LumeAmountChanged(int _lumeIndex, int _currentAmount, int _maxCapacity)
    {
        mLumeAmountChangedCallback?.Invoke(_lumeIndex, _currentAmount, _maxCapacity);
    }

    public void LumeRegenStarted(int _lumeIndex, float _duration)
    {
        mLumeRegenStartedCallback?.Invoke(_lumeIndex, _duration);
    }

    public void LumeRegenStopped(int _lumeIndex)
    {
        mLumeRegenStoppedCallback?.Invoke(_lumeIndex);
    }

    public void LumeRegened(int _lumeIndex)
    {
        mLumeRegenedCallback?.Invoke(_lumeIndex);
    }

    public void LockedLume(int _lume)
    {
        mLockLumeCallback?.Invoke(_lume);
    }

    public void UnlockedLume(int _lume)
    {
        mUnlockLumeCallback?.Invoke(_lume);
    }

    public void ConfirmLumeUnlock(int _lume)
    {
        mConfirmLumeUnlockCallback?.Invoke(_lume);
    }

    public void ConfirmedLumeUnlock()
    {
        mConfirmedLumeUnlockCallback?.Invoke();
    }
}
