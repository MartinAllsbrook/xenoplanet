using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinalCrystal : MonoBehaviour
{
    [SerializeField] private int requiredIntuition;
    [SerializeField] private Material chargedMaterial;
    
    private UnityEvent _onCharge;
    private bool _charged;

    public void SetEvent(UnityEvent onCharge)
    {
        _onCharge = onCharge;
    }

    public void Charge()
    {
        if (_charged)
            return;
        
        _charged = true;
        _onCharge.Invoke();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = chargedMaterial;
    }

    public int GetRequiredIntuition()
    {
        return requiredIntuition;
    }

    public bool Charged()
    {
        return _charged;
    }
}
