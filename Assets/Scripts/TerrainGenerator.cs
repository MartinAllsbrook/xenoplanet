using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainGenerator : MonoBehaviour
{
    public int width;
    public int length;
    public int depth;

    [SerializeField] private TerrainPainter terrainPainter;
    [SerializeField] private TerrainData baseTerrainData;
    
    private float _seed;

    private void Start()
    {
        _seed = TerrainLoader.Instance.seed;

        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = Instantiate(baseTerrainData);
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
        
        TerrainCollider terrainCollider = GetComponent<TerrainCollider>();
        terrainCollider.terrainData = terrain.terrainData;
        
        terrainPainter.PaintTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width;
        
        terrainData.size = new Vector3(width, depth, length);
        
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
                heights[x, y] = CompileNoise(x, y);
                // heights[x, y] = CalculateNoise(x, y, _tempSeed, macroScale);
            }
        }

        return heights;
    }

    float CompileNoise(int x, int y)
    {
        float height;

        float mountains = CalculateNoise(x, y, TerrainLoader.Instance.biomeScale);
        mountains *= mountains;
        
        height = CalculateNoise(x, y, TerrainLoader.Instance.macroScale) * mountains;
        
        return height;
    }

    float CalculateNoise(int x, int y, float scale)
    {
        var position = transform.position;
        float xNorm = (float) (x + position.z - (position.z / 513)) / width * scale + _seed ;
        float yNorm = (float) (y + position.x - (position.x / 513)) / length * scale + _seed ;
        
        return Mathf.PerlinNoise(xNorm, yNorm);
    }
}