using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreeScatter : MonoBehaviour
{
    [Serializable]
    public class MyTreeGroup
    {
        public GameObject[] trees;
        public float minMoisture;
    }
    
    [Range(1, 10)]
    [SerializeField] private float treeUniformity;
    [SerializeField] private int numTrees;
    [SerializeField] private MyTreeGroup[] treeGroups;
    [SerializeField] private float minHeight;
    [SerializeField] private float minMoisture;
    [SerializeField] private float rockSpawnChance;
    [SerializeField] private GameObject[] rocks;

    private Quaternion _zero = new Quaternion(0, 0, 0, 0);
    
    public delegate void GenericDelegate();

    public void PlaceTrees(ChunkData chunkData, int size, GenericDelegate callBack)
    {
        StartCoroutine(PlaceTreesRoutine(transform, chunkData, size, callBack));
    }
    
    private IEnumerator PlaceTreesRoutine(Transform parent, ChunkData chunkData, int size, GenericDelegate callBack)
    {
        for (int xI = 0; xI < numTrees; xI++)
        {
            for (int zI = 0; zI < numTrees; zI++)
            {
                float treeSpacing = (float) size / numTrees;
                float x = xI * treeSpacing + Random.Range(0f, treeSpacing / treeUniformity);
                float z = zI * treeSpacing + Random.Range(0f, treeSpacing / treeUniformity);
                
                PlaceTree(x, z, parent, chunkData);
                
                yield return null;
            }
        }

        callBack();
    }

    private void PlaceTree(float x, float z, Transform parent, ChunkData chunkData)
    {
        float height = chunkData.GetHeight(x, z);
        if (height < minHeight)
            return;

        if (Random.Range(0, 101) < rockSpawnChance)
        {
            SpawnRock(x, z, height, parent, chunkData);
            return;
        }

        float slope = chunkData.GetSlope(x, z);
        if (slope > 40)
            return;
        
        float moisture = chunkData.GetMoisture(x, z);
        foreach (MyTreeGroup treeGroup in treeGroups)
        {
            if (treeGroup.minMoisture < moisture)
            {
                int treeIndex = Random.Range(0, treeGroup.trees.Length);
                
                GameObject tree = Instantiate(treeGroup.trees[treeIndex], new Vector3(parent.position.x + x, height, parent.position.z + z), _zero, parent);
                tree.transform.Rotate(0, Random.Range(0,360), 0);
                return;
            } 
        }
    }

    private void SpawnRock(float x, float z, float height, Transform parent, ChunkData chunkData)
    {
        int rockIndex = Random.Range(0, rocks.Length);
        GameObject rock = rocks[rockIndex];
        
        Vector3 position = new Vector3(transform.position.x + x, height, transform.position.z + z);
        GameObject newRock = Instantiate(rock, position, _zero, parent);
        
        newRock.transform.up = chunkData.GetNormal(x, z);
        newRock.transform.Rotate(0, Random.Range(0,360), 0);
    }
}
