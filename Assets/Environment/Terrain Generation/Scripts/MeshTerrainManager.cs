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
    [SerializeField] private GameObject grassChunk;
    [SerializeField] private int terrainRadius;
    [SerializeField] private Transform playerTransform;
    
    private int _terrainSize;

    private GameObject[,] _activeChunks;
    private GameObject[,] _grassChunks;
    private List<MeshTerrainChunk> _loadedChunks; 
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

        _loadedChunks = new List<MeshTerrainChunk>();
        
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
        _activeChunks = new GameObject[_terrainSize, _terrainSize];
        _grassChunks = new GameObject[_terrainSize, _terrainSize];

        for (int x = 0; x < _terrainSize; x++)
        {
            for (int z = 0; z < _terrainSize; z++)
            {
                _grassChunks[x,z] = Instantiate(grassChunk);
            }
        }
        
        for (int x = -_terrainSize; x < _terrainSize * 2; x++)
        {
            for (var z = -_terrainSize; z < _terrainSize * 2; z++)
            {
                if (x < _terrainSize && z < _terrainSize && x >= 0 && z >= 0)
                    _activeChunks[x, z] = LoadChunk(new Vector2Int(x,z), _grassChunks[x, z].GetComponent<ChunkGrassManager>());
                else
                    LoadInactiveChunk(new Vector2Int(x,z));
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
        GameObject[] grassHolder = new GameObject[length];

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

        // Deactivate last row
        for (int j = 0; j < length; j++)
        {
            _activeChunks[j, lastRowIndex].SetActive(false);
            grassHolder[j] = _grassChunks[j, lastRowIndex];
        }

        // Move rows
        if (deltaCell > 0)
        {
            for (int i = 0; i < length - 1; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    _activeChunks[j, i] = _activeChunks[j, (i + 1)];
                    _grassChunks[j, i] = _grassChunks[j, i + 1];
                }
            }
        }
        else
        {
            for (int i = length - 1; i > 0; i--)
            {
                for (int j = 0; j < length; j++)
                {
                    _activeChunks[j, i] = _activeChunks[j, i - 1]; 
                    _grassChunks[j, i] = _grassChunks[j, i - 1];
                }
            }
        }
        
        for (var i = -terrainRadius; i <= terrainRadius; i++)        
        {
            int x = i + _xPlayerCell;
            int z = _zPlayerCell + terrainRadius * deltaCell;

            _grassChunks[i + terrainRadius, firstRowIndex] = grassHolder[i + terrainRadius];
            _activeChunks[i + terrainRadius, firstRowIndex] = LoadChunk(new Vector2Int(x,z), grassHolder[i + terrainRadius].GetComponent<ChunkGrassManager>());
            yield return null;
        }

        yield return null;
    }

    private IEnumerator LoadRowX(int deltaCell)
    {
        var length = _terrainSize;
        GameObject[] grassHolder = new GameObject[length];
        
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

        // Deactivate last row
        for (int j = 0; j < length; j++)
        {
            _activeChunks[lastRowIndex, j].SetActive(false);
            grassHolder[j] = _grassChunks[lastRowIndex, j];
        }

        // Move rows
        if (deltaCell > 0)
        {
            for (int i = 0; i < length - 1; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    _activeChunks[i, j] = _activeChunks[i + 1, j];
                    _grassChunks[i, j] = _grassChunks[i + 1, j];
                }
            }
        }
        else
        {
            for (int i = length - 1; i > 0; i--)
            {
                for (int j = 0; j < length; j++)
                {
                    _activeChunks[i, j] = _activeChunks[i - 1, j];
                    _grassChunks[i, j] = _grassChunks[i - 1, j];
                }
            }
        }
        
        for (var i = -terrainRadius; i <= terrainRadius; i++)        
        {
            var x = _xPlayerCell + terrainRadius * deltaCell;
            var z = i + _zPlayerCell;

            _grassChunks[firstRowIndex, i + terrainRadius] = grassHolder[i + terrainRadius];
            _activeChunks[firstRowIndex, i + terrainRadius] = LoadChunk(new Vector2Int(x,z), grassHolder[i + terrainRadius].GetComponent<ChunkGrassManager>());
            yield return null;
        }
        yield return null;
    }
    
    private GameObject LoadChunk(Vector2Int chunkPosition, ChunkGrassManager grassManager)
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();
        
        // Activate generated chunk
        for (int i = 0; i < _loadedChunks.Count; i++)
        {
            if (chunkPosition == _loadedChunks[i].GetPosition())
            {
                MeshTerrainChunk savedChunk = _loadedChunks[i]; // Get Chunk
                savedChunk.gameObject.SetActive(true);
                savedChunk.CheckLatePlace(); 
                savedChunk.AddGrass(grassManager);

                // TESTING TESTING
                timer.Stop();
                Debug.Log("Loaded chunk activate time: " + timer.ElapsedMilliseconds);
                // TESTING TESTING
                
                return savedChunk.gameObject; // Return the gameObject
            }
        }
        
        // Generate new chunk
        GameObject newChunk = Instantiate(terrainChunk, new Vector3(chunkPosition.x * (_chunkSize - 1), 0, chunkPosition.y * (_chunkSize - 1)), _zeroRotation,transform);
        
        var chunk = newChunk.GetComponent<MeshTerrainChunk>();
        chunk.SetTerrain(_seeds, true);
        chunk.AddGrass(grassManager);
        
        _loadedChunks.Add(newChunk.GetComponent<MeshTerrainChunk>());
        
        // TESTING TESTING
        timer.Stop();
        Debug.Log("New ACTIVE chunk load time: " + timer.ElapsedMilliseconds);
        // TESTING TESTING
        
        return newChunk;
    }
    
    private void LoadInactiveChunk(Vector2Int chunkPosition)
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();
        
        GameObject newChunk = Instantiate(terrainChunk, new Vector3(chunkPosition.x * (_chunkSize - 1), 0, chunkPosition.y * (_chunkSize - 1)), _zeroRotation,transform);
        newChunk.GetComponent<MeshTerrainChunk>().SetTerrain(_seeds, false);
        _loadedChunks.Add(newChunk.GetComponent<MeshTerrainChunk>());
        
        // TESTING TESTING
        timer.Stop();
        Debug.Log("New INACTIVE chunk load time: " + timer.ElapsedMilliseconds);
        // TESTING TESTING
    }

    private void UpdatePlayerCell()
    {
        var playerPosition = playerTransform.position;
        _xPlayerCell = (int) MathF.Floor(playerPosition.x / (_chunkSize - 1));
        _zPlayerCell = (int) MathF.Floor(playerPosition.z / (_chunkSize - 1));
    }
}
