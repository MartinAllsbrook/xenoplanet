using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(MeshFilter))]
public class MeshTerrainChunk : MonoBehaviour
{
    // References
    private MapGenerator _mapGenerator;
    private LandMarkGenerator _landMarkGenerator;
    private TreeScatter _treeScatter;
    private EnemySpawner _enemySpawner;
        
    // General Data
    private ChunkData _chunkData;
    private const int Size = 65;
    private Vector2Int _chunkPosition;

    // Mesh Data
    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;
    private Vector2[] _uvs;

    // States
    private bool _placedTrees;

    private void Start()
    {

    }

    public void SetTerrain(int[] seeds)
    {
        _mapGenerator = GetComponent<MapGenerator>();
        _landMarkGenerator = GetComponent<LandMarkGenerator>();
        _treeScatter = GetComponent<TreeScatter>();
        _enemySpawner = GetComponent<EnemySpawner>();
        
        var position = transform.position / (Size - 1);
        _chunkPosition = new Vector2Int((int) position.x, (int) position.z);
        
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        
        _mapGenerator.GenerateMap(_chunkPosition, seeds, chunkData =>
        {
            Debug.Log(chunkData);
            AfterMapsGenerated(chunkData);
        });
    }

    private void AfterMapsGenerated(ChunkData chunkData)
    {
        _chunkData = chunkData;
            
        _landMarkGenerator.PlaceLandMark(ref _chunkData, Size);
            
        // Create Mesh
        CreateShape();
        UpdateMesh();

        _treeScatter.PlaceTrees(_chunkData, Size, () =>
        {
            _placedTrees = true;
        });
    }

    public void AddGrass(ChunkGrassManager grassManager)
    {
        grassManager.PlaceGrass(_chunkData, transform.position);
    }

    /*private void Update()
    {
        UpdateMesh();
    }*/

    #region Mesh Generation

    void CreateShape()
    {
        CreateVertecies();
        CreateTriangles(ref _chunkData);
        CreateUVs();
    }

    private void CreateVertecies()
    {
        _vertices = new Vector3[(Size) * (Size)];

        for (int i = 0, z = 0; z <= Size - 1; z++)
        {
            for (int x = 0; x <= Size - 1; x++)
            {
                _vertices[i] = new Vector3(x, _chunkData.GetHeight(x, z), z);
                i++;
            }
        }
    }

    private void CreateTriangles(ref ChunkData chunkData)
    {
        _triangles = new int[(Size - 1) * (Size - 1) * 6];
        int vertexIndex = 0;
        int trangleIndex = 0;
        
        for (int z = 0; z < Size - 1; z++)
        {
            for (int x = 0; x < Size - 1; x++)
            {
                _triangles[trangleIndex + 0] = vertexIndex + 0;
                _triangles[trangleIndex + 1] = vertexIndex + Size;
                _triangles[trangleIndex + 2] = vertexIndex + 1;
                _triangles[trangleIndex + 3] = vertexIndex + 1;
                _triangles[trangleIndex + 4] = vertexIndex + Size;
                _triangles[trangleIndex + 5] = vertexIndex + Size + 1;
                vertexIndex++;
                trangleIndex += 6;
            }
            // yield return null;
            vertexIndex++;
        }
    }

    private void CreateUVs()
    {
        _uvs = new Vector2[_vertices.Length];
        
        for (int i = 0, z = 0; z <= Size - 1; z++)
        {
            for (int x = 0; x <= Size - 1; x++)
            {
                _uvs[i] = new Vector2((float) x / Size, (float) z / Size);
                i++;
            }
        }
    }
    
    void UpdateMesh()
    {
        _mesh.Clear();
        
        _mesh.vertices = _vertices;
        // _mesh.subMeshCount = 1;
        // _mesh.SetTriangles(_triangles, 0);
        _mesh.triangles = _triangles;
        _mesh.uv = _uvs;
        
        MeshCollider collider = gameObject.AddComponent<MeshCollider>(); //collider.material = physicMaterial;
     
        collider.cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation | MeshColliderCookingOptions.EnableMeshCleaning | MeshColliderCookingOptions.WeldColocatedVertices | MeshColliderCookingOptions.UseFastMidphase;
        collider.convex = false;
        collider.sharedMesh = _mesh;
        collider.enabled = true;

        _mesh.RecalculateNormals();
    }

    #endregion

    private void OnDrawGizmos()
    {
        // if (_vertices == null)
        //     return;
        //
        // for (int i = 0; i < _vertices.Length; i++)
        // {
        //     Gizmos.DrawSphere(_vertices[i], .1f);
        // }
    }
    
    public int GetChunkSize()
    {
        return Size;
    }

    public Vector2Int GetPosition()
    {
        return _chunkPosition;
    }

    public void CheckLatePlace()
    {
        if (!_placedTrees)
        {
            _treeScatter.PlaceTrees(_chunkData, Size, () =>
            {
                _placedTrees = true;
            });
        }
    }
}
