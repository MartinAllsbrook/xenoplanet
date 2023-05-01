using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinalTemple : MonoBehaviour
{
    [SerializeField] private FinalCrystal[] finalCrystals;
    [SerializeField] private MeshRenderer finalCrystalRenderer;
    [SerializeField] private Material chargedMaterial;
    [SerializeField] private AudioSource winAudio;
    private UnityEvent _onCharge;
    private int _numCrystals;
    private int _numChargedCrystals = 0;
    
    private void Start()
    {
        _onCharge = new UnityEvent();
        _onCharge.AddListener(OnCrystalCharge);
        _numCrystals = finalCrystals.Length;
        
        foreach (FinalCrystal crystal in finalCrystals)
        {
            crystal.SetEvent(_onCharge);
        }
    }

    private void OnCrystalCharge()
    {
        _numChargedCrystals++;
        if (_numChargedCrystals >= _numCrystals)
        {
            OnAllCrystalsCharged();    
        }
    }

    private void OnAllCrystalsCharged()
    {
        finalCrystalRenderer.material = chargedMaterial;
        winAudio.Play();
    }
}
