using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaylightCycle : MonoBehaviour
{
    [SerializeField] private float dayLength;
    [SerializeField] private AnimationCurve fogCurve;
    
    private float _dayTimer;

    private Quaternion _sunRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        _dayTimer = 0; 
        _sunRotation = transform.rotation;
        
    }

    // Update is called once per frame
    void Update()
    {
        // Count though day
        if (_dayTimer < dayLength) _dayTimer += Time.deltaTime;
        else _dayTimer = 0;
        var dayPercent = _dayTimer / dayLength;
        
        // Set sun
        var sunPosition = 360 * dayPercent;
        transform.rotation = Quaternion.Euler(new Vector3(sunPosition, -30f, 0f));
        
        // Set fog
        var fogColor = fogCurve.Evaluate(dayPercent);
        RenderSettings.fogColor = new Color(fogColor, fogColor, fogColor);

    }
}
