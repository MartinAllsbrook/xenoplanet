using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class MeshTerrainManager : MonoBehaviour
{
    [SerializeField] private GameObject terrainChunk;
    [SerializeField] private GameObject grassChunk;
    [SerializeField] private int terrainRadius;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LoadingScreen loadingScreen;
    [SerializeField] private bool visualizationMode;

    private int _terrainSize;

    private GameObject[,] _activeChunks; // terrainRadius * 2 + 1
    private GameObject[,] _grassChunks;
    private Dictionary<Vector2Int, MeshTerrainChunk> _loadedChunks; 
    private int[] _seeds;
    
    private int _chunkSize;

    private int _xPlayerCell;
    private int _zPlayerCell;

    private int _grassArrayLength = 5;
    private UnityEvent _onChunkLoaded;
    private int _numLoadedChunks = 0;
    private int _chunksToLoad;
    
    private readonly Quaternion _zeroRotation = new Quaternion(0, 0, 0, 0);
    
    private void Start()
    {
        _terrainSize = terrainRadius * 2 + 1;
        _chunksToLoad = _terrainSize * _terrainSize;
        _onChunkLoaded = new UnityEvent();
        
        _chunkSize = terrainChunk.GetComponent<MeshTerrainChunk>().GetChunkSize();
        
        _seeds = new int[3];
        _seeds[0] = Random.Range(2000, 10000);
        _seeds[1] = Random.Range(2000, 10000);
        _seeds[2] = Random.Range(2000, 10000);

        _loadedChunks = new Dictionary<Vector2Int, MeshTerrainChunk>();
        _onChunkLoaded.AddListener(OnChunkLoaded);

        if (visualizationMode)
        {
            StartCoroutine(CreateInitialChunksVisual());
        }
        else
        {
            CreateInitialChunks();
            CreateFinalMonument();
        }
        UpdatePlayerCell();
    }

    private void Update()
    {
        var lastXCell = _xPlayerCell;
        var lastZCell = _zPlayerCell;
        
        UpdatePlayerCell();

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
        // Create Chunks
        _activeChunks = new GameObject[_terrainSize, _terrainSize];
        for (int x = 0; x < _terrainSize; x++)
        {
            for (var z = 0; z < _terrainSize; z++)
            {
                _activeChunks[x, z] = LoadChunk(new Vector2Int(x,z), _onChunkLoaded);
            }
        }
            
        // Create Grass
        _grassChunks = new GameObject[_grassArrayLength, _grassArrayLength];
        int radius = (_grassArrayLength - 1) / 2;
        for (int x = 0; x < _grassArrayLength; x++)
        {
            for (int z = 0; z < _grassArrayLength; z++)
            {
                // Create Grass Chunk
                _grassChunks[x,z] = Instantiate(grassChunk, transform);
                    
                int terrainCenter = terrainRadius;
                Vector2Int grassChunkPosition = new Vector2Int(terrainCenter + x - radius, terrainCenter + z - radius);
                    
                MeshTerrainChunk chunk = _activeChunks[grassChunkPosition.x, grassChunkPosition.y].GetComponent<MeshTerrainChunk>();
                chunk.AddGrass(_grassChunks[x,z].GetComponent<ChunkGrassManager>());
            }
        }
    }
    
    private IEnumerator CreateInitialChunksVisual()
        {
            // Create Chunks
            _activeChunks = new GameObject[_terrainSize, _terrainSize];
            for (int x = 0; x < _terrainSize; x++)
            {
                for (var z = 0; z < _terrainSize; z++)
                {
                    _activeChunks[x, z] = LoadChunk(new Vector2Int(x,z), _onChunkLoaded);
                    yield return null;
                }
            }
            
            // Create Grass
            /*_grassChunks = new GameObject[_grassArrayLength, _grassArrayLength];
            int radius = (_grassArrayLength - 1) / 2;
            for (int x = 0; x < _grassArrayLength; x++)
            {
                for (int z = 0; z < _grassArrayLength; z++)
                {
                    // Create Grass Chunk
                    _grassChunks[x,z] = Instantiate(grassChunk, transform);
                    
                    int terrainCenter = terrainRadius;
                    Vector2Int grassChunkPosition = new Vector2Int(terrainCenter + x - radius, terrainCenter + z - radius);
                    
                    MeshTerrainChunk chunk = _activeChunks[grassChunkPosition.x, grassChunkPosition.y].GetComponent<MeshTerrainChunk>();
                    chunk.AddGrass(_grassChunks[x,z].GetComponent<ChunkGrassManager>());
                }
            }*/
            yield return null;
        }

    private void OnChunkLoaded()
    {
        _numLoadedChunks++;
        if (_numLoadedChunks >= _chunksToLoad)
            DoneLoading();
    }

    private void DoneLoading()
    {
        HUDController.Instance.DoneLoading();
        Player.Instance.OnGameStart();
    }

    private void CreateFinalMonument()
    {
        Vector2Int center = new Vector2Int(12, 12);
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                Vector2Int chunkPosition = center + new Vector2Int(x, z);
                LoadChunk(chunkPosition, new Vector2Int(x, z));
            }
        }
    }

    // Move grass chunks to align with player
    private GameObject[,] ShiftGrass(GameObject[,] arr, int size, string direction)
    {
        GameObject[,] newArr = new GameObject[size, size];
        int radius = (size - 1) / 2;
        switch (direction)
        {
            case "up":
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size - 1; j++)
                    {
                        newArr[i, j] = arr[i, j + 1];
                    }
                    newArr[i, size - 1] = arr[i, 0]; // last row = first row
                    
                    int terrainCenter = terrainRadius;
                    Vector2Int grassChunkPosition = new Vector2Int(terrainCenter + i - radius, terrainCenter + radius);
                    MeshTerrainChunk chunk = _activeChunks[grassChunkPosition.x, grassChunkPosition.y].GetComponent<MeshTerrainChunk>();
                    chunk.AddGrass(newArr[i, size - 1].GetComponent<ChunkGrassManager>());
                }
                break;
            case "down":
                for (int i = 0; i < size; i++)
                {
                    for (int j = 1; j < size; j++)
                    {
                        newArr[i, j] = arr[i, j - 1];
                    }
                    newArr[i, 0] = arr[i, size - 1];
                    
                    int terrainCenter = terrainRadius;
                    Vector2Int grassChunkPosition = new Vector2Int(terrainCenter + i - radius, terrainCenter - radius);
                    MeshTerrainChunk chunk = _activeChunks[grassChunkPosition.x, grassChunkPosition.y].GetComponent<MeshTerrainChunk>();
                    chunk.AddGrass(newArr[i, 0].GetComponent<ChunkGrassManager>());
                }
                break;
            case "right":
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size - 1; j++)
                    {
                        newArr[j, i] = arr[j + 1, i];
                    }
                    newArr[size - 1, i] = arr[0, i];
                    
                    int terrainCenter = terrainRadius;
                    Vector2Int grassChunkPosition = new Vector2Int(terrainCenter + radius, terrainCenter + i - radius);
                    MeshTerrainChunk chunk = _activeChunks[grassChunkPosition.x, grassChunkPosition.y].GetComponent<MeshTerrainChunk>();
                    chunk.AddGrass(newArr[size - 1, i].GetComponent<ChunkGrassManager>());
                }
                break;
            case "left":
                for (int i = 0; i < size; i++)
                {
                    for (int j = 1; j < size; j++)
                    {
                        newArr[j, i] = arr[j - 1, i];
                    }
                    newArr[0, i] = arr[size - 1, i];
                    
                    int terrainCenter = terrainRadius;
                    Vector2Int grassChunkPosition = new Vector2Int(terrainCenter - radius, terrainCenter + i - radius);
                    MeshTerrainChunk chunk = _activeChunks[grassChunkPosition.x, grassChunkPosition.y].GetComponent<MeshTerrainChunk>();
                    chunk.AddGrass(newArr[0, i].GetComponent<ChunkGrassManager>());
                }
                break;
        }
        return newArr;
    }

    // These three methods need to become another switch statement like the shift grass method above
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

        // Deactivate last row
        for (int j = 0; j < length; j++)
        {
            _activeChunks[j, lastRowIndex].SetActive(false);
            // grassHolder[j] = _grassChunks[j, lastRowIndex];
        }

        // Move rows
        if (deltaCell > 0)
        {
            for (int i = 0; i < length - 1; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    _activeChunks[j, i] = _activeChunks[j, (i + 1)];
                    // _grassChunks[j, i] = _grassChunks[j, i + 1];
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
                    // _grassChunks[j, i] = _grassChunks[j, i - 1];
                }
            }
        }
        
        for (var i = -terrainRadius; i <= terrainRadius; i++)        
        {
            int x = i + _xPlayerCell;
            int z = _zPlayerCell + terrainRadius * deltaCell;

            // _grassChunks[i + terrainRadius, firstRowIndex] = grassHolder[i + terrainRadius];
            _activeChunks[i + terrainRadius, firstRowIndex] = LoadChunk(new Vector2Int(x,z));
            yield return null;
        }
        
        // Shift the grass array
        if (deltaCell > 0)
            _grassChunks = ShiftGrass(_grassChunks, _grassArrayLength, "up");
        else
            _grassChunks = ShiftGrass(_grassChunks, _grassArrayLength, "down");

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
            // grassHolder[j] = _grassChunks[lastRowIndex, j];
        }

        // Move rows
        if (deltaCell > 0)
        {
            for (int i = 0; i < length - 1; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    _activeChunks[i, j] = _activeChunks[i + 1, j];
                    // _grassChunks[i, j] = _grassChunks[i + 1, j];
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
                    // _grassChunks[i, j] = _grassChunks[i - 1, j];
                }
            }
        }
        
        for (var i = -terrainRadius; i <= terrainRadius; i++)        
        {
            var x = _xPlayerCell + terrainRadius * deltaCell;
            var z = i + _zPlayerCell;

            // _grassChunks[firstRowIndex, i + terrainRadius] = grassHolder[i + terrainRadius];
            _activeChunks[firstRowIndex, i + terrainRadius] = LoadChunk(new Vector2Int(x,z));
            yield return null;
        }
        
        // Shift the grass array
        if (deltaCell > 0)
            _grassChunks = ShiftGrass(_grassChunks, _grassArrayLength, "right");
        else
            _grassChunks = ShiftGrass(_grassChunks, _grassArrayLength, "left");
        
        yield return null;
    }
    
    private GameObject LoadChunk(Vector2Int chunkPosition)
    {
        if (_loadedChunks.ContainsKey(chunkPosition))
        {
            MeshTerrainChunk savedChunk = _loadedChunks[chunkPosition]; // Get Chunk
            savedChunk.gameObject.SetActive(true);
            savedChunk.CheckLatePlace();

            return savedChunk.gameObject; // Return the gameObject
        }
        
        GameObject newChunk = Instantiate(terrainChunk, new Vector3(chunkPosition.x * (_chunkSize - 1), 0, chunkPosition.y * (_chunkSize - 1)), _zeroRotation,transform);
        
        var chunk = newChunk.GetComponent<MeshTerrainChunk>();
        chunk.SetTerrain(_seeds);
        
        _loadedChunks.Add(chunkPosition, chunk);

        return newChunk;
    }

    private void UpdatePlayerCell()
    {
        var playerPosition = playerTransform.position;
        _xPlayerCell = (int) MathF.Floor(playerPosition.x / (_chunkSize - 1));
        _zPlayerCell = (int) MathF.Floor(playerPosition.z / (_chunkSize - 1));
    }
    
    private GameObject LoadChunk(Vector2Int chunkPosition, Vector2Int monumentRelative)
    {
        GameObject newChunk = Instantiate(terrainChunk, new Vector3(chunkPosition.x * (_chunkSize - 1), 0, chunkPosition.y * (_chunkSize - 1)), _zeroRotation,transform);
        
        var chunk = newChunk.GetComponent<MeshTerrainChunk>();
        chunk.SetTerrain(_seeds, monumentRelative);

        _loadedChunks.Add(chunkPosition, chunk);

        return newChunk;
    }
    
    private GameObject LoadChunk(Vector2Int chunkPosition, UnityEvent onLoad)
    {
        GameObject newChunk = Instantiate(terrainChunk, new Vector3(chunkPosition.x * (_chunkSize - 1), 0, chunkPosition.y * (_chunkSize - 1)), _zeroRotation,transform);
        
        var chunk = newChunk.GetComponent<MeshTerrainChunk>();
        chunk.SetTerrain(_seeds, onLoad);

        _loadedChunks.Add(chunkPosition, chunk);

        return newChunk;
    } 
}
