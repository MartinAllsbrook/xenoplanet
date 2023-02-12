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
    
    private Vector3[] _vertices;
    private int[] _triangles;
    private Vector2[] uvs;

    [SerializeField] private float[] octaves;
    [SerializeField] private float redistributionFactor;
    [SerializeField] private float maxHeight;

    // private TerrainPainter terrainPainter;
    // private TerrainScatter terrainScatter;
    // private BiomeGenerator biomeGenerator;
    // private LandMarkGenerator landMarkGenerator;
    
    private const int _size = 65;

    // private UnityEvent chunkLoaded;
    
    private float[,] heightMap;
    private float[,] moistureMap;
    private int[,] biomeMap;

    private delegate void GenericDelegate();
    private delegate void GenericDelegate<T>(T variable);
    
    public void SetTerrain(int seed)
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        GenerateNoise(seed, () =>
        {
            // StartCoroutine(CreateShape());
            CreateShape();
            UpdateMesh();
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
                _vertices[i] = new Vector3(x, heightMap[x, z] * maxHeight, z);
                i++;
            }
        }

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
        _mesh.SetTriangles(_triangles, 0);
        _mesh.uv = uvs;
        
        MeshCollider collider = gameObject.AddComponent<MeshCollider>();        //collider.material = physicMaterial;
     
        collider.cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation | MeshColliderCookingOptions.EnableMeshCleaning | MeshColliderCookingOptions.WeldColocatedVertices | MeshColliderCookingOptions.UseFastMidphase;
        collider.convex = false;
        collider.sharedMesh = _mesh;
        collider.enabled = true;


        _mesh.RecalculateNormals();
    }

    #endregion

    #region Noise Generation

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
}
