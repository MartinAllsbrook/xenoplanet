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
        int position = Random.Range(radius * 2, 512 - radius * 2);
        
        int start = position - radius;
        int end = position + radius;
        int smoothStart = position - radius * 2;
        int smoothEnd = position + radius * 2;
        float height = heightMap[position, position];
        
        for (int x = smoothStart; x <= smoothEnd; x++)
        { 
            for (int z = smoothStart; z <= smoothEnd; z++)
            {
                // If we are near the center of the land mark
                if (x >= start && x <= end && z >= start && z <= end)
                    heightMap[z, x] = height;
                // Else smooth the transition
                else
                {
                    int distance = Mathf.Abs(x - position);
                    if (distance < Mathf.Abs(z - position))
                        distance = Mathf.Abs(z - position);
                    distance -= radius;
                    float percent = (float) distance / radius;
                    Debug.Log(percent);
                    heightMap[z,x] = height * (1-percent) + heightMap[z,x] * percent;
                }
            }
        }

        Instantiate(landMark.structure, new Vector3(position + transform.position.x, height*512, position + transform.position.z), new Quaternion(0, 0, 0, 0));
    }
}
