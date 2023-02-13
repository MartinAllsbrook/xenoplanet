using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkData
{
    // private Vector2Int _position;
    public float[,] HeightMap;
    public float[,] MoistureMap;
    public float[,] StrangenessMap;
    public int[,] BiomeMap;
    public int NumBiomes;

    public ChunkData(float[,] heightMap, float[,] moistureMap, float[,] strangenessMap, int[,] biomeMap, int numBiomes)
    {
        HeightMap = heightMap;
        MoistureMap = moistureMap;
        StrangenessMap = strangenessMap;
        BiomeMap = biomeMap;
        NumBiomes = numBiomes;
    }
}
