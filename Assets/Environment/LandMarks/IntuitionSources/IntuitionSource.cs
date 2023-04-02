using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntuitionSource : MonoBehaviour
{
    // A integer that stores the mana reward value
    [SerializeField] private int manaReward;
    
    // A bool that indicates whether the reward has been used or not
    public bool used;

    // A method that returns the mana reward, plays the particle system, and sets used to true if used is false
    public int UseReward()
    {
        // Check if the reward has been used or not
        if (!used)
        {
            Consume();

            // Set used to true
            used = true;
            
            // Return the mana reward value
            return manaReward;
        }
        return 0;
    }

    protected virtual void Consume()
    {
        
    }
}
