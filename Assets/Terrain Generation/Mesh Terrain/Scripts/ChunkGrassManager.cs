using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGrassManager : MonoBehaviour
{
    [SerializeField] private GameObject patch;
    [SerializeField] private GameObject[] grass;

    public void PlaceGrass(float[,] heightMap, float maxHeight)
    {
        for (int i = 0; i < grass.Length; i++)
        {
            // Vector3 position = grass[i].transform.position;
            var x = Mathf.FloorToInt(i / 32f);
            var z = i % 32;
            Debug.Log("i: " + i + " x: " + x + " z: " + z);
            grass[i].transform.position = new Vector3(x, heightMap[x, z] * maxHeight, z);
        }
    }
}
