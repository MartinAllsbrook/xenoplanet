using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrassTest : MonoBehaviour
{ 
    [SerializeField] private GameObject grassPrefab;
    // [SerializeField] private float grassHeightThreshold = 10.0f;
    // [SerializeField] private float voronoiScale = 10.0f;
    // [SerializeField] private float grassDensity;
    // public Terrain terrain;

    private Mesh mesh;
    private Vector3[] vertices; 
    
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        
        for (var i = 0; i < vertices.Length; i ++)
        {
            Instantiate(grassPrefab, vertices[i], Quaternion.identity);
            Debug.Log("poop");
        }
        // Debug.Log("poopTest");
        //     float[,] heights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution,
        //         terrain.terrainData.heightmapResolution);
        //
        //     for (int i = 0; i < terrain.terrainData.heightmapResolution; i++)
        //     {
        //         for (int j = 0; j < terrain.terrainData.heightmapResolution; j++)
        //         {
        //             float height = heights[i, j];
        //             float voronoi = Mathf.PerlinNoise(i / voronoiScale, j / voronoiScale);
        //             if (height < grassHeightThreshold && voronoi > 0.5f)
        //             {
        //                 // Vector3 spawnPos = terrain.terrainData.GetInterpolatedNormal(i / heightmapResolution, j / heightmapResolution)
        //             
        //                 float terrainSize = terrain.terrainData.size.x;
        //                 float x = i / (float)terrain.terrainData.heightmapResolution * terrainSize;
        //                 float z = j / (float)terrain.terrainData.heightmapResolution * terrainSize;
        //                 float y = terrain.SampleHeight(new Vector3(x, 0, z));
        //                 Vector3 spawnPos = new Vector3(x, y, z);
        //             
        //             
        //                 float random = Random.Range(0.0f, 1.0f);
        //                 // if (random < grassDensity * (0.9f - voronoi))
        //                 if (random < grassDensity)
        //                 {
        //                     Debug.Log("poop");
        //                     Instantiate(grassPrefab, spawnPos, Quaternion.identity, terrain.transform);
        //                 }
        //             }
        //         }
        //     }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
