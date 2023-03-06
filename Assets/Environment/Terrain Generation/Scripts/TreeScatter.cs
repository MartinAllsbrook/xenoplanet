using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScatter : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] private float treeUniformity;
    [SerializeField] private int numTrees;
    [SerializeField] private GameObject[] trees;
    [SerializeField] private float minHeight;
    [SerializeField] private float minMoisture;
    
    public void PlaceTrees(ChunkData chunkData, int size)
    {
        StartCoroutine(PlaceTreesRoutine(transform, chunkData, size));
    }
    private IEnumerator PlaceTreesRoutine(Transform parent, ChunkData chunkData, int size)
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
    }
    
    
}
