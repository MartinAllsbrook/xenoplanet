using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScatter : MonoBehaviour
{
    public GameObject[] scatter;
    
    public void ScatterFoliage(Terrain terrain)
    {
        TreeInstance treeInstance = new TreeInstance();
        Debug.Log(terrain.terrainData.treePrototypes);
        treeInstance.prototypeIndex = 0;
        treeInstance.color = new Color32(255, 255, 255, 255);
        treeInstance.heightScale = 10;
        treeInstance.position = new Vector3(0.5f, 0, 0.5f);
        treeInstance.widthScale = 10;
        Debug.Log(terrain.terrainData.treeInstances);
        terrain.AddTreeInstance(treeInstance);
        Debug.Log(terrain.terrainData.treeInstances);

    }
}
