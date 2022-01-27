using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour
{
    public Vector3 mStartPos;
    public Vector3 mEndPos;
    public float mDestroyReductionInterval = 15;

    [SerializeField]
    CapsuleCollider mCapsule = null;
    [SerializeField]
    LineRenderer mLine = null;
    [SerializeField]
    Transform mBeamTrans = null;
    [SerializeField]
    public int mDamage = 20;
    [SerializeField]
    private float mMaxTimeToTakeDestroying = 3f;

    #region State Variables
    private bool mIsDestroying = false;
    public bool mBeamDoneGrowing = false;
    public Vector3 mMaxEndPos;
    private float mTimeSinceDestroyTrigger = 0;
    #endregion

    public void Grow(Vector3 _growthAmount)
    {
        mEndPos += _growthAmount;
        mBeamDoneGrowing = false;
    }

    void Start()
    {
        mCapsule.radius = mLine.startWidth / 2;
        mCapsule.center = Vector3.zero;
        mCapsule.direction = 2;
    }

    void Update()
    {
        if (mIsDestroying)
            DestroyAnimated();

        mLine.SetPosition(0, mStartPos);
        mLine.SetPosition(1, mEndPos);

        mCapsule.transform.position = mStartPos + (mEndPos - mStartPos) / 2;
        mCapsule.transform.LookAt(mStartPos);
        mCapsule.height = (mEndPos - mStartPos).magnitude;
    }

    private void LateUpdate()
    {
        mBeamTrans.position = mStartPos;
    }

    
    private void DestroyAnimated()
    {
        mTimeSinceDestroyTrigger += Time.deltaTime;
        if (mTimeSinceDestroyTrigger > mMaxTimeToTakeDestroying || (mStartPos - mEndPos).magnitude < 1)
        {
            Destroy(gameObject);
            return;
        }

        mEndPos -= ((mEndPos - mStartPos).normalized * mDestroyReductionInterval * Time.deltaTime);

    }

    public void DestroyLaser()
    {
        mTimeSinceDestroyTrigger = 0;
        mIsDestroying = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IAffectable affectable = other.GetComponent<IAffectable>();
            affectable.TakeDamage(mDamage, AttackFlags.Arcane | AttackFlags.Enemy);
        }
        else
        {
            Debug.Log("Occluded by " + other.name);
            mEndPos.x = Mathf.Min(other.transform.position.x, mEndPos.x);
            mEndPos.z = Mathf.Min(other.transform.position.z, mEndPos.z);
            mBeamDoneGrowing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Debug.Log("Resetting length");
            //mEndPos = mMaxEndPos;
        }
    }

}
