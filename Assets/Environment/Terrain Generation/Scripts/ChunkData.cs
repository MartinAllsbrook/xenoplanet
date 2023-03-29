using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkData
{
    // private Vector2Int _position;
    private float[,] _heightMap;
    private float[,] _moistureMap;
    private float[,] _strangenessMap;
    private int _size;
    public int[,] BiomeMap;
    public int NumBiomes;
    private Plane[,] _planes;

    public ChunkData(float[,] heightMap, float[,] moistureMap, float[,] strangenessMap)
    {
        _heightMap = heightMap;
        _moistureMap = moistureMap;
        _strangenessMap = strangenessMap;
        _size = heightMap.GetLength(0);
        GeneratePlanes();
        // BiomeMap = biomeMap;
        // NumBiomes = numBiomes;
    }

    public void GeneratePlanes()
    {
        _planes = new Plane[_size - 1, _size - 1];

        for (int x = 0; x < _size - 1; x++)
        {
            for (int z = 0; z < _size - 1; z++)
            {
                Vector3 a = new Vector3(x, _heightMap[x, z], z);
                Vector3 b = new Vector3(x + 1, _heightMap[x + 1, z], z);
                Vector3 c = new Vector3(x, _heightMap[x, z + 1], z + 1);
                _planes[x,z] = new Plane(a, b, c);
            }
        }
    }

    public float GetHeight(float x, float z)
    {
        int xFloor = (int) Mathf.Floor(x);
        int zFloor = (int) Mathf.Floor(z);

        if (x == xFloor && z == zFloor)
            return _heightMap[xFloor, zFloor];

        // Debug.Log("xFloor: " + xFloor + " zFloor: " + zFloor);
        Plane plane = _planes[xFloor, zFloor];
        Ray ray = new Ray(new Vector3(x, 0, z), Vector3.up);
        plane.Raycast(ray, out float y);
        // Debug.Log(y);
        return y;
    }

    public float GetMoisture(float x, float z)
    {
        int xFloor = (int) Mathf.Floor(x);
        int zFloor = (int) Mathf.Floor(z);

        return _moistureMap[xFloor, zFloor];
    }

    public float GetSlope(float x, float z)
    {
        int xFloor = (int) Mathf.Floor(x);
        int zFloor = (int) Mathf.Floor(z);
        
        Vector3 normal = _planes[xFloor, zFloor].normal;

        return Vector3.Angle(normal, Vector3.up);
    }

    public Vector3 GetNormal(float x, float z)
    {
        int xFloor = (int) Mathf.Floor(x);
        int zFloor = (int) Mathf.Floor(z);
        
       return -_planes[xFloor, zFloor].normal;
    }

    public void SetHeight(int x, int z, float height)
    {
        _heightMap[x, z] = height;
    }
    
    public void SetMoisture(int x, int z, float moisture)
    {
        _moistureMap[x, z] = moisture;
    }
}