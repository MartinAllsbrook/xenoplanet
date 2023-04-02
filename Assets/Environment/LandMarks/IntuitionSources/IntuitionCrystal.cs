using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntuitionCrystal : IntuitionSource
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material unusedMaterial;
    [SerializeField] private Material usedMaterial;
    [SerializeField] private Light pointLight;
    [SerializeField] private Light spotLight;
    [SerializeField] private AudioSource ambientAudio;
    [SerializeField] private AudioSource onUseAudio;
    [SerializeField] private ParticleSystem unusedParticleSystem;
    [SerializeField] private ParticleSystem onUseParticleSystem;
    
    protected override void Consume()
    {
        base.Consume();

        ambientAudio.Stop();
        onUseAudio.Play();
        unusedParticleSystem.Stop();
        onUseParticleSystem.Play();
        StartCoroutine(ConsumeRoutine());

    }

    private IEnumerator ConsumeRoutine()
    {
        yield return new WaitForSeconds(2.2f);
        pointLight.intensity = 0.8f;
        spotLight.intensity = 0.5f;
        meshRenderer.material = usedMaterial;

    }
}
