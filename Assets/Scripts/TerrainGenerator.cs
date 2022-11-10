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
    public float macroScale;
    private float _tempSeed;
    private void Start()
    {
        _tempSeed = 0;
        // _tempSeed = Random.Range(0, 9999f);
        // TerrainCollider tc = GetComponent<TerrainCollider>();
        // terrain.terrainData = Instantiate(terrain.terrainData);
        // tc.terrainData = terrain.terrainData;
        
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
                // heights[x, y] = CalculateNoise(x, y, TerrainLoader.Instance.seed, macroScale);
                heights[x, y] = CalculateNoise(x, y, _tempSeed, macroScale);
            }
        }

        return heights;
    }

    float CalculateNoise(int x, int y, float seed, float scale)
    {
        var position = transform.position;
        float xNorm = (float) (x + position.z - (position.z / 513)) / width * scale + seed ;
        float yNorm = (float) (y + position.x - (position.x / 513)) / length * scale + seed ;
        
        return Mathf.PerlinNoise(xNorm, yNorm);
    }
}