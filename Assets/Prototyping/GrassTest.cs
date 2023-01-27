using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class GrassTest : MonoBehaviour
{ 
    [SerializeField] private GameObject grassPrefab; 
    Mesh mesh;
    Vector3[] vertices;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        
        for (var i = 0; i < vertices.Length; i ++)
        {
            Instantiate(grassPrefab, vertices[i], Quaternion.identity);
            Debug.Log("poop");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
