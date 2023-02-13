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
    private int[] _seeds;
    
    private int _chunkSize;

    private int _xPlayerCell;
    private int _zPlayerCell;
    
    private readonly Quaternion _zeroRotation = new Quaternion(0, 0, 0, 0);
    
    private void Start()
    {
        _terrainSize = terrainRadius * 2 + 1;
        _chunkSize = terrainChunk.GetComponent<MeshTerrainChunk>().GetChunkSize();
        _seeds = new int[3];
        _seeds[0] = Random.Range(2000, 10000);
        _seeds[1] = Random.Range(2000, 10000);
        _seeds[2] = Random.Range(2000, 10000);

        _chunkData = new List<MeshTerrainChunk>();
        
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
        
        // Debug.Log("X cell: " + _xPlayerCell + " Z cell: " + _zPlayerCell);
        
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
                _loadedChunks[x, z] = LoadChunk(new Vector2Int(x,z));
            }
        }
    }

    private void LoadRow(int deltaCell, bool loadZ)
    {
        if (loadZ)
            StartCoroutine(LoadRowZ(deltaCell));
        else
            StartCoroutine(LoadRowX(deltaCell));
    }

    private IEnumerator LoadRowZ(int deltaCell)
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
            _loadedChunks[j, lastRowIndex].SetActive(false);
        }

        // Move rows
        if (deltaCell > 0)
        {
            for (int i = 0; i < length - 1; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    _loadedChunks[j, i] = _loadedChunks[j, (i + 1)];
                }
            }
        }
        else
        {
            for (int i = length - 1; i > 0; i--)
            {
                for (int j = 0; j < length; j++)
                {
                    _loadedChunks[j, i] = _loadedChunks[j, i - 1]; 
                }
            }
        }
        
        for (var i = -terrainRadius; i <= terrainRadius; i++)        
        {
            int x = i + _xPlayerCell;
            int z = _zPlayerCell + terrainRadius * deltaCell;

            _loadedChunks[i + terrainRadius, firstRowIndex] = LoadChunk(new Vector2Int(x,z));
            yield return null;
        }

        yield return null;
    }

    private IEnumerator LoadRowX(int deltaCell)
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
            _loadedChunks[lastRowIndex, j].SetActive(false);
        }

        // Move rows
        if (deltaCell > 0)
        {
            for (int i = 0; i < length - 1; i++)
            {
                for (int j = 0; j < length; j++)
                {
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
                    _loadedChunks[i, j] = _loadedChunks[i - 1, j];
                }
            }
        }
        
        for (var i = -terrainRadius; i <= terrainRadius; i++)        
        {
            var x = _xPlayerCell + terrainRadius * deltaCell;
            var z = i + _zPlayerCell;

            _loadedChunks[firstRowIndex, i + terrainRadius] = LoadChunk(new Vector2Int(x,z));
            yield return null;
        }
        yield return null;
    }
    
    private GameObject LoadChunk(Vector2Int chunkPosition)
    {
        for (int i = 0; i < _chunkData.Count; i++)
        {
            if (chunkPosition == _chunkData[i].GetPosition())
            {
                // Debug.Log("It's working");
                GameObject savedChunk = _chunkData[i].gameObject;
                savedChunk.SetActive(true);
                return savedChunk;
            }
        }
        GameObject newChunk = Instantiate(terrainChunk, new Vector3(chunkPosition.x * (_chunkSize - 1), 0, chunkPosition.y * (_chunkSize - 1)), _zeroRotation);
        newChunk.GetComponent<MeshTerrainChunk>().SetTerrain(_seeds);
        _chunkData.Add(newChunk.GetComponent<MeshTerrainChunk>());
        return newChunk;
    }

    private void UpdatePlayerCell()
    {
        var playerPosition = Player.Instance.transform.position;
        _xPlayerCell = (int) MathF.Floor(playerPosition.x / (_chunkSize - 1));
        _zPlayerCell = (int) MathF.Floor(playerPosition.z / (_chunkSize - 1));
    }
}
