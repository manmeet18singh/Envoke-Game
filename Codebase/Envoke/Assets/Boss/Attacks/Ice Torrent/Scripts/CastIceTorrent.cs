using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastIceTorrent : BossSpell
{
    [Header("Variations")]
    [SerializeField] private IceTorrentSO mIceTorrentData;
    [SerializeField] private List<IceTorrentSO> mVariations = null;
    public int NumberOfVariations { get => mVariations.Count; }

    [Header("References")]
    [SerializeField] private GameObject mIceTorrentEffect = null;
    [SerializeField] protected string[] mSfx = null;

    #region State Variables
    private Coroutine mCoroutine;
    private List<IceTorrentController> mTorrents;
    private float mCurrentAngle;
    [HideInInspector]  public float mTimeSinceLastHit = -1;
    #endregion

    private void Start()
    {
        mTorrents = new List<IceTorrentController>();
    }

    public override void CancelCast()
    {
        foreach (IceTorrentController torrent in mTorrents)
        {
            if (torrent != null)
                torrent.DestroyTorrent();
        }

        mTorrents.Clear();

        if (mCoroutine != null)
            StopCoroutine(mCoroutine);
    }

    public override void CastSpell()
    {
        mIsDoneCasting = false;
        mCurrentAngle = 0;
        mCoroutine = StartCoroutine(BeginIceTorrent());
    }

    private IEnumerator StraightIceTorrent()
    {
        float count = 0;
        Vector3 direction = ((mIceTorrentData.mOverrideTarget == null ? GameManager.Instance.mPlayer.transform.position : mIceTorrentData.mOverrideTarget.transform.position) - transform.position).normalized;
        Vector3 start = transform.position;
        
        while (mCurrentAngle < 2 * Mathf.PI)
        {
            yield return new WaitForSeconds(mIceTorrentData.mSpawnSpeed);
            Vector3 location = start + (direction * (mIceTorrentData.mDistanceBetweenTorrents * ++count));
            location.x += Mathf.Sin(mCurrentAngle) * mIceTorrentData.mDistanceVariation;
            location.y = start.y + 1;
            location.z += Mathf.Cos(mCurrentAngle) * mIceTorrentData.mDistanceVariation;
            mTorrents.Add(Instantiate(mIceTorrentEffect, location, Quaternion.identity).GetComponent<IceTorrentController>());
            mTorrents[mTorrents.Count - 1].gameObject.GetComponent<IceTorrentDamage>().mParent = this;
            mCurrentAngle += mIceTorrentData.mAngleInterval;
        }

        yield return null;
    }

        private IEnumerator FollowPlayerFlameWreath()
        {
            float endAngle = 2 * Mathf.PI;
            while (mCurrentAngle < endAngle)
            {
                Vector3 location = mIceTorrentData.mOverrideTarget == null ? GameObject.FindGameObjectWithTag("Player").transform.position : mIceTorrentData.mOverrideTarget.transform.position;
                yield return new WaitForSeconds(mIceTorrentData.mSpawnSpeed);
                Debug.Log(((endAngle - mCurrentAngle) / endAngle) * mIceTorrentData.mDistanceBetweenTorrents);
                float distance = Mathf.Max((1 - ((endAngle - mCurrentAngle) / endAngle)) * mIceTorrentData.mDistanceBetweenTorrents, mIceTorrentData.mMinFollowDistance);
                location.x += Mathf.Sin(mCurrentAngle) * distance;
                location.y += 1;
                location.z += Mathf.Cos(mCurrentAngle) * distance;
                mTorrents.Add(Instantiate(mIceTorrentEffect, location, Quaternion.identity).GetComponent<IceTorrentController>());
                mTorrents[mTorrents.Count - 1].gameObject.GetComponent<IceTorrentDamage>().mParent = this;
                mCurrentAngle += mIceTorrentData.mAngleInterval;
            }
            yield return null;
        }

    private IEnumerator BeginIceTorrent()
    {
        switch (mIceTorrentData.mPathType)
        {
            case IceTorrentSO.PathType.Straight:
                AudioManager.PlayRandomSFX(mSfx);
                yield return StraightIceTorrent();
                break;
            case IceTorrentSO.PathType.FollowPlayer:
                AudioManager.PlayRandomSFX(mSfx);
                yield return FollowPlayerFlameWreath();
                break;
        }

        //Debug.Log("is done casting");
        mIsDoneCasting = true;
        yield return null;
    }

    private void Update()
    {
        mTimeSinceLastHit += Time.deltaTime;
    }

    public override void SetSpellVariation(int _index)
    {
        if (_index >= mVariations.Count)
            return;

        mIceTorrentData = mVariations[_index];
    }
}
