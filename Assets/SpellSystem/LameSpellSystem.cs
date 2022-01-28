#region HEADER
////////////////////////////////////////////////////////////////////////////////
/// Purpose: In charge of queuing up lumes, casting/canceling spells
///
/// Author: Yashwant Patel
////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Flags]
public enum AttackFlags
{
    Unspecified = 0,
    Player = 1,
    Enemy = 2,
    Ice = 4,
    Lightning = 8,
    Fire = 16,
    Arcane = 32,
    Physical = 64
}

public enum Lume
{
    EDUR = 0,
    CINOS = 1,
    SOLEIS = 2
}

public delegate void VoidDelegate();

public class LameSpellSystem : MonoBehaviour
{
    //public delegate void LMBPressed();

    #region NamedArrayAttributes
#if UNITY_EDITOR
    [NamedArray(new string[]
    {
        "(E,E)",
        "LEAVE EMPTY",
        "(C,E)",
        "(C,C)",
        "(E,S)",
        "(C,S)",
        "(S,S)"
    })]
#endif
    [SerializeField]
    #endregion
    private SpellFactoryBase[] mSpells = null;
    [SerializeField]
    private GameObject mBasicProjectile = null;
    [SerializeField]
    private float mFireRate = .8f;
    [SerializeField]
    private float mLumeRegenTime = 10f;
    private const byte mNumLumeTypes = 3;

    private LumeQueue mLumeQueue;
    //Delegate for knowing when to cast or fire regular attack
    private VoidDelegate mLeftMousePressed;
    private bool mCanAttack = false;
    private bool mCanQueueLumes = true;
    private bool mCanCancel = false;
    private float mTimeToFire = 0f;
    private int hashedIndex;

    private bool mAnimatingAttack = false;

    private Animator mPlayerAnim = null;

#if UNITY_EDITOR
    [SerializeField]
    int mLumeAmount = 999999;
    [SerializeField]
    int mMaxAmount = 9999999;
    [SerializeField]
    bool mUseCustomValues = false;

#endif

    private void Start()
    {
        mLeftMousePressed = FireRegAttack;

        InputManager.controls.Player.QueueEdur.performed += ctx => QueueLume((sbyte)Lume.EDUR);
        InputManager.controls.Player.QueueCinos.performed += ctx => QueueLume((sbyte)Lume.CINOS);
        InputManager.controls.Player.QueueSoleis.performed += ctx => QueueLume((sbyte)Lume.SOLEIS);
        InputManager.controls.Player.CastSpell.performed += ctx => mLeftMousePressed();
        InputManager.controls.Player.CancelSpellCast.performed += ctx => CancelCast();
        InputManager.controls.Player.CastSpell.canceled += ctx => mCanAttack = false;
        SpellEvents.Instance.mLumeRegenedCallback += RegenedLume;
        GameManager.Instance.onChangeRoom += CancelCast;
        GameManager.Instance.onSaveGame += CancelCast;

        mLumeQueue = new LumeQueue(2, CreateSpell);
        LumeInventory.SetNumLumeTypeQueued(mLumeQueue.mNumLumeTypeQueued);
        mPlayerAnim = GameManager.Instance.mPlayer.GetComponent<Animator>();


        LumeInventory.ResetInventory();

        if (SavePointSystem.SavedStats)
        {
            int[] unlockedLumes = SavePointSystem.UnlockedLumes;

            for (int i = 0; i < SavePointSystem.NumLumesUnlocked; ++i)
            {
                SpellEvents.Instance.UnlockedLume(unlockedLumes[i]);
                LumeInventory.UnlockLume(unlockedLumes[i]);
                LumeInventory.SetLumeAmounts(unlockedLumes[i], SavePointSystem.CurrentLumes[unlockedLumes[i]], SavePointSystem.MaxLumes[unlockedLumes[i]]);
            }

            LumeRegeneration();
        }

        GameManager.Instance.onGamePaused += GameUnpaused;

#if UNITY_EDITOR
        if (mUseCustomValues)
        {
            for (byte i = 0; i < mNumLumeTypes; ++i)
            {
                LumeInventory.IncreaseMaxCapacity(i, mMaxAmount);
                LumeInventory.TryAddLume(i, mLumeAmount);
                LumeInventory.UnlockLume(i);
                SpellEvents.Instance.UnlockedLume(i);
            }

            mUseCustomValues = false;
        }

#endif

    }

    private void OnEnable()
    {
        mAnimatingAttack = false;
        mCanAttack = false;
    }

    private void OnDestroy()
    {
        SpellEvents.Instance.mLumeRegenedCallback -= RegenedLume;
        GameManager.Instance.onChangeRoom -= CancelCast;
        GameManager.Instance.onSaveGame -= CancelCast;
        GameManager.Instance.onGamePaused -= GameUnpaused;
    }

    private void Update()
    {
        if (mTimeToFire > 0)
        {
            mTimeToFire -= Time.deltaTime;
        }
        else if (mTimeToFire <= 0 && mCanAttack)
        {
            Vector3 firePoint = GameManager.Instance.mSpellFirePoint.position;
            Vector3 spellPos = new Vector3(firePoint.x, firePoint.y, firePoint.z);
            Instantiate(mBasicProjectile, spellPos, GameManager.Instance.mPlayer.transform.rotation).GetComponent<BaseProjectile>()
                .Move();
            mTimeToFire = mFireRate;
        }
    }

    /// <summary>
    /// Queues lume into array based on if player has enough
    /// lumes and invokes an event to notify that lume has been queued.
    /// </summary>
    /// <param name="_lume"> Which lume to queue </param>
    private void QueueLume(sbyte _lume)
    {
        if (!mCanQueueLumes)
            return;

        int lumeIndex = (_lume);
        if (LumeInventory.TryRemoveLume(lumeIndex))
        {
            mCanCancel = true;
            SpellEvents.Instance.LumeQueued(mLumeQueue.GetNumLumesQueued(), _lume);
            mLumeQueue.QueueLume(ref _lume);
            return;
        }

        NotificationManager.Instance.AddNotification($"Not Enough Lumes!", .45f);
    }

    /// <summary>
    /// Uses lumes in queue and hashes it into an index to use for
    /// casting a spell. Invokes event to notify that spell has
    /// been queued
    /// </summary>
    private void CreateSpell()
    {
        mCanQueueLumes = false;
        mLeftMousePressed = ShowChargedSpell;
        sbyte[] queue = mLumeQueue.GetQueue();
        hashedIndex = (queue[0] + queue[1]) + ((queue[0] >= queue[1]) ? queue[0] : queue[1]);
        //TODO: Change mouse cursor depending on if spell can be cast
        mSpells[hashedIndex].enabled = true;
        SpellEvents.Instance.QueuedSpell(mSpells[hashedIndex].SpellUISprite);
    }

    private Vector3 mCastingSpellLocation;
    private void ShowChargedSpell()
    {
        if (mAnimatingAttack)
            return;

        mCanCancel = false;
        mPlayerAnim.SetLayerWeight(1, 1f);
        mPlayerAnim.SetTrigger("Attack");
        mAnimatingAttack = true;

        // Immediately Cast Spell

        //if (!mSpells[hashedIndex].CastSpell())
        //{
        //    AudioManager.instance.Play("Cannot Cast");
        //    NotificationManager.Instance.AddNotification("Cannot Cast!");
        //    mCanCancel = true;
        //    mAnimatingAttack = false;
        //    return;
        //}

        //mLumeQueue.ClearQueue();
        //mSpells[hashedIndex].enabled = false;
        //SpellEvents.Instance.SpellCast();
        //LumeRegeneration();
        //mLeftMousePressed = FireRegAttack; //Changes LMB to fire reg attack
        //mCanQueueLumes = true;
        //CursorManager.Instance.SetActiveCursorAnimation(CursorType.Regular);
    }
    /// <summary>
    /// Tries to cast the queued spell. If it succeeds
    /// event is invoked to notify that spell as been cast
    /// and checs for lume regeneration.
    /// </summary>
    private void CastSpell()
    {
        if (!mSpells[hashedIndex].CastSpell())
        {
            AudioManager.instance.Play("Cannot Cast");
            NotificationManager.Instance.AddNotification("Cannot Cast!");
            mCanCancel = true;
            mAnimatingAttack = false;
            return;
        }

        mLumeQueue.ClearQueue();
        mSpells[hashedIndex].enabled = false;
        SpellEvents.Instance.SpellCast();
        LumeRegeneration();
        mLeftMousePressed = FireRegAttack; //Changes LMB to fire reg attack
        mCanQueueLumes = true;
        CursorManager.Instance.SetActiveCursorAnimation(CursorType.Regular);
    }

    /// <summary>
    /// Changes bool so player can use regular attack when
    /// presssing LMB
    /// </summary>adadadaa
    private void FireRegAttack()
    {
        if (mAnimatingAttack)
            return;

        mPlayerAnim.SetLayerWeight(1, 1f);
        mPlayerAnim.SetTrigger("QuickAttack");
        CursorManager.Instance.SetActiveCursorAnimation(CursorType.BasicAttack);
        mAnimatingAttack = true;
    }

    private void ShowQuickSpell()
    {
        mCanAttack = true;
    }

    private void ResetCanAttack()
    {
        mCanAttack = false;
        mAnimatingAttack = false;
    }
    
    private void ResetLayerWeight()
    {
        mPlayerAnim.SetLayerWeight(1, 0f);
        mAnimatingAttack = false;
        mCanCancel = true;
    }

    /// <summary>
    /// If lumes are queued the queued spell is "refunded" or
    /// queued lume is refunded and left mouse button 
    /// is set to fire basic attack.
    /// </summary>
    private void CancelCast()
    {
        if (!mCanCancel)
            return;

        mSpells[hashedIndex].enabled = false;
        if (mLumeQueue.GetNumLumesQueued() == 0)
        {
            mCanQueueLumes = true;
            return;
        }

        sbyte[] queuedLumes = mLumeQueue.GetQueue();
        int lumeIndex = 0;

        for (int i = 0; i < queuedLumes.Length; ++i)
        {
            lumeIndex = queuedLumes[i];
            if (lumeIndex > -1 && !LumeInventory.IsInventoryFull((byte)lumeIndex))
                LumeInventory.UnSafeAddLume((byte)lumeIndex);
        }

        mLumeQueue.ClearQueue();
        SpellEvents.Instance.ClearedLumes();
        mLeftMousePressed = FireRegAttack;
        mCanQueueLumes = true;
        CursorManager.Instance.SetActiveCursorAnimation(CursorType.Regular);
    }

    private void GameUnpaused(bool _paused)
    {
        if (!_paused && !mCanQueueLumes)
            Cursor.visible = false;
    }

    /// <summary>
    /// If lumes resource for lume type is empty
    /// starts regenertaing a single lume.
    /// </summary>
    private void LumeRegeneration()
    {
        bool[] emptyLumes = LumeInventory.GetEmptyLumeTypes();
        bool[] unlockedLumes = LumeInventory.GetUnlockedLumes();
        for (byte i = 0; i < emptyLumes.Length; ++i)
        {
            if (unlockedLumes[i] && emptyLumes[i])
            {
                SpellEvents.Instance.LumeRegenStarted(i, mLumeRegenTime);
            }
        }
    }

    private void RegenedLume(int _lumeIndex)
    {
        if (!LumeInventory.IsLumeNotEmpty(_lumeIndex))
            LumeInventory.UnSafeAddLume(_lumeIndex);
    }

    public void RefillLume(int _index, int mAmount)
    {
        LumeInventory.TryAddLume(_index, mAmount);
    }

    public void RefillLumes(int mAmount)
    {
        for(int i = 0; i < mNumLumeTypes; ++i)
        {
            LumeInventory.TryAddLume(i, mAmount);
        }
    }

    public void IncreaseLumeCapacity(int _index, int mAmount)
    {
        LumeInventory.UnSafeAddLume(_index, mAmount);
        LumeInventory.IncreaseMaxCapacity(_index, mAmount);
    }

    public void IncreaseLumeCapacities(int mAmount)
    {
        bool[] unlockedLumes = LumeInventory.GetUnlockedLumes();
        for (int i = 0; i < mNumLumeTypes; ++i)
        {
            if (unlockedLumes[i])
            {
                LumeInventory.UnSafeAddLume(i, mAmount);
                LumeInventory.IncreaseMaxCapacity(i, mAmount);
            }
        }
    }

/*    void RefundLumes()
    {
        for (int i = 0; i < 2; ++i)
        {
            LumeInventory.TryAddLume(mLumeCombos[hashedIndex, i]);
        }
    }*/

    //Lume queue data structure to handle how lumes are queued
    //and what happens when enough lumes are queued
    private class LumeQueue
    {
        public delegate void CombineLumes();

        //Tracks which lumes are queued
        private sbyte[] mLumeQueue;

        public byte[] mNumLumeTypeQueued { get; set; }

        //Tracks the number of lumes queued
        private byte mNumLumesQueued;

        //How many lumes it takes to combine into a spell
        private byte mNumToCombine;

        //Pointer to function for when enough lumes are queued for a spell
        private CombineLumes mCombineLumePtr;

        public LumeQueue(byte _numToCombine, CombineLumes _combineLumeFuncPtr)
        {
            mLumeQueue = new sbyte[_numToCombine];
            mNumLumeTypeQueued = new byte[LumeInventory.NumLumeTypes];
            mNumToCombine = _numToCombine;
            mCombineLumePtr = _combineLumeFuncPtr;
            mNumLumesQueued = 0;
        }

        public void ClearQueue()
        {
            for (int i = 0; i < mLumeQueue.Length; ++i)
                mLumeQueue[i] = -1;

            for (int i = 0; i < mNumLumeTypeQueued.Length; ++i)
                mNumLumeTypeQueued[i] = 0;

            mNumLumesQueued = 0;
        }

        public void QueueLume(ref sbyte _lume)
        {
            mLumeQueue[mNumLumesQueued] = _lume;
            ++mNumLumeTypeQueued[_lume];
            ++mNumLumesQueued;

            if (mNumLumesQueued >= mNumToCombine)
            {
                //LameSpellSystem.mSpellMarkerActive = true;
                mCombineLumePtr();
            }
        }

        public sbyte[] GetQueue()
        {
            return mLumeQueue;
        }

        public byte GetNumLumesQueued()
        {
            return mNumLumesQueued;
        }


    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (mUseCustomValues && Application.isPlaying)
        {
            try
            {
                for (byte i = 0; i < mNumLumeTypes; ++i)
                {
                    LumeInventory.IncreaseMaxCapacity(i, mMaxAmount);
                    LumeInventory.TryAddLume(i, mLumeAmount);
                    LumeInventory.UnlockLume(i);
                    SpellEvents.Instance.UnlockedLume(i);
                }
            }
            catch { };
        }
    }

#endif
}
