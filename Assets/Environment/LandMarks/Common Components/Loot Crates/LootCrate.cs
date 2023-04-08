using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCrate : MonoBehaviour
{
    [SerializeField] private string[] possibleItems;
    [SerializeField] private int maxNumItems;
    [SerializeField] private GameObject hudMarker;
    
    private AudioSource _lootedAudioSource;
    private bool _looted = false;
    
    void Start()
    {
        _lootedAudioSource = GetComponent<AudioSource>();
    }

    public bool Lootable()
    {
        return !_looted;
    }

    public string[] LootItems()
    { 
        _lootedAudioSource.Play();
        
        int numItems = Random.Range(1, maxNumItems);

        string[] lootedItems = new string[numItems];

        for (int i = 0; i < numItems; i++)
        {
            int itemIndex = Random.Range(0, possibleItems.Length);
            string item = possibleItems[itemIndex];
            lootedItems[i] = item;
        }
        
        hudMarker.SetActive(false);
        _looted = true;
        return lootedItems;
    }
}
