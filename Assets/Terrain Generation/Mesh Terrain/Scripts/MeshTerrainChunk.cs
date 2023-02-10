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

    [SerializeField] private float[] octaves;
    [SerializeField] private float redistributionFactor;
    [SerializeField] private float maxHeight;

    // private TerrainPainter terrainPainter;
    // private TerrainScatter terrainScatter;
    // private BiomeGenerator biomeGenerator;
    // private LandMarkGenerator landMarkGenerator;
    
    private const int _size = 64;

    // private UnityEvent chunkLoaded;
    
    private float[,] heightMap;
    private float[,] moistureMap;
    private int[,] biomeMap;

    private delegate void GenericDelegate();
    private delegate void GenericDelegate<T>(T variable);
    
    // Start is called before the first frame update
    void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        GenerateNoise(100, () =>
        {
            StartCoroutine(CreateShape());
        });
        
        // CreateShape();
    }

    private void Update()
    {
        UpdateMesh();
    }

    #region Mesh Generation

    IEnumerator CreateShape()
    {
        _vertices = new Vector3[(_size) * (_size)];

        for (int i = 0, z = 0; z <= _size - 1; z++)
        {
            for (int x = 0; x <= _size - 1; x++)
            {
                _vertices[i] = new Vector3(x, heightMap[x,z] * maxHeight, z);
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
            yield return null;

            vertexIndex++;
        }
    }

    void UpdateMesh()
    {
        _mesh.Clear();

        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        
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

        Vector3 position = transform.position + new Vector3(seed, 0, seed);

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
                noise[z, x] = CompileNoise(z, x, position);
            }
        }
        timer.Stop();
        // Debug.Log("Total time: " + timer.ElapsedMilliseconds);
        
        callback(noise);
    }
    
    float CompileNoise(int x, int y, Vector3 position)
    {
        float value = 0;
        float octaveSum = 0f;

        float xNorm = (x + position.z - (position.z / 513)) / _size;
        float yNorm = (y + position.x - (position.x / 513)) / _size;
    
        for (int i = 0; i < octaves.Length; i++)
        {
            value += (1/octaves[i]) * CalculateNoise(xNorm, yNorm, octaves[i]);
            octaveSum += 1/octaves[i];
        }
        value /= octaveSum;

        value = Mathf.Pow(value, redistributionFactor);
    
        // Debug.Log(height);
        return value;
    }
    
    float CalculateNoise(float xNorm, float yNorm, float scale)
    {
        xNorm *= scale;
        yNorm *= scale;

        // return (NoiseManager.SimplexPerlin.GetValue(xNorm, yNorm) + 1)/1;
        return Mathf.PerlinNoise(xNorm, yNorm);
    }

    private void OnDrawGizmos()
    {
        if (_vertices == null)
            return;
        
        for (int i = 0; i < _vertices.Length; i++)
        {
            Gizmos.DrawSphere(_vertices[i], .1f);
        }
    }

    #endregion
}
