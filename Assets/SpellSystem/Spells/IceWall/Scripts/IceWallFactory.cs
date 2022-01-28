#region HEADER
////////////////////////////////////////////////////////////////////////////////
/// Purpose: Icewall (1,2) used to cast ice wall where player's mouse and wall is
/// parallel to player.
///
/// Author: Yashwant Patel
////////////////////////////////////////////////////////////////////////////////
#endregion
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public struct IceWallData
{
    public int LifeTime;
    public int MaxHealth;
    public int ExplosionDamage;
    public float Radius;
    public bool CanExplode;
    public LayerMask ExplodeMask;
}


public class IceWallFactory : SpellFactoryBase
{
    [SerializeField]
    LayerMask hitMask = 0;
    [SerializeField]
    IceWallData mSpellData;
    //For the Spell Marker
    [SerializeField] 
    SpellMarker mSpellMarker = null;

    private LayerMask mIndicatorLayerMask;

    private void Awake()
    {
        mIndicatorLayerMask = LayerMask.GetMask("Ground");
    }

    public override bool CastSpell()
    {
        //Ray ray = GameManager.Instance.mMainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        //RaycastHit hit;

        if (mCanCast)
        {
            IceWallHealth mSpell = Instantiate(mSpellPrefab, mHit.point, GameManager.Instance.mPlayer.transform.rotation).GetComponent<IceWallHealth>();
            mSpell.Initialize(mSpellData);
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
       // UpdateSpellMarker();
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

/*    /// <summary>
    /// Draws a marker where the spell will cast
    /// </summary>
    private void UpdateSpellMarker()
    {
        RaycastHit hit;

        if (Physics.Raycast(CursorManager.Instance.ScreenToRay, out hit, Mathf.Infinity, mIndicatorLayerMask))
        {
            mSpellMarker.transform.position = new Vector3(hit.point.x, hit.point.y + .25f, hit.point.z);
        }
    }*/

    public void HealthUpgrade(int _health)
    {
        mSpellData.MaxHealth = _health;
    }

    public void SlowerMeltUpgrade(int _lifeTime)
    {
        mSpellData.LifeTime = _lifeTime;
    }

    public void ExplosionUpgrade()
    {
        mSpellData.CanExplode = true;
    }
}
