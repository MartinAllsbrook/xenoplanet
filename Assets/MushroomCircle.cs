using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomCircle : MonoBehaviour
{
    [SerializeField] private int healAmount;
    [SerializeField] private ParticleSystem baseParticles;
    [SerializeField] private ParticleSystem onUseParticles;

    [SerializeField] private Light lightSource;

    [SerializeField] private int dimSteps;
    [SerializeField] private float dimTime;
    
    private float _initialLightIntensity;
    private bool _used;
    
    public int Use()
    {
        if (!_used)
        {
            _initialLightIntensity = lightSource.intensity;
            StartCoroutine(DimLight(dimSteps, dimTime));
            
            baseParticles.Stop();
            onUseParticles.Play();
            _used = true;
            
            return healAmount;
        }

        return 0;
    }

    public IEnumerator DimLight(int steps, float time)
    {
        float deltaTime = time / steps;
        
        for (int i = 1; i <= steps; i++)
        {
            float percent = (float) i / steps;
            lightSource.intensity = Mathf.Lerp(_initialLightIntensity, 0, percent);
            
            yield return new WaitForSeconds(deltaTime);
        }

        lightSource.intensity = 0;
    }
}
