using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainLoader : MonoBehaviour
{

    public GameObject[,] loadedChunks;

    [SerializeField] private float seed;
    [SerializeField] private GameObject terrain;

    [SerializeField] private int loadDistance;

    [SerializeField] private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                Instantiate(terrain, new Vector3(i*513f, 0, j*513f), new Quaternion(0,0,0,0));
                // newChunk.GetComponent<TerrainGenerator>().Generate(i*50000);
                // loadedChunks[i, j] = newChunk;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Set position to the same position as the player
        var playerPosition = player.position;
        transform.position = new Vector3(playerPosition.x, 0, playerPosition.z);
        
        
    }
}
