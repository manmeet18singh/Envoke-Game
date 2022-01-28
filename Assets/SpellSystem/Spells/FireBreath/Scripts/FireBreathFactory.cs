using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public struct FireBreathData
{
    public float LifeTime;
    public int InitialDamage;
    public int Duration;
    public int DamagePerTick;
    public bool CanDOT;
}


public class FireBreathFactory : SpellFactoryBase
{
    [SerializeField]
    FireBreathData mSpellData;

    [Header("UI")]
    [SerializeField] GameObject mIndicatorCanvas = null;
    [SerializeField] Image mRangeIndicator = null;


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

    public override bool CastSpell()
    {
        //GameManager.Instance.PausePlayerInput();
        FireBreath spell = Instantiate(mSpellPrefab, GameManager.Instance.mSpellFirePoint.position, GameManager.Instance.mSpellFirePoint.rotation).GetComponent<FireBreath>();
        spell.Initialize(ref mSpellData);
        return true;
    }

    private void Update()
    {
        Vector3 lightningEndPoint = GameManager.Instance.mPlayer.transform.position + (GameManager.Instance.mPlayer.transform.forward);
        Vector3 position = GameManager.Instance.mPlayer.transform.position;
        Quaternion rotation = Quaternion.LookRotation(lightningEndPoint - GameManager.Instance.mPlayer.transform.position);
        rotation *= Quaternion.Euler(0, 90, 0);

        UpdateIndicator(position, rotation);
    }

    private void UpdateIndicator(Vector3 _position, Quaternion _rotation)
    {
        mIndicatorCanvas.transform.position = new Vector3(_position.x, _position.y + 2, _position.z);
        mRangeIndicator.rectTransform.rotation = _rotation * Quaternion.Euler(90, 0, 90);
    }

    public void DOTUpgrade()
    {
        mSpellData.CanDOT = true;
    }

    public void IncreaseInitialDamage(int _damage)
    {
        mSpellData.InitialDamage += _damage;
    }
    
    public void IncreaseDOTDuration(int _duration)
    {
        mSpellData.Duration = _duration;
    }
}
