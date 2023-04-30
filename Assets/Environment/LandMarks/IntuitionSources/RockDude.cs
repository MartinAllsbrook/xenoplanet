using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class RockDude : IntuitionSource
{
    // A particle system to play when the reward is used
    [SerializeField] private ParticleSystem unusedParticleSystem;
    [SerializeField] private ParticleSystem onUseParticleSystem;
    
    protected override void Consume()
    {
        base.Consume();
        Player.Instance.AddShield(15);
        unusedParticleSystem.Stop();
        onUseParticleSystem.Play();
    }
}
