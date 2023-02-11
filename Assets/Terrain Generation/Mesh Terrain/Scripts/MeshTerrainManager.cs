using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeshTerrainManager : MonoBehaviour
{
    [SerializeField] private GameObject terrainChunk;
    [SerializeField] private int terrainRadius;

    private int _terrainSize;
    
    private MeshTerrainChunk[,] _chunks;
    private int _seed;

    private int _chunkSize;
    
    private readonly Quaternion _zeroRotation = new Quaternion(0, 0, 0, 0);
    
    private void Start()
    {
        _terrainSize = terrainRadius * 2 + 1;
        _chunkSize = terrainChunk.GetComponent<MeshTerrainChunk>().GetChunkSize();
        _seed = Random.Range(1000, 2000);
        
        CreateInitialChunks();
    }

    private void CreateInitialChunks()
    {
        _chunks = new MeshTerrainChunk[_terrainSize, _terrainSize];
        for (int x = 0; x < _terrainSize; x++)
        {
            for (var z = 0; z < _terrainSize; z++)
            {
                MeshTerrainChunk newChunk = Instantiate(terrainChunk, new Vector3(x * (_chunkSize - 1), 0, z * (_chunkSize - 1)), _zeroRotation).GetComponent<MeshTerrainChunk>();
                newChunk.SetTerrain(_seed);
                _chunks[x, z] = newChunk;
            }
        }
    }
}
