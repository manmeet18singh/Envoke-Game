using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CastSummonPortals : BossSpell
{
    private static readonly int mMaxSummonAttempts = 10;

    [SerializeField] private CastSummonPortalsSO mSpellData;
    public bool TeleportBeforeCasting { get => mSpellData.ShouldTeleportBeforeSummon; }
    [SerializeField] private List<CastSummonPortalsSO> mVariations = null;
    [HideInInspector] private Vector3 mTeleportLocation;
    public Vector3 TeleportLocation { set => mTeleportLocation = value; }
    public int NumberOfVariations { get => mVariations.Count; }

    public List<Transform> mPortalSpawns;

    [SerializeField] protected string[] mSfx = null;
    #region State Variables
    private Coroutine mCoroutine;
    private List<GameObject> mPortals = new List<GameObject>();
    public bool HasSummonedAllPortals
    { 
        get => (mSpellData.IsRandom && mPortals.Count == mSpellData.NumberOfRandomPortals && !AllPortalsDestroyed()) 
            || (!mSpellData.IsRandom && mPortals.Count == mPortalSpawns.Count && !AllPortalsDestroyed());
    }

    #endregion

    private bool AllPortalsDestroyed()
    {
        bool allPortalsDestroyed = true;

        foreach (GameObject portal in mPortals)
        { 
            if (portal != null)
            {
                allPortalsDestroyed = false;
                break;
            }
        }

        return allPortalsDestroyed;
    }

    public override void CancelCast()
    {
        if (mCoroutine != null)
            StopCoroutine(mCoroutine);
    }

    public override void CastSpell()
    {
        mCoroutine = StartCoroutine(BeginSummoningPortals());
    }

    public override void SetSpellVariation(int _index)
    {
        if (_index >= mVariations.Count)
            return;
        mSpellData = mVariations[_index];
    }

    private IEnumerator BeginSummoningPortals()
    {
        if (mSpellData.IsRandom)
        {
            for (int i = 0; i < mSpellData.NumberOfRandomPortals; ++i)
            {
                yield return new WaitForSeconds(mSpellData.TimeBetweenPortalSummons);
                SummonPortalAtRandomLocation();
            }
        }
        else
        {
            for (int index = 0; index < mPortalSpawns.Count; ++index)
            {
                yield return new WaitForSeconds(mSpellData.TimeBetweenPortalSummons);

                if (index >= mPortals.Count)
                {
                    AudioManager.PlayRandomSFX(mSfx);
                    Transform portalLocation = mPortalSpawns[index];
                    mPortals.Add(SummonPortal(portalLocation.position, portalLocation.rotation));
                }
                else if (mPortals[index] == null)
                {
                    AudioManager.PlayRandomSFX(mSfx);
                    Transform portalLocation = mPortalSpawns[index];
                    mPortals[index] = SummonPortal(portalLocation.position, portalLocation.rotation);
                }
            }
        }

        yield return null;
    }

    private void SummonPortalAtRandomLocation()
    {
        mPortals.Clear();
        Vector3 portalPosition = new Vector3(0,0,0);
        for (int i = 0; i < mMaxSummonAttempts; ++i)
        {
            portalPosition = Random.insideUnitSphere * mSpellData.MaxPortalDistance;
            portalPosition += mBoss.transform.position;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(portalPosition, out hit, mSpellData.MaxPortalDistance, 1))
            {
                AudioManager.PlayRandomSFX(mSfx);
                mPortals.Add(SummonPortal(hit.position, Quaternion.identity));
                return;
            }
        }
        
#if UNITY_EDITOR
        Debug.LogWarning("Unable to summon a portal... couldn't sample point around " + portalPosition);
#endif
    }

    private GameObject SummonPortal(Vector3 _location, Quaternion _rotation)
    {
        return Instantiate(mSpellData.Portals[Random.Range(0, mSpellData.Portals.Count)], _location, _rotation);
    }

    public void DestroyAllPortals()
    {
        foreach (GameObject portal in mPortals)
        {
            if (portal != null)
            {
                portal.GetComponent<PortalHealth>().TakeDamage(999, AttackFlags.Unspecified);
            }
        }
    }

    public bool SomePortalsDestroyed()
    {
        if (mPortals.Count != mPortalSpawns.Count)
            return true;

        foreach (GameObject portal in mPortals)
        {
            if (portal == null)
                return true;
        }

        return false;
    }
}
