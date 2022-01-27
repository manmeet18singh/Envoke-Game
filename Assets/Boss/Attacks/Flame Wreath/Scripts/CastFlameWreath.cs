using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastFlameWreath : BossSpell
{
    [SerializeField] private FlameWreathSO mFlameWreathData;

    [SerializeField] private List<FlameWreathSO> mVariations = null;
    public int NumberOfVariations { get => mVariations.Count; }

    [Header("References")]
    [SerializeField] public GameObject mFlameWreathEffect;
    [SerializeField] protected string[] mSfx = null;

    #region State Variables
    private Coroutine mCoroutine;
    private List<FlameWreathController> mFlames;
    public float mTimeSinceLastHit = -1;
    #endregion

    private void Start()
    {
        mFlames = new List<FlameWreathController>();
    }

    public override void CancelCast()
    {
        foreach (FlameWreathController flame in mFlames)
        {
            if (flame != null)
                flame.DestroyFlames();
        }

        mFlames.Clear();

        if (mCoroutine != null)
            StopCoroutine(mCoroutine);
    }

    public override void CastSpell()
    {
        mIsDoneCasting = false;
        mCoroutine = StartCoroutine(BeginFlameWreath());
    }

    private IEnumerator CircleFlameWreath()
    {
        float currentAngle = 0;

        while (currentAngle < 2 * Mathf.PI)
        {
            yield return new WaitForSeconds(mFlameWreathData.mSpawnSpeed);
            Vector3 location = mBoss.transform.position;
            location.x += Mathf.Sin(currentAngle) * mFlameWreathData.mDistanceOffset;
            //location.y += 1;
            location.z += Mathf.Cos(currentAngle) * mFlameWreathData.mDistanceOffset;
            //Debug.Log("Spawning flames at " + location + " with sin(" + currentAngle + ") = " + Mathf.Sin(currentAngle));
            mFlames.Add(Instantiate(mFlameWreathEffect, location, Quaternion.identity).GetComponent<FlameWreathController>());
            mFlames[mFlames.Count - 1].gameObject.GetComponent<FlameWreathDamage>().mParent = this;
            currentAngle += mFlameWreathData.mAngleInterval;
        }
    }

    private IEnumerator FollowPlayerFlameWreath()
    {
        float currentAngle = 0;
        float endAngle = 2 * Mathf.PI;
        while (currentAngle < endAngle)
        {
            Vector3 location = mFlameWreathData.mOverrideTarget == null ? GameManager.Instance.mPlayer.transform.position : mFlameWreathData.mOverrideTarget.transform.position;
            yield return new WaitForSeconds(mFlameWreathData.mSpawnSpeed);
            //Debug.Log(((endAngle - currentAngle) / endAngle) * mDistanceOffset);
            float distance = Mathf.Max((1 - ((endAngle - currentAngle) / endAngle)) * mFlameWreathData.mDistanceOffset, mFlameWreathData.mMinFollowDistance);
            location.x += Mathf.Sin(currentAngle) * distance;
            location.y += 1;
            location.z += Mathf.Cos(currentAngle) * distance;
            mFlames.Add(Instantiate(mFlameWreathEffect, location, Quaternion.identity).GetComponent<FlameWreathController>());
            mFlames[mFlames.Count - 1].gameObject.GetComponent<FlameWreathDamage>().mParent = this;
            currentAngle += mFlameWreathData.mAngleInterval;
        }
    }

    private IEnumerator BeginFlameWreath()
    {
        switch (mFlameWreathData.mPathType)
        {
            case FlameWreathSO.PathType.Circle:
                AudioManager.PlayRandomSFX(mSfx);
                yield return CircleFlameWreath();
                break;
            case FlameWreathSO.PathType.FollowPlayer:
                AudioManager.PlayRandomSFX(mSfx);
                yield return FollowPlayerFlameWreath();
                break;
        }

        mIsDoneCasting = true;
    }

    private void Update()
    {
        mTimeSinceLastHit += Time.deltaTime;
    }

    public override void SetSpellVariation(int _index)
    {
        if (_index >= mVariations.Count)
            return;

        mFlameWreathData = mVariations[_index];
    }
}
