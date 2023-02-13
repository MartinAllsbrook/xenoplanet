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
    private int[][] _triangles;
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
        _vertices = new Vector3[(_size) * (_size)];

        for (int i = 0, z = 0; z <= _size - 1; z++)
        {
            for (int x = 0; x <= _size - 1; x++)
            {
                _vertices[i] = new Vector3(x, _chunkData.HeightMap[x, z] * maxHeight, z);
                i++;
            }
        }

        _triangles = new int[_chunkData.NumBiomes][];
        List<int>[] triangleSet = new List<int>[_chunkData.NumBiomes];

        for (int i = 0; i < triangleSet.Length; i++)
        {
            triangleSet[i] = new List<int>();
        }
        
        int vertexIndex = 0;
        int trangleIndex = 0;

        for (int z = 0; z < _size - 1; z++)
        {
            for (int x = 0; x < _size - 1; x++)
            { 
                /*float heightSum = 0f;
                heightSum += _vertices[vertexIndex + 0].y;
                heightSum += _vertices[vertexIndex + _size].y;
                heightSum += _vertices[vertexIndex + 1].y;
                heightSum += _vertices[vertexIndex + _size + 1].y;
                float quadHeight = heightSum / 4;*/

                var biomeCode = _chunkData.BiomeMap[x, z];
                
                int setIndex = 0;
                for (int i = 0; i < _chunkData.NumBiomes; i++)
                {
                    if (biomeCode == biomeTextures[i].biomeCode)
                        setIndex = biomeTextures[i].textureIndex;
                }

                triangleSet[setIndex].Add(vertexIndex + 0);
                triangleSet[setIndex].Add(vertexIndex + _size);
                triangleSet[setIndex].Add(vertexIndex + 1);
                triangleSet[setIndex].Add(vertexIndex + 1);
                triangleSet[setIndex].Add(vertexIndex + _size);
                triangleSet[setIndex].Add(vertexIndex + _size + 1);

                vertexIndex++;
                trangleIndex += 6;

            }
            // yield return null;

            vertexIndex++;
        }

        for (int i = 0; i < triangleSet.Length; i++)
        {
            _triangles[i] = new int[triangleSet[i].Count];
            for (int j = 0; j < triangleSet[i].Count; j++)
            {
                _triangles[i][j] = triangleSet[i][j];
            }
        }

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

        _mesh.subMeshCount = _triangles.Length;
        for (int i = 0; i < _triangles.Length; i++)
        {
            // Debug.Log(i);
            _mesh.SetTriangles(_triangles[i], i);
        }
        _mesh.uv = uvs;
        
        MeshCollider collider = gameObject.AddComponent<MeshCollider>();        //collider.material = physicMaterial;
     
        collider.cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation | MeshColliderCookingOptions.EnableMeshCleaning | MeshColliderCookingOptions.WeldColocatedVertices | MeshColliderCookingOptions.UseFastMidphase;
        collider.convex = false;
        collider.sharedMesh = _mesh;
        collider.enabled = true;

        _mesh.RecalculateNormals();
    }

    #endregion

    /*#region Noise Generation

    private void GenerateNoise(int seed, GenericDelegate onFinishedCallback)
    {
        StartCoroutine(GenerateNoiseCoroutine(seed, data =>
            {
                heightMap = data;
                onFinishedCallback?.Invoke();
            }
        ));
    }

    IEnumerator GenerateNoiseCoroutine(int seed, GenericDelegate<float[,]> callback)
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();
    
        
        float[,] noise = new float[_size, _size];

        Vector3 seedOffest = new Vector3(seed, 0, seed);
        Vector3 positionOffset = transform.position;
        Vector3 offset = positionOffset + seedOffest;

        for (int z = 0; z < _size; z++)
        {
            if (timer.ElapsedMilliseconds > 3)
            {
                yield return null;
                timer.Reset();
                timer.Start();
            }

            for (int x = 0; x < _size; x++)
            {
                noise[x, z] = CompileNoise(x, z, offset);
            }
        }
        timer.Stop();
        // Debug.Log("Total time: " + timer.ElapsedMilliseconds);
        
        callback(noise);
        yield return null;
    }
    
    float CompileNoise(int x, int z, Vector3 position)
    {
        float value = 0;
        float octaveSum = 0f;

        // float xNorm = (x + position.z - (position.z / _size)) / _size;
        // float zNorm = (z + position.x - (position.x / _size)) / _size;
        
        float xNorm = (x + position.x) / _size;
        float zNorm = (z + position.z) / _size;
    
        for (int i = 0; i < octaves.Length; i++)
        {
            value += (1/octaves[i]) * CalculateNoise(xNorm, zNorm, octaves[i]);
            octaveSum += 1/octaves[i];
        }
        value /= octaveSum;

        value = Mathf.Pow(value, redistributionFactor);
    
        // Debug.Log(height);
        return value;
    }
    
    float CalculateNoise(float xNorm, float zNorm, float scale)
    {
        xNorm *= scale;
        zNorm *= scale;

        // return (NoiseManager.SimplexPerlin.GetValue(xNorm, yNorm) + 1)/1;
        return Mathf.PerlinNoise(xNorm, zNorm);
    }

    #endregion*/

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
