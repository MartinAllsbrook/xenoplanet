using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI loadingTimeDisplay;
    [SerializeField] private GameObject backDrop;
    [SerializeField] private GameObject textCover;
    [SerializeField] private TextDeleter1 textDeleter1;
    
    private bool _loaded = false;
    private float _loadTime = 0;
    private void Update()
    {
        if (!_loaded)
        {
            _loadTime += Time.deltaTime;
            loadingTimeDisplay.text = "Loading...    Time: " + _loadTime.ToString("F1") + "s";    
            return;
        }
        
        _loadTime += Time.deltaTime;
        loadingTimeDisplay.text = "Running v1.24.4.2";
    }

    public void DoneLoading()
    {
        backDrop.SetActive(false);
        textCover.SetActive(false);
        _loaded = true;
        textDeleter1.Delete();
    }
}
