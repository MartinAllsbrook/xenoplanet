using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

public class CrosshaireController : MonoBehaviour
{
    [SerializeField] private GameObject hitmarker;
    [SerializeField] private GameObject crossHair;
    
    public void PlayHitMarker()
    {
        StartCoroutine(HitMarker());
    }

    IEnumerator HitMarker()
    {
        hitmarker.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitmarker.SetActive(false);
        yield return null;
    }

    public void ShowCrossHair()
    {
        crossHair.SetActive(true);
    }

    public void HideCrossHair()
    {
        crossHair.SetActive(false);
    }

}
