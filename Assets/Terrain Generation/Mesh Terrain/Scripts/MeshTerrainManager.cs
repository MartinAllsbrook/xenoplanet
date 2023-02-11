using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class MeshTerrainManager : MonoBehaviour
{
    [SerializeField] private GameObject terrainChunk;
    [SerializeField] private int terrainRadius;

    private int _terrainSize;

    private GameObject[,] _loadedChunks;
    private List<MeshTerrainChunk> _chunkData; 
    private int _seed;

    private int _chunkSize;

    private int _xPlayerCell;
    private int _zPlayerCell;
    
    private readonly Quaternion _zeroRotation = new Quaternion(0, 0, 0, 0);
    
    private void Start()
    {
        _terrainSize = terrainRadius * 2 + 1;
        _chunkSize = terrainChunk.GetComponent<MeshTerrainChunk>().GetChunkSize();
        _seed = Random.Range(1000, 2000);
        
        CreateInitialChunks();
        UpdatePlayerCell();
    }
    
    
    private void Update()
    {
        // TODO: Remove when no longer needed
        if (terrainRadius == 0)
        {
            return;
        }
        var lastXCell = _xPlayerCell;
        var lastZCell = _zPlayerCell;
        
        UpdatePlayerCell();
        
        Debug.Log("X cell: " + _xPlayerCell + " Z cell: " + _zPlayerCell);
        
        var deltaXCell = _xPlayerCell - lastXCell;
        var deltaZCell = _zPlayerCell - lastZCell;
        
        if (deltaXCell != 0 || deltaZCell != 0)
        {
            bool loadZ = deltaZCell != 0;
            if (loadZ)
                LoadRow(deltaZCell, true);
            else
                LoadRow(deltaXCell, false);
        }
    }

    private void CreateInitialChunks()
    {
        _loadedChunks = new GameObject[_terrainSize, _terrainSize];
        for (int x = 0; x < _terrainSize; x++)
        {
            for (var z = 0; z < _terrainSize; z++)
            {
                _loadedChunks[x, z] = LoadChunk(x,z);
            }
        }
    }
    
    private void LoadRow(int deltaCell, bool loadZ)
    {
        var length = _terrainSize;
        
        // Find last row
        int lastRowIndex;
        int firstRowIndex;
        if (deltaCell > 0)
        {
            lastRowIndex = 0;
            firstRowIndex = length - 1;
        }
        else
        {
            lastRowIndex = length - 1;
            firstRowIndex = 0;
        }

        // Delete last row
        for (int j = 0; j < length; j++)
        {
            if (loadZ)
                Destroy(_loadedChunks[j, lastRowIndex]);
            else
                Destroy(_loadedChunks[lastRowIndex, j]);
        }


        // Move rows

        if (deltaCell > 0)
        {
            for (int i = 0; i < length - 1; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (loadZ)
                    {
                        _loadedChunks[j, i] = _loadedChunks[j, (i + 1)];
                    }
                    else
                        _loadedChunks[i, j] = _loadedChunks[i + 1, j];

                }
            }
        }
        else
        {
            for (int i = length - 1; i > 0; i--)
            {
                for (int j = 0; j < length; j++)
                {
                    if (loadZ)
                    {
                        _loadedChunks[j, i] = _loadedChunks[j, i - 1];
                    }
                    else
                        _loadedChunks[i, j] = _loadedChunks[i - 1, j];
                }
            }
        }
        
        for (var i = -terrainRadius; i <= terrainRadius; i++)        
        {
            if (loadZ)
            {
                /*var xPos = (i + _xPlayerCell) * _chunkSize;
                var zPos = (_zPlayerCell + terrainRadius * deltaCell) * _chunkSize;
                
                _loadedChunks[i + terrainRadius, firstRowIndex] = Instantiate(terrainChunk, 
                    new Vector3(xPos, 0, zPos), 
                    new Quaternion(0,0,0,0)
                );*/

                int x = i + _xPlayerCell;
                int z = _zPlayerCell + terrainRadius * deltaCell;

                _loadedChunks[i + terrainRadius, firstRowIndex] = LoadChunk(x, z);
            }
            else
            {
                var x = _xPlayerCell + terrainRadius * deltaCell;
                var z = i + _zPlayerCell;

                _loadedChunks[firstRowIndex, i + terrainRadius] = LoadChunk(x, z);
            }
        }
    }

    private GameObject LoadChunk(int chunkX, int chunkZ)
    {
        GameObject newChunk = Instantiate(terrainChunk, new Vector3(chunkX * (_chunkSize - 1), 0, chunkZ * (_chunkSize - 1)), _zeroRotation);
        newChunk.GetComponent<MeshTerrainChunk>().SetTerrain(_seed);
        return newChunk;
    }

    private void UpdatePlayerCell()
    {
        var playerPosition = Player.Instance.transform.position;
        _xPlayerCell = (int) MathF.Floor(playerPosition.x / (_chunkSize - 1));
        _zPlayerCell = (int) MathF.Floor(playerPosition.z / (_chunkSize - 1));
    }
}
