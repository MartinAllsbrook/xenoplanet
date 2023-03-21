using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class TestingInstantiation : MonoBehaviour
{
    [SerializeField] private GameObject terrtainChunk;

    private int[] _seeds;
    private void Start()
    {
        _seeds = new int[3];
        _seeds[0] = Random.Range(2000, 10000);
        _seeds[1] = Random.Range(2000, 10000);
        _seeds[2] = Random.Range(2000, 10000);
        
        Invoke("Test", 5);
    }

    private void Test()
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();

        for (int i = 0; i < 5; i++)
        {
            var newChunk = Instantiate(terrtainChunk, new Vector3(i * 65, 0, 0), new Quaternion(0,0,0,0));
            // newChunk.GetComponent<MeshTerrainChunk>().SetTerrain(_seeds, true);
        }
        timer.Stop();
        Debug.Log(timer.Elapsed);
    }
}
