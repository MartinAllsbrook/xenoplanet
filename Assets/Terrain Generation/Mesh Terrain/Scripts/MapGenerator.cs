using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MapGenerator : MonoBehaviour
{
    // private List<ChunkData> _chunkDataList;
    // private List<Vector2> _mapCoordinates;

    [Header("Height")]
    [SerializeField] private float[] heightOctaves;
    [SerializeField] private float heightRedistributionFactor;
    [SerializeField] private int maxHeight;
    // [SerializeField] private float[] heightCutoffs;
    
    [Header("Moisture")]
    [SerializeField] private float[] moistureOctaves;
    [SerializeField] private float moistureRedistributionFactor;
    [SerializeField] private int maxMoisture;
    // [SerializeField] private float[] moistureCutoffs;

    [Header("Strangeness")]
    [SerializeField] private float[] strangenessOctaves;
    [SerializeField] private float strangenessRedistributionFactor;
    [SerializeField] private int maxStrangeness;
    // [SerializeField] private float[] strangenessCutoffs;
    
    private float[,] _generatedMap;
    
    private const int _size = 65;
    private bool _currentlyGenerating;

    private Vector2Int _currentTile;

    private delegate void GenericDelegate();
    public delegate void GenericDelegate<T>(T variable);

    private void Start()
    {
        // _chunkDataList = new List<ChunkData>();
    }

    #region Map Generation

    // Step 1: Generate Heights
    public void GenerateMap(Vector2Int tile, int[] seeds, GenericDelegate<ChunkData> finalCallback)
    {
        GenerateNoise(seeds[0], tile, heightOctaves, heightRedistributionFactor, maxHeight, () =>
        {
            GenerateMap(tile, seeds, _generatedMap, finalCallback);
        });
    }

    // Step 2: Generate Moisture
    private void GenerateMap(Vector2Int tile, int[] seeds, float[,] heightMap, GenericDelegate<ChunkData> finalCallback)
    {
        GenerateNoise(seeds[1], tile, moistureOctaves, moistureRedistributionFactor, maxMoisture, () =>
        {
            GenerateMap(tile, seeds, heightMap, _generatedMap, finalCallback);
        });
    }

    // Step 3: Generate Strangeness
    private void GenerateMap(Vector2Int tile, int[] seeds, float[,] heightMap, float[,] moistureMap, GenericDelegate<ChunkData> finalCallback)
    {
        GenerateNoise(seeds[2], tile, strangenessOctaves, strangenessRedistributionFactor, maxStrangeness, () =>
        {
            GenerateMap(heightMap, moistureMap, _generatedMap, finalCallback);
        });
    }
    
    // Step 4: Compile into biome map
    private void GenerateMap(float[,] heightMap, float[,] moistureMap, float[,] strangenessMap, GenericDelegate<ChunkData> finalCallback)
    {
        // int[,] newBiomeMap = new int[_size, _size];
        //
        // for (int x = 0; x < _size; x++)
        // {
        //     for (int z = 0; z < _size; z++)
        //     {
        //         newBiomeMap[x, z] = CalculateBiome(heightMap[x, z], moistureMap[x, z], strangenessMap[x, z]);
        //     }
        // }
        //
        // int numBiomes = (heightCutoffs.Length - 1) * (moistureCutoffs.Length - 1) * (strangenessCutoffs.Length - 1);

        // for (int i = 0; i < _size; i++)
        // {
        //     for (int j = 0; j < _size; j++)
        //     {
        //         if (heightMap[i, j] >= 1)
        //             Debug.Log("HEIGHTMAP: " + heightMap[i, j]);
        //         if (moistureMap[i, j] >= 1)
        //             Debug.Log("MOISTURE: " + moistureMap[i, j]);
        //         if (strangenessMap[i, j] >= 1)
        //             Debug.Log("STRANGENESS: " + strangenessMap[i, j]);
        //     }
        // }

        ChunkData newChunk = new ChunkData(heightMap, moistureMap, strangenessMap);
        
        finalCallback(newChunk);
        // _chunkDataList.Add(newChunk);
    }

    #endregion

    /*#region Biome Calculation

    private int CalculateBiome(float height, float moisture, float strangeness)
    {
        int biomeValue = 0;
        
        // Check Height (Set ones place)
        for (int i = 0; i < heightCutoffs.Length; i++)
        {
            if (height > heightCutoffs[i] && height < heightCutoffs[i + 1])
                biomeValue += i;
        }
        
        // Check Moisture (Set 10s place)
        for (int i = 0; i < moistureCutoffs.Length; i++)
        {
            if (moisture > moistureCutoffs[i] && moisture < moistureCutoffs[i + 1])
                biomeValue += i * 10;
        }
        
        // Check Strangeness (Set 100s place)
        for (int i = 0; i < strangenessCutoffs.Length; i++)
        {
            if (strangeness > strangenessCutoffs[i] && strangeness < strangenessCutoffs[i + 1])
                biomeValue += i + 100;
        }

        return biomeValue;
    }

    #endregion*/
    
    #region Noise Generation

    private void GenerateNoise(int seed, Vector2Int position, float[] octaves, float redistributionFactor, int maxValue, GenericDelegate onFinishedCallback)
    {
        StartCoroutine(GenerateNoiseCoroutine(seed, position, octaves, redistributionFactor, maxValue, data =>
            {
                _generatedMap = data;
                onFinishedCallback?.Invoke();
            }
        ));
    }

    IEnumerator GenerateNoiseCoroutine(int seed, Vector2Int position, float[] octaves, float redistributionFactor, int maxValue, GenericDelegate<float[,]> callback)
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();

        float[,] noise = new float[_size, _size];

        Vector2 seedOffset = new Vector2(seed, seed);
        Vector2 positionOffset = position * (_size - 1);
        Vector2 offset = positionOffset + seedOffset;

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
                noise[x, z] = (maxValue * CompileNoise(x, z, offset, octaves, redistributionFactor));
            }
        }
        timer.Stop();
        // Debug.Log("Total time: " + timer.ElapsedMilliseconds);
        
        callback(noise);
        yield return null;
    }
    
    float CompileNoise(int x, int z, Vector2 offset, float[] octaves, float redistributionFactor)
    {
        float value = 0;
        float octaveSum = 0f;

        // float xNorm = (x + position.z - (position.z / _size)) / _size;
        // float zNorm = (z + position.x - (position.x / _size)) / _size;
        
        float xNorm = (x + offset.x) / _size;
        float zNorm = (z + offset.y) / _size;
    
        for (int i = 0; i < octaves.Length; i++)
        {
            value += (1/octaves[i]) * CalculateNoise(xNorm, zNorm, octaves[i]);
            octaveSum += 1/octaves[i];
        }
        value /= octaveSum;

        value = Mathf.Pow(value, redistributionFactor);
    
        // Debug.Log(height);
        return value;
    }
    
    float CalculateNoise(float xNorm, float zNorm, float scale)
    {
        xNorm *= scale;
        zNorm *= scale;

        // return (NoiseManager.SimplexPerlin.GetValue(xNorm, yNorm) + 1)/1;
        return Mathf.PerlinNoise(xNorm, zNorm);
    }

    #endregion
}
