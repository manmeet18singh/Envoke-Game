using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct GigaStunData
{
    #region Initial Blast Properties
    public float BlastLength;
    public float BlastWidth;
    public float InitialStunDuration;
    public int InitialDamage;
    #endregion

    #region Arc Properties
    public int NumberOfArcs;
    public int ArcDamage;
    public float ArcStunDuration;
    public float ArcJumpRadius;
    public bool ArcCanBacktrack;
    #endregion
}

public class GigaStunFactory : SpellFactoryBase
{
    [SerializeField]
    GigaStunData mSpellData;
    [Header("UI")]
    [SerializeField] GameObject mIndicatorCanvas = null;
    [SerializeField] Image mRangeIndicator = null;
    [SerializeField] Color mCanCastColor = Color.white;
    [SerializeField] Color mCantCastColor = Color.white;

    [HideInInspector] public LayerMask mEnemyLayer;

    #region References
    GameObject mPlayer;
    #endregion

    #region State Variables
    private Collider mNearestEnemy;
    private IAffectable mEnemyAffected;
    #endregion

    public override bool CastSpell()
    {
        if (mCanCast && mEnemyAffected != null)
        {
            GigaStun spell = Instantiate(mSpellPrefab, GameManager.Instance.mSpellFirePoint.position, GameManager.Instance.mSpellFirePoint.rotation).GetComponent<GigaStun>();
            spell.Initialize(ref mSpellData, mNearestEnemy, mEnemyAffected);
        }

        return mCanCast;
    }

    private void Start()
    {
        mEnemyLayer = LayerMask.GetMask("Enemy");
        mPlayer = GameManager.Instance.mPlayer;
    }

    private void OnDisable()
    {
        CursorManager.Instance.EnableCursor();
        mIndicatorCanvas.SetActive(false);
    }

    private void OnEnable()
    {
        CursorManager.Instance.DisableCursor();
        mIndicatorCanvas.SetActive(true);
    }

    private void Update()
    {
        Vector3 lightningEndPoint = mPlayer.transform.position + (mPlayer.transform.forward * mSpellData.BlastLength);
        Vector3 position = GameManager.Instance.mPlayer.transform.position;
        Quaternion rotation = Quaternion.LookRotation(lightningEndPoint - mPlayer.transform.position);
        rotation *= Quaternion.Euler(0, 90, 0);

        UpdateIndicator(position, rotation);

        // Update position to center point. Box will be drawn around this point.
        position += (lightningEndPoint - mPlayer.transform.position).normalized * mSpellData.BlastLength / 2;
        Vector3 hitCenterBox = new Vector3(mSpellData.BlastLength / 2, mSpellData.BlastWidth / 2, mSpellData.BlastWidth / 2);

        Collider[] hitBox = Physics.OverlapBox(position, hitCenterBox, rotation, mEnemyLayer);

        mNearestEnemy = GigaStun.GetNearestEnemy(hitBox, mPlayer.transform.position,
            out mEnemyAffected, null, mSpellData.ArcCanBacktrack);

        if (mNearestEnemy != null)
        {
            mRangeIndicator.color = mCanCastColor;
            mCanCast = true;
        }
        else 
        {
            mRangeIndicator.color = mCantCastColor;
            mCanCast = false;
        }
    }

    private void UpdateIndicator(Vector3 _position, Quaternion _rotation)
    {
        mIndicatorCanvas.transform.position = new Vector3(_position.x, _position.y + 2, _position.z);
        mRangeIndicator.rectTransform.sizeDelta = new Vector2(mSpellData.BlastWidth * 4, mSpellData.BlastLength);
        mRangeIndicator.rectTransform.rotation = _rotation * Quaternion.Euler(90, 0, 90);
    }

    #region Upgrade Functions
    public void UpgradeOne(int _initialDamage = 25, int _numberOfArcs = 3)
    {
        mSpellData.InitialDamage = _initialDamage;
        mSpellData.NumberOfArcs = _numberOfArcs;
    }

    public void UpgradeTwo(int _numberOfArcs = 5, int _arcStunDuration = 3)
    {
        mSpellData.NumberOfArcs = _numberOfArcs;
        mSpellData.ArcStunDuration = _arcStunDuration;
    }

    public void UpgradeThree()
    {
        mSpellData.ArcCanBacktrack = true;
    }
    #endregion

    #region Debug
    //Vector3 lightningEndPoint;
    //Vector3 position;
    //Quaternion rotation;
    //private void OnDrawGizmos()
    //{
    //    Vector3 debugCP = lightningEndPoint;
    //    debugCP.y += 20;
    //    Gizmos.DrawLine(lightningEndPoint, debugCP);

    //    Vector3 position = mPlayer.transform.position;
    //    position += (lightningEndPoint - mPlayer.transform.position).normalized * mSpellData.BlastLength / 2;
    //    Gizmos.matrix = Matrix4x4.TRS(position, rotation, new Vector3(mSpellData.BlastLength, mSpellData.BlastWidth, mSpellData.BlastWidth));
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(Vector3.zero, Vector3.one);
    //}
    #endregion
}
