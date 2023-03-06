using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainScatter : MonoBehaviour
{
    // private TerrainLoader.SplatInfo[] splatInfo;
    private TerrainLoader.BiomeInfo[] biomeInfo;

    // public GameObject[] scatter;

    [SerializeField] private int treeCount;
    [Range(-1, 1)] [SerializeField] private float maxOffset;
    
    [SerializeField] private int patchDetail;
    
    private float xOffset;
    private float zOffset;
    private float roadWidth;
    private Terrain terrain;
    
    //Grass Test
    [SerializeField] private GameObject grassPrefab;
    // [SerializeField] private float grassHeightThreshold = 10.0f;
    // [SerializeField] private float voronoiScale = 10.0f;
    [SerializeField] private int grassDensity;
    
    public void ScatterFoliage(Terrain passedTerrain, int[,] biomeMap, float[,] heightMap)
    {
        biomeInfo = TerrainLoader.Instance.biomeInfo;
        // splatInfo = TerrainLoader.Instance.splatInfo;

        terrain = passedTerrain;
        xOffset = transform.position.x;
        zOffset = transform.position.z;
        roadWidth = TerrainLoader.Instance.roadwidth;
        ScatterGrass(biomeMap, heightMap);
        ScatterTrees(biomeMap);

    }

    void ScatterGrass(int[,] biomeMap, float[,] heightMap)
    {
        
        // terrain.terrainData.SetDetailResolution(grassDensity, patchDetail);
        
        // int[,] newMap = new int[grassDensity, grassDensity];
        
        
        for (int xIterator = 0; xIterator < 24; xIterator++)
        {
            for (int zIterator = 0; zIterator < 24; zIterator++)
            {
                int x = xIterator;
                int z = zIterator;
                var biome = biomeMap[x, z];
                for (int i = 1; i < biomeInfo.Length; i++)
                {
                    for (int j = 1; j < 4; j++)
                    {
                        if (biome == 0)
                        {
                            return;
                        }
                        /*if (biome == biomeInfo[i].moistureInfo[j].biomeIndex)
                        {
                            // Debug.Log("x: " + x + " z: " + z + " i: " + i + " j: " + j + " Grasses: " + biomeInfo[i].moistureInfo[j].grasses.Length);
                            if (biomeInfo[i].moistureInfo[j].grasses.Length > 0)
                            {
                                newMap[x, z] = biomeInfo[i].moistureInfo[j].grasses[0];
                            }
                        }*/
                        Instantiate(grassPrefab, new Vector3(x + transform.position.x, heightMap[z, x] * 512, z + transform.position.z), new Quaternion(0, 0, 0, 0), transform);
                    }
                }
            }
        }
        
        // terrain.terrainData.SetDetailLayer(0, 0, 0, newMap);
    }
    

    void ScatterTrees(int[,] biomeMap)
    {
        for (int i = 0; i < treeCount; i++)
        {
            for (int j = 0; j < treeCount; j++)
            {
                Vector3 potentialPosition = new Vector3((i + Random.Range(-maxOffset, maxOffset)) / treeCount, 0, (j + Random.Range(-maxOffset, maxOffset)) / treeCount);
                TestTree(potentialPosition, biomeMap);
            }
        }
    }

    void TestTree(Vector3 potentialPosition, int[,] biomeMap)
    {
        Vector3Int potentialPositionInt = Vector3Int.FloorToInt(potentialPosition * 513);

        // If position is off terrain don't place
        if (potentialPositionInt.x < 0 || potentialPositionInt.z < 0 ||
            potentialPositionInt.x > 513 || potentialPositionInt.z > 513)
            return;
        
        int biome = biomeMap[potentialPositionInt.z, potentialPositionInt.x];

        for (int i = 1; i < biomeInfo.Length; i++)
        {
            for (int j = 1; j < 4; j++)
            {
                if (biome == 0)
                {
                    return;
                }
                else if (biome == biomeInfo[i].moistureInfo[j].biomeIndex && biomeInfo[i].moistureInfo[j].trees.Length > 0)
                {
                    PlaceTree(potentialPosition, biomeInfo[i].moistureInfo[j].trees);
                    return;
                }
            }
        }
    }

    void PlaceTree(Vector3 position, int[] trees)
    {
        TreeInstance treeInstance = new TreeInstance();
        treeInstance.prototypeIndex = trees[Random.Range(0, trees.Length)];

        treeInstance.heightScale = 1;
        treeInstance.widthScale = 1;
        treeInstance.color = new Color32(255, 255, 255, 255);
        treeInstance.position = position;
        
        terrain.AddTreeInstance(treeInstance);
    }
}
