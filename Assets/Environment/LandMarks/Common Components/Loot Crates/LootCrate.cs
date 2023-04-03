using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCrate : MonoBehaviour
{
    [SerializeField] private string[] possibleItems;
    [SerializeField] private int maxNumItems;
    
    private AudioSource lootedAudioSource;
    private bool looted = false;
    
    void Start()
    {
        lootedAudioSource = GetComponent<AudioSource>();
    }

    public bool Lootable()
    {
        return !looted;
    }

    public string[] LootItems()
    { 
        lootedAudioSource.Play();
        
        int numItems = Random.Range(1, maxNumItems);

        string[] lootedItems = new string[numItems];

        for (int i = 0; i < numItems; i++)
        {
            int itemIndex = Random.Range(0, possibleItems.Length);
            string item = possibleItems[itemIndex];
            lootedItems[i] = item;
        }

        looted = true;
        return lootedItems;
    }
}
