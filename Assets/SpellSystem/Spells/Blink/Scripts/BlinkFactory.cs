using UnityEngine.InputSystem;
using UnityEngine;

[System.Serializable]
public struct BlinkData
{
    public float MaxBlinkDistance;
    public float SpellDuration;
    public int DamagePerTick;
    public float DamageRadius;
}

public class BlinkFactory : SpellFactoryBase
{
    [SerializeField]
    LayerMask mTargetHitMask = 0;
    [SerializeField]
    Vector3 mLineCastOffset = Vector3.zero;
    [SerializeField]
    BlinkData mSpellData;
    //For the Spell Marker
    [SerializeField]
    SpellMarker mSpellMarker = null;


    private GameObject mPlayer;
    private LayerMask mIndicatorLayerMask;
    private void Awake()
    {
        mPlayer = GameManager.Instance.mPlayer;
        mIndicatorLayerMask = LayerMask.GetMask("Ground");
    }

    public override bool CastSpell()
    {
        Vector3 targetPos = mHit.point;
        targetPos.y = mPlayer.transform.position.y;

        if (mCanCast)
        {
            BlinkController blink = Instantiate(mSpellPrefab, targetPos, Quaternion.identity).GetComponent<BlinkController>();
            blink.Initialize(mSpellData);

            mPlayer.GetComponent<PlayerMovement>().Teleport(targetPos);
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
        if (Physics.Raycast(CursorManager.Instance.ScreenToRay, out mHit, Mathf.Infinity, mIndicatorLayerMask) 
            && Vector3.Distance(mHit.point, mPlayer.transform.position) <= mSpellData.MaxBlinkDistance)
        {
            Vector3 endPoint = mHit.point;
            endPoint.y = mPlayer.transform.position.y + mLineCastOffset.y;
            /* Debug.Log(endPoint);
             Debug.DrawLine(GameManager.Instance.mPlayer.transform.position + mLineCastOffset, endPoint, Color.red, 5f);*/
            if (!Physics.Linecast(mPlayer.transform.position + mLineCastOffset, endPoint, mTargetHitMask))
            {
                if (mCanCast)
                    return;

                mCanCast = true;
                mSpellMarker.UpdateMarker(1);
            }
            else
            {
                mCanCast = false;
            }
        }
        else
        {
            if (!mCanCast)
                return;

            mCanCast = false;
            mSpellMarker.UpdateMarker(0);
        }

    }


    #region Upgrades
    public void DamageUpgrade(int _damage = 5)
    {
        mSpellData.DamagePerTick += _damage;
    }

    public void AOEUpgrade(int _increase = 1)
    {
        mSpellData.DamageRadius += _increase;
    }

    public void RangeUpgrade(int _increase = 5)
    {
        mSpellData.MaxBlinkDistance += _increase;
    }
    #endregion

    /*    private void OnDrawGizmosSelected()
        {
            *//* if(Application.isPlaying)
                 Gizmos.DrawLine(GameManager.Instance.mPlayer.transform.position, GameManager.Instance.mPlayer.transform.forward);
             else
                 Gizmos.DrawLine(transform.position + mLineCastOffset, transform.forward * 10);*//*
        }*/
}
