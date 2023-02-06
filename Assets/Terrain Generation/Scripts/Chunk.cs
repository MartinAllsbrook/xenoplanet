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
    [SerializeField] private TerrainData baseTerrainData;
    [SerializeField] private float[] octaves;
    [SerializeField] private float redistributionFactor;
    
    private TerrainPainter terrainPainter;
    private TerrainScatter terrainScatter;
    private BiomeGenerator biomeGenerator;
    private LandMarkGenerator landMarkGenerator;
    
    private const int length = 513;
    private const int width = 513;
    private const int depth = 513;

    private UnityEvent chunkLoaded;
    
    private float[,] heightMap;
    private float[,] moistureMap;
    private int[,] biomeMap;

    private Terrain terrain;
    private TerrainData terrainData;

    private delegate void GenericDelegate();
    private delegate void GenericDelegate<T>(T variable);

    private void Awake()
    {
        terrainPainter = gameObject.GetComponent<TerrainPainter>();
        terrainScatter = gameObject.GetComponent<TerrainScatter>();
        biomeGenerator = gameObject.GetComponent<BiomeGenerator>();
        landMarkGenerator = gameObject.GetComponent<LandMarkGenerator>();
        
        chunkLoaded = TerrainLoader.Instance.chunkLoaded;
    }

    private void Start()
    {
        terrain = GetComponent<Terrain>();
        terrain.terrainData = Instantiate(baseTerrainData);
        terrainData = terrain.terrainData;
        terrainData.heightmapResolution = width;
        terrainData.size = new Vector3(width, depth, length);

        MakeNoise(20, 0, TerrainLoader.Instance.seed, MapStepTwo);
    }

    private void MapStepTwo()
    {
        heightMap = moistureMap;
        MakeNoise(20, 0, TerrainLoader.Instance.moistureSeed, SetTerrain);
    }

    private void SetTerrain()
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
    }
    
    private void BuildRoad()
    {
        Debug.Log("building road");
        var baseRoadHeight = 20f/513f;
        var curbFactor = 256;
        var baseCurbLength = 15;
        var roadWidth = 20;
        var center = 256;
        for (int x = 0; x < 513; x++)
        {
            var leftHeight = heightMap[center + roadWidth + baseCurbLength, x];
            var rightHeight = heightMap[center - roadWidth - baseCurbLength, x];
            var leftCurbLength = (int) (leftHeight * curbFactor) + baseCurbLength;
            var rightCurbLength = (int) (rightHeight * curbFactor) + baseCurbLength;
            
            for(int z = center - roadWidth - rightCurbLength; z <= center + roadWidth + leftCurbLength; z++)
            {
                float roadHeight;
                float distanceFromCenter = Math.Abs(z - center);
                float distanceFromRoad = distanceFromCenter - roadWidth;
                if (distanceFromRoad <= 0)
                    distanceFromRoad = 0;
                float percentFromRoad;
    
                if (z > center)
                {
                    percentFromRoad = distanceFromRoad / (leftCurbLength);
                    // roadHeight = percentFromRoad;
                    roadHeight = baseRoadHeight * (1-percentFromRoad) + heightMap[z,x] * percentFromRoad;
                }
                else
                {
                    percentFromRoad = distanceFromRoad / (rightCurbLength);
                    // roadHeight = percentFromRoad;
                    roadHeight = baseRoadHeight * (1-percentFromRoad) + heightMap[z,x] * percentFromRoad;
                }
                
                heightMap[z,x] = roadHeight;
            }
        }
    }


    // Helper functions
    private void MakeNoise(int roadwidth, int smoothFactor, int seed, GenericDelegate onFinishedCallback)
    {
        StartCoroutine(GenerateHeightsCoroutine(roadwidth, smoothFactor, seed, data =>
            {
                moistureMap = data;
                onFinishedCallback?.Invoke();
            }
        ));
    }

    IEnumerator GenerateHeightsCoroutine(int roadwidth, int smoothFactor, int seed, GenericDelegate<float[,]> callback)
    {
        System.Diagnostics.Stopwatch timer = new Stopwatch();
        timer.Start();
    
        float[,] heights = new float[width, length];

        Vector3 position = transform.position + new Vector3(seed, 0, seed);

        for (int z = 0; z < width; z++)
        
        {
            if (timer.ElapsedMilliseconds > 3)
            {
                yield return null;
                timer.Reset();
                timer.Start();
            }

            for (int x = 0; x < length; x++)
            {
                heights[z, x] = CompileNoise(z, x, position);
            }
        }
        timer.Stop();
        // Debug.Log("Total time: " + timer.ElapsedMilliseconds);
        
        callback( heights);
    }
    
    float CompileNoise(int x, int y, Vector3 position)
    {
        // Sea level
        float height = 0;
        float octaveSum = 0f;

        float xNorm = (x + position.z - (position.z / 513)) ;
        float yNorm = (y + position.x - (position.x / 513)) ;
    
        for (int i = 0; i < octaves.Length; i++)
        {
            height += (1/octaves[i]) * CalculateNoise(xNorm, yNorm, octaves[i]);
            octaveSum += 1/octaves[i];
        }
        height /= octaveSum;

        height = Mathf.Pow(height, redistributionFactor);
    
        // Debug.Log(height);
        return height;
    }
    
    float CalculateNoise(float xNorm, float yNorm, float scale)
    {
        xNorm = xNorm / width * scale;
        yNorm = yNorm / length * scale;

        // return (NoiseManager.SimplexPerlin.GetValue(xNorm, yNorm) + 1)/1;
        return Mathf.PerlinNoise(xNorm, yNorm);
    }
}
