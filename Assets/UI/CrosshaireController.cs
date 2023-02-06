using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshaireController : MonoBehaviour
{
    [SerializeField] private GameObject hitmarker;

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
}
