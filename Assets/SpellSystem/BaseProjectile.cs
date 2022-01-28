#region HEADER
////////////////////////////////////////////////////////////////////////////////
/// Purpose: Basic projectile class which controls projectile movement according
/// to set speed
/// Author: Yashwant Patel
////////////////////////////////////////////////////////////////////////////////
#endregion
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class BaseProjectile : MonoBehaviour
{
    [SerializeField]
    private float mSpeed = 0f;
    [SerializeField]
    protected float mLifeTime = 5f;
    [SerializeField]
    protected GameObject mHitEffect = null;
    private Rigidbody mRigidBod;
    [SerializeField]
    protected string[] mCastSFX;
    [SerializeField]
    protected string[] mHitTargetSFX;


    protected virtual void Awake()
    {
        mRigidBod = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        StartCoroutine(WaitForDeath());
        AudioManager.PlayRandomSFX(mCastSFX);
    }

    public virtual void Move(Vector3 _pos)
    {
        mRigidBod.velocity = (_pos - transform.position).normalized  * mSpeed;
    }

    public virtual void Move()
    {
        mRigidBod.velocity = transform.forward * mSpeed;
    }

    IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(mLifeTime);
        Destroy(gameObject);
    } 
}
