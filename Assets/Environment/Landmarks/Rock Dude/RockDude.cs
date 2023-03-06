using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDude : MonoBehaviour
{
    // A integer that stores the mana reward value
    public int manaReward;

    // A particle system to play when the reward is used
    [SerializeField] private ParticleSystem unusedParticleSystem;
    [SerializeField] private ParticleSystem onUseParticleSystem;

    // A bool that indicates whether the reward has been used or not
    public bool used;

    // A method that returns the mana reward, plays the particle system, and sets used to true if used is false
    public int UseReward()
    {
        // Check if the reward has been used or not
        if (!used)
        {
            // Play the particle system
            onUseParticleSystem.Play();
            unusedParticleSystem.Stop();

            // Set used to true
            used = true;
            
            // Return the mana reward value
            return manaReward;
        }
        return 0;
    }
}
