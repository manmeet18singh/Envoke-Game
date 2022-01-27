using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Giga Stun (Soleis + Soleis):
///     Base Spell Description:
///         Shoots a lightning bolt, damaging the nearest enemy in front of the player
///         for 15 and stunning them for 4 seconds. The bolt can jump to up to 1 additional
///         enemies doing less damage and only stunning for 1 second.
///         
///     Upgrade 1:
///         Initial damage is increased by 10 and the lightning bolt can jump to up to 3
///         enemies.
///     
///     Upgrade 2: 
///         The lightning bolt now stuns additional enemies for 3 seconds and can jump to 
///         5 enemies.
///         
///     Upgrade 3:
///         The lightning bolt can arc back to previously hit enemies. 
///         
/// </summary>
public class GigaStun : MonoBehaviour
{
    GigaStunData mSpellData;

    #region Properties
    [Header("Effects")]
    [SerializeField] private GameObject mLightningBolt = null;

    [Header("SFX")]
    [SerializeField] private string[] mInitialBlastSFX = null;
    [SerializeField] private string[] mArcSFX = null;
    #endregion

    #region State Variables
    Coroutine mCoroutine;
    Collider mNearestEnemyCollider;
    IAffectable mEnemyAffected;
    #endregion

    #region References
    private GameObject mPlayer;
    private LayerMask mEnemyLayer;
    #endregion

    /// <summary>
    /// Starts off the initial blast and then arcs through each nearby enemy that can be hit.
    /// </summary>
    IEnumerator GigaStunBlast()
    {
        GameObject affectedEnemy = InitialBlast();

        //Debug.Log("Initial hit enemy: " + affectedEnemy);

        for (int i = 0; affectedEnemy != null && i < mSpellData.NumberOfArcs; ++i)
        {
            affectedEnemy = Arc(affectedEnemy.gameObject);
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.4f));
        }

        mCoroutine = null;
        Destroy(gameObject);
        yield return null;
    }

    public void Initialize(ref GigaStunData _mSpellData, Collider _nearestEnemyCollider, IAffectable _affectedEnemy)
    {
        mSpellData = _mSpellData;
        mPlayer = GameManager.Instance.mPlayer;
        mEnemyLayer = LayerMask.GetMask("Enemy");
        mNearestEnemyCollider = _nearestEnemyCollider;
        mEnemyAffected = _affectedEnemy;

        if (mNearestEnemyCollider != null)
        {
            mCoroutine = StartCoroutine(GigaStunBlast());
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("GigaStun didn't find a target.");
            Destroy(gameObject);
#endif
        }
    }

    private void OnDestroy()
    {
        if (mCoroutine != null)
            StopCoroutine(mCoroutine);
    }

    #region Spell Logic
    /// <summary>
    /// The initial blast shoots out a beam of lightning that latches onto the
    /// nearest enemy in front of the player. This does heavy damage and stuns
    /// for a long while. After the initial blast, the arcing logic begins.
    /// </summary>
    /// 
    /// <returns>The Enemy that received the initial blast. If this is null, 
    /// then Giga Blast fizzles out.</returns>
    private GameObject InitialBlast()
    {
        if (mNearestEnemyCollider != null)
        {
            // Stun
            //Debug.Log("Stunning for " + mSpellData.InitialStunDuration + " seconds.");
            mEnemyAffected.Stun(mSpellData.InitialStunDuration);
            // Mark enemy as being affected by GigaStun (used for arc logic)
            mNearestEnemyCollider.gameObject.AddComponent<AffectedByGigaStun>();
            // Apply initial damage
            //Debug.Log("Dealing " + mSpellData.InitialDamage + " initial damage to to " + initialHitEnemy.name + " (current health: " + initialHitEnemy.mHealth.CurrentHealth + " )");
            mEnemyAffected.TakeDamage(mSpellData.InitialDamage, AttackFlags.Player | AttackFlags.Lightning);

            SpawnLightningBolt(GameManager.Instance.mSpellFirePointObj, mNearestEnemyCollider.gameObject, false);
            AudioManager.PlayRandomSFX(mInitialBlastSFX);

            return mNearestEnemyCollider.gameObject;
        }
        else
        {
            // TODO - Show fizzle out effect potentially
        }

        return null;
    }

    /// <summary>
    /// Finds the next enemy for the lightning to jump to. If an enemy is found, 
    /// it is stunned and has damage applied to it.
    /// </summary>
    /// <param name="_arcSource">The source of the previous arc.</param>
    /// <returns>The next enemy that was hit. This will be the source of the next arc. If no enemy is found, returns null.</returns>
    private GameObject Arc(GameObject _arcSource)
    {
        IAffectable enemyAffected;
        Collider[] nearbyEnemies = Physics.OverlapSphere(_arcSource.transform.position, mSpellData.ArcJumpRadius, mEnemyLayer);
        Collider enemyCollider = GetNearestEnemy(nearbyEnemies, _arcSource.transform.position, out enemyAffected, _arcSource, mSpellData.ArcCanBacktrack);

        if (enemyCollider != null)
        {
            SpawnLightningBolt(_arcSource, enemyCollider.gameObject);
            //Debug.Log("Arcing");
            AudioManager.PlayRandomSFX(mArcSFX);

            enemyAffected.Stun(mSpellData.ArcStunDuration);
            enemyCollider.gameObject.AddComponent<AffectedByGigaStun>();
            //Debug.Log("Dealing " + mArcDamage + " arc damage to to " + nextEnemy.name);
            enemyAffected.TakeDamage(mSpellData.ArcDamage, AttackFlags.Player | AttackFlags.Lightning);
        }

        return enemyCollider == null ? null : enemyCollider.gameObject;
    }

    /// <summary>
    /// Spawns the visual lightning bolt effect used during the intial blast and the 
    /// arc.
    /// </summary>
    /// <param name="_source">Where the lightning bolt spawns from.</param>
    /// <param name="_destination">Where the lightning bolt hit. This is the enemy game object.</param>
    private void SpawnLightningBolt(GameObject _source, GameObject _destination, bool _useStaticYPos = true)
    {
        Debug.Log("source y " + _source.transform.localPosition.y);
        float staticYPos = _useStaticYPos ? 1.4f : 0;
        Vector3 sourcePos = new Vector3(_source.transform.position.x, staticYPos, _source.transform.position.z);

        GameObject lightningBolt = Instantiate(mLightningBolt, sourcePos, Quaternion.identity);
        LineController lineController = lightningBolt.GetComponent<LineController>();
        lineController.mStaticYSourcePos = _useStaticYPos ? staticYPos : 0;
        lineController.mSource = _source;
        lineController.mDestination = _destination;
    }

    #endregion

    #region Helper Functions
    /// <summary>
    /// Finds the nearest enemy to the lightning bolt source.
    /// </summary>
    /// <param name="_hitColliders">Enemies within range of source.</param>
    /// <param name="_source">The Location where the lightning bolt originated from.</param>
    /// <param name="_sourceObject">The object that triggered the search. This object won't be considered.</param>
    /// <param name="_enemyAffected">The IAffectable of the found enemy, or null if nothing is found.</param>
    /// <param name="_canBacktrack">Whether the arc and jump back to previously hit enemies.</param>
    /// <returns>The collider of the enemy, or null if none is found.</returns>
    public static Collider GetNearestEnemy(Collider[] _hitColliders, Vector3 _source, out IAffectable _enemyAffected, GameObject _sourceObject = null, bool _canBacktrack = true)
    {
        float nearestEnemyDistance = -1;
        _enemyAffected = null;
        Collider enemyCollider = null;

        foreach (Collider collider in _hitColliders)
        {
            // Ignore source enemy
            if (_sourceObject != null && _sourceObject == collider.gameObject)
                continue;

            float distance = (collider.transform.position - _source).magnitude;

            // Skip over enemies that have recently been shocked (unless spell is upgraded)
            if (!_canBacktrack && collider.GetComponent<AffectedByGigaStun>() != null)
                continue;

            // Finds closest enemy
            if (nearestEnemyDistance == -1 || distance < nearestEnemyDistance)
            {
                IAffectable affectable = collider.GetComponent<IAffectable>();
                if (affectable == null)
                    continue;

                // Ignore dead enemies
                Health targetHealth = collider.GetComponent<Health>();
                if (targetHealth == null || targetHealth.CurrentHealth <= 0)
                    continue;

                nearestEnemyDistance = distance;
                _enemyAffected = affectable;
                enemyCollider = collider;
            }
        }

        // Will return null if _hitColliders[] is empty or only hit dead enemies
        return enemyCollider;
    }
    #endregion
}
