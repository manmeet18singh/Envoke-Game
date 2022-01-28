using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PortalSpawner))]
public class PortalHealth : Health
{
    #region References
    [Header("UI References")]
    [SerializeField] protected GameObject mHealthBar = null;
    [SerializeField] protected Image mHealthBarImage = null;
    [SerializeField] PortalSpawner mPortalSpawner = null;
    [SerializeField] PortalAnimationController mController = null;
    #endregion

    public override void Dying()
    {
        mHealthBar.SetActive(false);
        mPortalSpawner.IsEnabled = false;
        mController.Dead();
    }

    public override void Death()
    {
        Destroy(gameObject);
    }

    public override void TakeDamage(int _damage, AttackFlags _damageFlags)
    {
        if (CurrentHealth <= 0)
            return;

        if (mHealthBar != null && !mHealthBar.activeInHierarchy)
            mHealthBar.SetActive(true);

        base.TakeDamage(_damage, _damageFlags);
        if (mHealthBarImage != null)
            mHealthBarImage.fillAmount = mCurrentHealth / (float)mMaxHealth;
    }
}
