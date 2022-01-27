using UnityEngine.InputSystem;
using UnityEngine;

[System.Serializable]
public struct HailData
{
    public int DamagePerTick;
    public float SpellDuration;
    public int TimeTillImmobile;
    public float MinMoveSpeed;
    public float FreezeDuration;
    public bool CanFreeze;
}

public class HailFactory : SpellFactoryBase
{
    [SerializeField]
    LayerMask hitMask = 0;
    [SerializeField]
    HailData mSpellData;
    //For the Spell Marker
    [SerializeField] SpellMarker mSpellMarker = null;

    public override bool CastSpell()
    {
        //Ray ray = GameManager.Instance.mMainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        //RaycastHit hit;

        if (mCanCast)
        {
            HailController hail = Instantiate(mSpellPrefab, mHit.point, Quaternion.identity).GetComponent<HailController>();
            hail.Initialize(mSpellData);
        }

        return mCanCast;
    }

    private void OnDisable()
    {
        CursorManager.Instance.EnableCursor();
        mSpellMarker.DisableMarker();
    }

    private void OnEnable()
    {
        CursorManager.Instance.DisableCursor();
        mSpellMarker.EnableMarker();
    }

    private void Update()
    {

        if (Physics.Raycast(CursorManager.Instance.ScreenToRay, out mHit, Mathf.Infinity, hitMask))
        {
            if (mCanCast)
                return;

            mCanCast = true;
            mSpellMarker.UpdateMarker(1);
        }
        else
        {
            if (!mCanCast)
                return;

            mCanCast = false;
            mSpellMarker.UpdateMarker(0);
        }
    }

    /// <summary>
    /// Draws a marker where the spell will cast
    /// </summary>

    public void FreezeUpgrade()
    {
        mSpellData.CanFreeze = true;
    }

    public void HailSlowUpgrade(int _timeTillImmobile)
    {
        mSpellData.TimeTillImmobile = _timeTillImmobile;
    }

    public void HailDamageIncrease(int _damagePerTick)
    {
        mSpellData.DamagePerTick = _damagePerTick;
    }
}
