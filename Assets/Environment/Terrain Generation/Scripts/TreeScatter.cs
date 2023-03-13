using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreeScatter : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] private float treeUniformity;
    [SerializeField] private int numTrees;
    [SerializeField] private GameObject[] trees;
    [SerializeField] private float minHeight;
    [SerializeField] private float minMoisture;
    
    public delegate void GenericDelegate();

    public void PlaceTrees(ChunkData chunkData, int size, GenericDelegate callBack)
    {
        StartCoroutine(PlaceTreesRoutine(transform, chunkData, size, callBack));
    }
    private IEnumerator PlaceTreesRoutine(Transform parent, ChunkData chunkData, int size, GenericDelegate callBack)
    {
        Quaternion zero = new Quaternion(0, 0, 0, 0);
        for (int xI = 0; xI < numTrees; xI++)
        {
            for (int zI = 0; zI < numTrees; zI++)
            {
                float treeSpacing = (float) size / numTrees;
                float x = xI * treeSpacing + Random.Range(0f, treeSpacing / treeUniformity);
                float z = zI * treeSpacing + Random.Range(0f, treeSpacing / treeUniformity);

                int treeIndex = Random.Range(0, trees.Length);

                float height = chunkData.GetHeight(x, z);
                float moisture = chunkData.GetMoisture(x, z);
                if (height > minHeight && moisture > minMoisture)
                {
                    Instantiate(trees[treeIndex], new Vector3(parent.position.x + x, height, parent.position.z + z), zero, parent);
                }

                
                yield return null;
            }
        }

        callBack();
    }
    
    
}
