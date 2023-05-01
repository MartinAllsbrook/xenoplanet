using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class MonumentGenerator : MonoBehaviour
{
    [SerializeField] private GameObject monument;
    [SerializeField] private int innerRadius;
    [SerializeField] private int outerRadius;
    [SerializeField] private Vector3 monumentPosition;
    [SerializeField] private AnimationCurve terraformAuthority;
    public void Generate(Vector2Int relativeChunkPosition, ChunkData chunkData)
    {
        if (relativeChunkPosition.x == 0 && relativeChunkPosition.y == 0)
        {
            SpawnMonument(chunkData);    
        }

        Vector3 relativeVector3 = new Vector3(relativeChunkPosition.x, 0, relativeChunkPosition.y);
        Vector3 relativeOrigin = relativeVector3 * (chunkData.GetSize() - 1);
        BlendTerrain(relativeOrigin, chunkData);
        
    }

    private void SpawnMonument(ChunkData chunkData)
    {
        Vector3 position = monumentPosition + transform.position;
        Quaternion zero = new Quaternion(0, 0, 0, 0);
        Instantiate(monument, position, zero);
    }

    private void BlendTerrain(Vector3 relativeOrigin, ChunkData chunkData)
    {
        for (int x = 0; x < chunkData.GetSize(); x++)
        {
            for (int z = 0; z < chunkData.GetSize(); z++)
            {
                Vector2 testPoint = new Vector2(relativeOrigin.x + x - monumentPosition.x, relativeOrigin.z + z - monumentPosition.z);
                float percent = DistanceBetweenCircles(testPoint);
                
                float terrainHeight = chunkData.GetHeight(x, z);
                float newHeight = Mathf.Lerp(terrainHeight, monumentPosition.y, percent);

                chunkData.SetHeight(x, z, newHeight);
                if (percent >= 0.99)
                    chunkData.SetMoisture(x, z, 0);
            }
        }
    }
    
    private float DistanceBetweenCircles(Vector2 testPoint)
    {
        float distance = Vector2.Distance(Vector2.zero, testPoint);
        if (distance <= innerRadius)
            return 1f;
        
        if (distance >= outerRadius)
            return 0f;
        
        float t = (distance - innerRadius) / (outerRadius - innerRadius);
        float percent = Mathf.Lerp(1f, 0f, t);
        return terraformAuthority.Evaluate(t);
    }
}
