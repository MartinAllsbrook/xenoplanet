using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScatter : MonoBehaviour
{
    public GameObject[] scatter;

    [SerializeField] private int treeCount;
    [Range(-1, 1)] [SerializeField] private float maxOffset; 
    public void ScatterFoliage(Terrain terrain)
    {
        for (int i = 0; i < treeCount; i++)
        {
            for (int j = 0; j < treeCount; j++)
            {
                TreeInstance treeInstance = new TreeInstance();
                treeInstance.prototypeIndex = 0;
                treeInstance.color = new Color32(255, 255, 255, 255);
                treeInstance.heightScale = 10000;
                treeInstance.position = new Vector3((i + Random.Range(-maxOffset, maxOffset)) / treeCount, 0, (j + Random.Range(-maxOffset, maxOffset)) / treeCount);
                treeInstance.widthScale = 10000;
                terrain.AddTreeInstance(treeInstance);
            }
        }
    }
}
