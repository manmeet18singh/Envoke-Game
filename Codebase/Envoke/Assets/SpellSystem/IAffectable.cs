using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAffectable
{
    public void Freeze(float _duration);

    public void Stun(float _duration);

    public void TakeDamage(int _damage, AttackFlags _damageFlags);

    public void DamageOverTime(int _damagePerTick, int _duration, AttackFlags _damageFlags);

    public void SlowOverTime(int _timeTillZero, float _minMoveSpeed, bool _freeze = false, float _duration = 0f);

    public GameObject ApplyVFX(GameObject _VFX, float _duration, bool _useBaseTransform = false);

    public void StopSlowOverTime();
}
