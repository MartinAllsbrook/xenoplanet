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
    
    public void PlaceLandMark(ref ChunkData chunkData, int size)
    {
        
        // Choose random landmark
        LandMark landMark = landMarks[Random.Range(0, landMarks.Length)];
        
        int width = landMark.width;
        int length = landMark.length;
        int xPosition = Random.Range(width * 2, size - width * 2);
        int zPosition = Random.Range(length * 2, size - length * 2);
        
        int xStart = xPosition - width;
        int xEnd = xPosition + width;
        int xSmoothStart = xPosition - width * 2;
        int xSmoothEnd = xPosition + width * 2;
        
        int zStart = zPosition - length;
        int zEnd = zPosition + length;
        int zSmoothStart = zPosition - length * 2;
        int zSmoothEnd = zPosition + length * 2;
        
        // Debug.Log(
        //     "x: " + xPosition + " " + xSmoothStart + " " + xSmoothEnd + 
        //     " z: " + zPosition + " " + zSmoothStart + " " + zSmoothEnd);
        
        float slope = chunkData.GetSlope(xPosition, zPosition);
        if (slope > 25)
            return;

        float height = chunkData.GetHeight(xPosition, zPosition);
        
        for (int x = xSmoothStart; x <= xSmoothEnd; x++)
        { 
            for (int z = zSmoothStart; z <= zSmoothEnd; z++)
            {
                // If we are near the center of the land mark
                if (x >= xStart && x <= xEnd && z >= zStart && z <= zEnd)
                {
                    chunkData.SetHeight(x, z, height);
                    chunkData.SetMoisture(x, z, 0f);
                }
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
                    
                    var newHeight = height * (1 - percent) + chunkData.GetHeight(x,z) * percent;
                    chunkData.SetHeight(x, z, newHeight);
                }
            }
        }
        chunkData.GeneratePlanes();
        Instantiate(landMark.structure, new Vector3(xPosition + transform.position.x, height, zPosition + transform.position.z), new Quaternion(0, 0, 0, 0), transform);
    }
}
