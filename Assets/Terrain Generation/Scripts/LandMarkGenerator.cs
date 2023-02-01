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
        
        int width = landMark.width;
        int length = landMark.length;
        int xPosition = Random.Range(width * 2, 512 - width * 2);
        int zPosition = Random.Range(length * 2, 512 - length * 2);
        
        int xStart = xPosition - width;
        int xEnd = xPosition + width;
        int xSmoothStart = xPosition - width * 2;
        int xSmoothEnd = xPosition + width * 2;
        
        int zStart = zPosition - length;
        int zEnd = zPosition + length;
        int zSmoothStart = zPosition - length * 2;
        int zSmoothEnd = zPosition + length * 2;
        
        float height = heightMap[zPosition, xPosition];
        
        for (int x = xSmoothStart; x <= xSmoothEnd; x++)
        { 
            for (int z = zSmoothStart; z <= zSmoothEnd; z++)
            {
                // If we are near the center of the land mark
                if (x >= xStart && x <= xEnd && z >= zStart && z <= zEnd)
                    heightMap[z, x] = height;
                // Else smooth the transition
                else
                {
                    int xDistance = Mathf.Abs(x - xPosition);
                    int zDistance = Mathf.Abs(z - zPosition);
                    
                    // Use the greater distance
                    int usedDistance;
                    float percent;
                    if (xDistance < zDistance)
                    {
                        usedDistance = zDistance;
                        usedDistance -= length;
                        percent = (float) usedDistance / length;

                    }
                    else
                    {
                        usedDistance = xDistance;
                        usedDistance -= width;
                        percent = (float) usedDistance / width;
                    }
                    
                    heightMap[z,x] = height * (1-percent) + heightMap[z,x] * percent;
                }
            }
        }

        Instantiate(landMark.structure, new Vector3(xPosition + transform.position.x, height*512, zPosition + transform.position.z), new Quaternion(0, 0, 0, 0));
    }
}
