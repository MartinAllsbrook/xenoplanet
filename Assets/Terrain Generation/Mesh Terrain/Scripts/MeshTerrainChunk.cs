using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(MeshFilter))]
public class MeshTerrainChunk : MonoBehaviour
{
    private Mesh _mesh;
    private ChunkData _chunkData;
    private Vector3[] _vertices;
    private int[] _triangles;
    private Vector2[] uvs;
    private Vector2Int _chunkPosition;
    
    [Serializable]
    class BiomeTexture
    {
        public int biomeCode;
        public int textureIndex;
    }

    [SerializeField] private BiomeTexture[] biomeTextures;
    
    [SerializeField] private float maxHeight;
    
    [SerializeField] private ChunkGrassManager chunkGrassManager;
    [SerializeField] private MapGenerator mapGenerator;
    
    private const int _size = 65;
    
    public void SetTerrain(int[] seeds)
    {
        var position = transform.position / (_size - 1);
        _chunkPosition = new Vector2Int((int) position.x, (int) position.z);
        // Debug.Log(_chunkPosition);
        
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        
        mapGenerator.GenerateMap(_chunkPosition, seeds, chunkData =>
        {
            _chunkData = chunkData;
            Debug.Log(_chunkData.HeightMap);
            CreateShape();
            UpdateMesh();
            chunkGrassManager.PlaceGrass(_chunkData, maxHeight);
        });
    }

    /*private void Update()
    {
        UpdateMesh();
    }*/

    #region Mesh Generation

    void CreateShape()
    {
        CreateVertecies();
        CreateTriangles();
        CreateUVs();
    }

    private void CreateVertecies()
    {
        _vertices = new Vector3[(_size) * (_size)];

        for (int i = 0, z = 0; z <= _size - 1; z++)
        {
            for (int x = 0; x <= _size - 1; x++)
            {
                _vertices[i] = new Vector3(x, _chunkData.HeightMap[x, z] * maxHeight, z);
                i++;
            }
        }
    }

    private void CreateTriangles()
    {
        _triangles = new int[(_size - 1) * (_size - 1) * 6];
        int vertexIndex = 0;
        int trangleIndex = 0;
        
        for (int z = 0; z < _size - 1; z++)
        {
            for (int x = 0; x < _size - 1; x++)
            {
                _triangles[trangleIndex + 0] = vertexIndex + 0;
                _triangles[trangleIndex + 1] = vertexIndex + _size;
                _triangles[trangleIndex + 2] = vertexIndex + 1;
                _triangles[trangleIndex + 3] = vertexIndex + 1;
                _triangles[trangleIndex + 4] = vertexIndex + _size;
                _triangles[trangleIndex + 5] = vertexIndex + _size + 1;
                vertexIndex++;
                trangleIndex += 6;
            }
            // yield return null;
            vertexIndex++;
        }
    }

    private void CreateUVs()
    {
        uvs = new Vector2[_vertices.Length];
        
        for (int i = 0, z = 0; z <= _size - 1; z++)
        {
            for (int x = 0; x <= _size - 1; x++)
            {
                uvs[i] = new Vector2((float) x / _size, (float) z / _size);
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
        _mesh.uv = uvs;
        
        MeshCollider collider = gameObject.AddComponent<MeshCollider>();        //collider.material = physicMaterial;
     
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
        return _size;
    }

    public Vector2Int GetPosition()
    {
        return _chunkPosition;
    }
}
