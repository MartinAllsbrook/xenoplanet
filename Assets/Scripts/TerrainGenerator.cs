
using System;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int width = 256;
    public int length = 256;

    public int depth = 20;

    private void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.size = new Vector3(width, length, depth);
        
        terrainData.SetHeights(0,0, GenerateHeights());

        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, length];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                heights[x, y] = CalculateNoise(x,y);
            }
        }

        return heights;
    }

    float CalculateNoise(int x, int y)
    {
        float xNorm = x / width;
        float yNorm = y / length;

        return Mathf.PerlinNoise(xNorm, yNorm);
    }
}
