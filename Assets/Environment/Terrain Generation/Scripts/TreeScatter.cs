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

        float slope = chunkData.GetSlope(x, z);
        if (slope > 40)
            return;
        
        float moisture = chunkData.GetMoisture(x, z);
        foreach (MyTreeGroup treeGroup in treeGroups)
        {
            if (treeGroup.minMoisture < moisture)
            {
                int treeIndex = Random.Range(0, treeGroup.trees.Length);
                
                Quaternion zero = new Quaternion(0, 0, 0, 0);
                Instantiate(treeGroup.trees[treeIndex], new Vector3(parent.position.x + x, height, parent.position.z + z), zero, parent);
                return;
            } 
        }
    }
}
