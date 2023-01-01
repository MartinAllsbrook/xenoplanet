using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMarkGenerator : MonoBehaviour
{
    [System.Serializable]
    public class LandMark
    {
        // public int textureIndex;
        public int width;
        public int length;
        public GameObject structure;

    }
    [SerializeField] public LandMark[] landMarks;
    
    public void PlaceLandMark(ref float[,] heightMap)
    {
        // Choose random landmark
        LandMark landMark = landMarks[Random.Range(0, landMarks.Length)];
        
        int radius = landMark.width;
        int position = Random.Range(radius, 512-radius);
        
        int start = position - radius;
        int end = position + radius;
        float height = heightMap[position, position];
        
        for (int x = start; x <= end; x++)
        { 
            for (int z = start; z <= end; z++)
            {
                heightMap[z, x] = height;
            }
        }

        Instantiate(landMark.structure, new Vector3(position + transform.position.x, height*512, position + transform.position.z), new Quaternion(0, 0, 0, 0));
    }
}
