using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : Health, IAffectable
{
    public static event Action mPlayerDeath;
    public static event Action<int, int> mPlayerDamaged;
    public static event Action<int, int> mPlayerHealed;

    public static event Action<int> mDamage;
    public static event Action<int> mHeal;
    [SerializeField] public Animator mAnimator; 

    Material mPlayerMat;

    protected override void Awake()
    {
        if (SavePointSystem.SavedStats)
        {
            mCurrentHealth = SavePointSystem.Health;
            mMaxHealth = SavePointSystem.MaxHealth;
        }
        else
            base.Awake();
        
        mPlayerMat = GetComponent<MeshRenderer>().material;
    }

    private void Start()
    {
        mPlayerHealed?.Invoke(mCurrentHealth, mMaxHealth);
        mAnimator.SetBool("isDead", false);
    }

    public override void TakeDamage(int _damage, AttackFlags _damageFlags)
    {
        AudioManager.instance.Play("PlayerDamaged");
        base.TakeDamage(_damage, _damageFlags);
        CameraShake.ShakeOneshot(null, 0.25f);
        mDamage?.Invoke(_damage);
        mPlayerDamaged?.Invoke(mCurrentHealth, mMaxHealth);
        mAnimator.SetTrigger("Hit");
        StartCoroutine(FlashRed());

    }

    public override void Death()
    {
        GameManager.Instance.PausePlayerInput();
        AudioManager.instance.Play("PlayerDeath");

        if (AudioManager.instance.IsThisPlaying("Walking"))
            AudioManager.instance.Stop("Walking");

        base.Death();
        mAnimator.SetTrigger("Death");
        mAnimator.SetBool("isDead", true);
    }

    private void ShowDeath()
    {
        Cursor.visible = true;
        mPlayerDeath?.Invoke();
    }

    public override void Heal(int _health)
    {
        base.Heal(_health);
        mHeal?.Invoke(_health);
        mPlayerHealed?.Invoke(mCurrentHealth, mMaxHealth);
    }

    public void PercentHeal(int _percent)
    {
        mCurrentHealth = (MaxHealth * (_percent / 100));
        mPlayerHealed?.Invoke(mCurrentHealth, mMaxHealth);
    }

    public float GetPercentHealth()
    {
        float percentHealth = mCurrentHealth / mMaxHealth;
        return percentHealth;
    }

    public void IncreaseMaxHealth(int _healthToAdd)
    {
        mCurrentHealth += _healthToAdd;
        mMaxHealth += _healthToAdd;
        mPlayerHealed?.Invoke(mCurrentHealth, mMaxHealth);
    }

    public override void Freeze(float _duration)
    {
        //TODO freeze player
    }
    public override void DamageOverTime(int _totalDamage, int _duration, AttackFlags _damageFlags)
    {
        base.DamageOverTime(_totalDamage, _duration, _damageFlags);
        if (_damageFlags.HasFlag(AttackFlags.Fire))
        {
            mCurrentStatus |= StatusEffects.Burning;
        }
    }

    private Coroutine mSlowCoroutine;
    private float mSpeedBeforeSlow;
    public override void SlowOverTime(int _timeTillZero, float _minMoveSpeed, bool _freeze, float _duration)
    {
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement == null)
            return;

        mCurrentStatus |= StatusEffects.Slowed;

        mSpeedBeforeSlow = playerMovement.moveSpeed;
        float speedPerTick =  playerMovement.moveSpeed / _timeTillZero;
        mSlowCoroutine = StartCoroutine(SlowOverTime(speedPerTick, _freeze, _duration));
    }

    public override void StopSlowOverTime()
    {
        StopCoroutine(mSlowCoroutine);
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement == null)
            return;

        mCurrentStatus &= ~StatusEffects.Slowed;
        playerMovement.moveSpeed = mSpeedBeforeSlow;
    }

    IEnumerator SlowOverTime(float speedPerTick, bool _freeze, float _duration)
    {
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            float currentSpeed = playerMovement.moveSpeed;

            while (currentSpeed > 0)
            {
                yield return new WaitForSeconds(1f);
                currentSpeed -= speedPerTick;
                playerMovement.moveSpeed = currentSpeed;
            }

            playerMovement.moveSpeed = mSpeedBeforeSlow;
            mCurrentStatus &= ~StatusEffects.Slowed;
            if (_freeze)
                Freeze(_duration);
        }
    }

    IEnumerator FlashRed()
    {
        mPlayerMat.color = Color.red;
        yield return new WaitForSeconds(.3f);
        mPlayerMat.color = Color.white;
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        mPlayerHealed?.Invoke(mCurrentHealth, mMaxHealth);
    }
#endif

}
