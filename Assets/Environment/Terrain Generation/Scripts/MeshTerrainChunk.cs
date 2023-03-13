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
    
    /*[Serializable]
    class BiomeTexture
    {
        public int biomeCode;
        public int textureIndex;
    }*/

    // [SerializeField] private BiomeTexture[] biomeTextures;
    [SerializeField] private ChunkGrassManager chunkGrassManager;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private LandMarkGenerator landMarkGenerator;
    [SerializeField] private TreeScatter treeScatter;
    private const int Size = 65;
    
    private bool _placedGrass;
    private bool _placedTrees;

    public void SetTerrain(int[] seeds, bool makeActive)
    {
        var position = transform.position / (Size - 1);
        _chunkPosition = new Vector2Int((int) position.x, (int) position.z);
        // Debug.Log(_chunkPosition);
        
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        
        mapGenerator.GenerateMap(_chunkPosition, seeds, chunkData =>
        {
            _chunkData = chunkData;
            
            landMarkGenerator.PlaceLandMark(ref _chunkData, Size);
            
            // Create Mesh
            CreateShape();
            UpdateMesh();

            // If this chunk is being made active load the trees and grass 
            if (makeActive)
            {
                chunkGrassManager.PlaceGrass(_chunkData, () =>
                {
                    _placedGrass = true;
                });
                
                treeScatter.PlaceTrees(_chunkData, Size, () =>
                {
                    _placedTrees = true;
                });
            }
            else // Else set it to be inactive
                gameObject.SetActive(false);
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
        uvs = new Vector2[_vertices.Length];
        
        for (int i = 0, z = 0; z <= Size - 1; z++)
        {
            for (int x = 0; x <= Size - 1; x++)
            {
                uvs[i] = new Vector2((float) x / Size, (float) z / Size);
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
        if (!_placedGrass)
        {
            chunkGrassManager.PlaceGrass(_chunkData, () =>
            {
                _placedGrass = true;
            });
        }

        if (!_placedTrees)
        {
            treeScatter.PlaceTrees(_chunkData, Size, () =>
            {
                _placedGrass = true;
            });
        }
    }
}
