using UnityEngine;

/// <summary>
/// ShockWave (Soleis + Cinos):
///     Base Spell Description:
///         Player casts a 360-degree shockwave that deals minor damage (~15) and pushes 
///         enemies within 15 meters back by up to 10 meters. 
///         
///     Upgrade 1:
///         Initial damage is increased. 
///     
///     Upgrade 2: 
///         The Shockwave now stuns enemies
///         
///     Upgrade 3:
///         The Shockwave has a larger area of damage
///         
/// </summary>
public class ShockWaveController : MonoBehaviour
{
    [SerializeField] ShockWaveDamage mShockWaveDamage = null;
    [SerializeField] private ParticleSystem mEffect = null;

    public void Initialize(ShockWaveData _data)
    {
        mEffect.Play(true);
        mShockWaveDamage.Initialize(_data);
    }
}
