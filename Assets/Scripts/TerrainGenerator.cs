
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainGenerator : MonoBehaviour
{
    public int width;
    public int length;

    public int depth;

    [SerializeField] private TerrainPainter terrainPainter;

    public float macroScale;
    public float microScale;

    private float _macroOffset;
    private float _microOffset;

    private void Start()
    {
        _macroOffset = Random.Range(0f, 9999f);
        _microOffset = Random.Range(0f, 9999f);
        
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
        
        terrainPainter.PaintTerrain();
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
                heights[x, y] = CalculateNoise(x,y);
            }
        }

        return heights;
    }

    float CalculateNoise(int x, int y)
    {
        float xMicro = (float) x / width * microScale + _microOffset;
        float yMicro = (float) y / length * microScale + _microOffset;
        
        float xMacro = (float) x / width * macroScale + _macroOffset;
        float yMacro = (float) y / length * macroScale + _macroOffset;
        
        var noiseMicro = Mathf.PerlinNoise(xMicro, yMicro);
        var noiseMacro = Mathf.PerlinNoise(xMacro, yMacro);
        return noiseMacro * 0.75f + noiseMicro * 0.25f;

        // return noiseMacro;
    }
}
