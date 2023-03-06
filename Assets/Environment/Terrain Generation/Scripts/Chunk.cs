using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class Chunk : MonoBehaviour
{
    [SerializeField] private float[] octaves;
    [SerializeField] private float redistributionFactor;
    
    // private TerrainPainter terrainPainter;
    // private TerrainScatter terrainScatter;
    // private BiomeGenerator biomeGenerator;
    // private LandMarkGenerator landMarkGenerator;
    
    private const int _size = 64;

    // private UnityEvent chunkLoaded;
    
    
    private float[,] heightMap;
    private float[,] moistureMap;
    private int[,] biomeMap;

    private delegate void GenericDelegate();
    private delegate void GenericDelegate<T>(T variable);

    private void Awake()
    {
        // terrainPainter = gameObject.GetComponent<TerrainPainter>();
        // terrainScatter = gameObject.GetComponent<TerrainScatter>();
        // biomeGenerator = gameObject.GetComponent<BiomeGenerator>();
        // landMarkGenerator = gameObject.GetComponent<LandMarkGenerator>();
        
        // chunkLoaded = TerrainLoader.Instance.chunkLoaded;
    }

    private void Start()
    {
        // MakeNoise(20, 0, TerrainLoader.Instance.seed, MapStepTwo);
    }

    private void MapStepTwo()
    {
        // heightMap = moistureMap;
        // MakeNoise(20, 0, TerrainLoader.Instance.moistureSeed, SetTerrain);
    }

    /*private void SetTerrain()
    {
        landMarkGenerator.PlaceLandMark(ref heightMap);
        
        terrainData.SetHeights(0, 0, heightMap);
        
        TerrainCollider terrainCollider = GetComponent<TerrainCollider>();
        terrainCollider.terrainData = terrain.terrainData;
        
        terrain.detailObjectDistance = 1000;
        terrain.treeBillboardDistance = 5000;
        
        biomeMap = biomeGenerator.GenerateBiomes(heightMap, moistureMap);
        terrainPainter.PaintTerrain(terrain.terrainData, biomeMap);
        terrainScatter.ScatterFoliage(terrain, biomeMap, heightMap);
        
        chunkLoaded.Invoke();
    }*/

    // Helper functions
    private void GenerateNoise(int roadwidth, int smoothFactor, int seed, GenericDelegate onFinishedCallback)
    {
        StartCoroutine(GenerateNoiseCoroutine(seed, data =>
            {
                moistureMap = data;
                onFinishedCallback?.Invoke();
            }
        ));
    }

    IEnumerator GenerateNoiseCoroutine(int seed, GenericDelegate<float[,]> callback)
    {
        System.Diagnostics.Stopwatch timer = new Stopwatch();
        timer.Start();
    
        
        float[,] noise = new float[_size, _size];

        Vector3 position = transform.position + new Vector3(seed, 0, seed);

        for (int z = 0; z < _size; z++)
        
        {
            if (timer.ElapsedMilliseconds > 3)
            {
                yield return null;
                timer.Reset();
                timer.Start();
            }

            for (int x = 0; x < _size; x++)
            {
                noise[z, x] = CompileNoise(z, x, position);
            }
        }
        timer.Stop();
        // Debug.Log("Total time: " + timer.ElapsedMilliseconds);
        
        callback(noise);
    }
    
    float CompileNoise(int x, int y, Vector3 position)
    {
        float value = 0;
        float octaveSum = 0f;

        float xNorm = (x + position.z - (position.z / 513)) / _size;
        float yNorm = (y + position.x - (position.x / 513)) / _size;
    
        for (int i = 0; i < octaves.Length; i++)
        {
            value += (1/octaves[i]) * CalculateNoise(xNorm, yNorm, octaves[i]);
            octaveSum += 1/octaves[i];
        }
        value /= octaveSum;

        value = Mathf.Pow(value, redistributionFactor);
    
        // Debug.Log(height);
        return value;
    }
    
    float CalculateNoise(float xNorm, float yNorm, float scale)
    {
        xNorm = xNorm * scale;
        yNorm = yNorm * scale;

        // return (NoiseManager.SimplexPerlin.GetValue(xNorm, yNorm) + 1)/1;
        return Mathf.PerlinNoise(xNorm, yNorm);
    }
}
