using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMarkGenerator : MonoBehaviour
{
    [System.Serializable]
    public class LandMark
    {
        public int innerRadius;
        public int outerRadius;
        public GameObject structure;
        
    }
    [SerializeField] public LandMark[] landMarks;
    
    public void PlaceLandMark(ref ChunkData chunkData, int size)
    {
        LandMark landMark = landMarks[Random.Range(0, landMarks.Length)];
        int innerRadius = landMark.innerRadius;
        int outerRadius = landMark.outerRadius;
        int xPosition = Random.Range(outerRadius, size - outerRadius);
        int zPosition = Random.Range(outerRadius, size - outerRadius);

        float slope = chunkData.GetSlope(xPosition, zPosition);
        if (slope > 25)
            return;

        float height = chunkData.GetHeight(xPosition, zPosition);
        if (height < 4)
            return;
            
        for (int x = -outerRadius; x <= outerRadius; x++)
        {
            for (int z = -outerRadius; z <= outerRadius; z++)
            {
                float percent = DistanceBetweenCircles(innerRadius, outerRadius, new Vector2(x, z));
                float newHeight = Mathf.Lerp(chunkData.GetHeight(xPosition + x, zPosition + z), height, percent);
                chunkData.SetHeight(xPosition + x, zPosition + z, newHeight);
                if (percent >= 0.99)
                    chunkData.SetMoisture(xPosition + x, zPosition + z, 0);
            }
        }
        
        chunkData.GeneratePlanes();
        GameObject newLandMark = Instantiate(landMark.structure, new Vector3(xPosition + transform.position.x, height, zPosition + transform.position.z), new Quaternion(0, 0, 0, 0), transform);
        newLandMark.transform.Rotate(0f, Random.Range(0f, 360f), 0f);
    }
    
    public float DistanceBetweenCircles(int innerRadius, int outerRadius, Vector2 testPoint)
    {
        float distance = Vector2.Distance(Vector2.zero, testPoint);
        if (distance <= innerRadius)
            return 1f;
        
        if (distance >= outerRadius)
            return 0f;
        
        float t = (distance - innerRadius) / (outerRadius - innerRadius);
        return Mathf.Lerp(1f, 0f, t);
    }
}
