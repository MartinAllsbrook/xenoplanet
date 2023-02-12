using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class ChunkGrassManager : MonoBehaviour
{

    [SerializeField] private float minGrassHeight;
    // [SerializeField] private GameObject patch;
    [SerializeField] private GameObject[] grass;
    // [SerializeField] private Transform grassHolder;
    
    /*private void Start()
    {
        grass = new GameObject[64 * 64];
        for (int x = 0; x < 64; x++)
        {
            for (int z = 0; z < 64; z++)
            {
                grass[x * 64 + z] = Instantiate(patch, new Vector3(x, 10, z), new Quaternion(0, 0, 0, 0), transform);
            }
        }
    }*/

    public void PlaceGrass(float[,] heightMap, float maxHeight)
    {
        StartCoroutine(PlaceGrassRoutine(heightMap, maxHeight));
    }

    private IEnumerator PlaceGrassRoutine(float[,] heightMap, float maxHeight)
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();
        
        for (int i = 0; i < grass.Length; i++)
        {
            if (timer.ElapsedMilliseconds > 3)
            {
                Debug.Log("Skip");
                yield return null;
                timer.Reset();
                timer.Start();
            }
            
            var x = Mathf.FloorToInt(i / 64f);
            var z = i % 64;
            var height = heightMap[x, z] * maxHeight;
            
            if (height < minGrassHeight)
            {
                grass[i].SetActive(false);
            }
            else
            {
                grass[i].transform.position = new Vector3(transform.position.x + x + Random.Range(-0.25f, 0.25f), height + Random.Range(-0.1f, 0.1f), transform.position.z + z + Random.Range(-0.25f, 0.25f));
                grass[i].transform.rotation = Quaternion.Euler(0,Random.Range(0,360),0);
            }
        }
        timer.Stop();
        yield return null;
    }
}
