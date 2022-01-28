using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public struct ShockWaveData
{
    #region Blast Properties
    public bool CanStun;
    public int Damage;
    public int BlastRadius;
    #endregion
}

public class ShockWaveFactory : SpellFactoryBase
{
    [SerializeField] ShockWaveData mSpellData;
    [SerializeField] private LayerMask mGroundMask = 0;
    //For the Spell Marker
    [SerializeField] SpellMarker mSpellMarker = null;

    public override bool CastSpell()
    {
        //Ray ray = GameManager.Instance.mMainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        //RaycastHit hit;

        if (mCanCast)
        { 
            ShockWaveController spell = Instantiate(mSpellPrefab, mHit.point, Quaternion.identity).GetComponent<ShockWaveController>();
            spell.Initialize(mSpellData);
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

        if (Physics.Raycast(CursorManager.Instance.ScreenToRay, out mHit, Mathf.Infinity, mGroundMask))
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
            mSpellMarker.UpdateMarker(1);
        }
    }


    #region Upgrade Functions
    public void UpgradeOne(int damage)
    {
        mSpellData.Damage = damage;
    }

    public void UpgradeTwo()
    {
        mSpellData.CanStun = true;
}

public void UpgradeThree(int radius)
    {
        mSpellData.BlastRadius = radius;
    }
    #endregion
}