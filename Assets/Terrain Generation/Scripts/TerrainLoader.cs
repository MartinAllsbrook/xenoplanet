using System;
// using Graphics.Tools.Noise;
// using Graphics.Tools.Noise.Primitive;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

/*public static class NoiseManager
{
    private static SimplexPerlin simplexPerlin;
    public static SimplexPerlin SimplexPerlin
    {
        get
        {
            if (simplexPerlin == null)
            {
                simplexPerlin = new SimplexPerlin(Seed, NoiseQuality.Fast);
            }

            return simplexPerlin;
        }
    }

    public static int Seed { get; set; } = 5000;
}*/

public class TerrainLoader : MonoBehaviour
{
    public static TerrainLoader Instance;
    
    [System.Serializable]
    public class MoistureInfo
    {
        public float moistureLevel;
        public int splat;
        public int[] trees;
        public int[] grasses;
        public int biomeIndex;
    }
    
    [System.Serializable]
    public class BiomeInfo
    {
        // public int textureIndex;
        public float maxHeight;
        
        public MoistureInfo[] moistureInfo;
    }

    [SerializeField] public BiomeInfo[] biomeInfo;
    
    public int seed;
    public int moistureSeed;
    public UnityEvent chunkLoaded;
    public UnityEvent terrainReady;
    
    private int xPlayerCell;
    private int zPlayerCell;


    public GameObject[,] loadedChunks;
    [SerializeField] private GameObject terrain;
    [SerializeField] private int loadDistance;
    // [SerializeField] private Transform player;
    private int numChunks;
    private int numChunksLoaded = 0;

    // Road information
    public int roadwidth;
    // public int roadSmoothFactor;
    /*[System.Serializable]
    public class SplatInfo
    {
        // public int textureIndex;
        public float moistureStart;
        public float moistureEnd;
        public float heightStart;
        public float heightEnd;
    }
    [SerializeField] public SplatInfo[] splatInfo;*/
    
    private void Awake()
    {
        // Create singleton
        if (Instance == null) Instance = this;
        
        // Create random seed
        seed = Random.Range(10000, 20000);
        moistureSeed = Random.Range(10000, 20000);
        
        // Deal with events
        if (chunkLoaded == null) chunkLoaded = new UnityEvent();
        if (terrainReady == null) terrainReady = new UnityEvent();
        chunkLoaded.AddListener(OnTerrainReady);
        numChunks = (loadDistance * 2 + 1) * (loadDistance * 2 + 1);
        
    }

    // Sends out event when the terrain is loaded and ready for action
    private void OnTerrainReady()
    {
        numChunksLoaded++;
        if (numChunksLoaded >= numChunks)
        {
            Debug.Log("Terrain Ready!");
            terrainReady.Invoke();
        }
    }

    void Start()
    {
        loadedChunks = new GameObject[loadDistance * 2 + 1, loadDistance * 2 + 1];
        for (var x = 0 - loadDistance; x <= 0 + loadDistance; x++)
        {
            for (var z = 0 - loadDistance; z <= 0 + loadDistance; z++)
            {
                loadedChunks[x + loadDistance, z + loadDistance] = Instantiate(terrain, new Vector3(x*513f, 0, z*513f), new Quaternion(0,0,0,0));
            }
        }
    }

    private void Update()
    {
        var lastXCell = xPlayerCell;
        var lastZCell = zPlayerCell;
        xPlayerCell = (int) MathF.Floor(transform.position.x / 512);
        zPlayerCell = (int) MathF.Floor(transform.position.z / 512);
        // Debug.Log("X cell: " + xPlayerCell + "Z cell: " + zPlayerCell);
        var deltaXCell = xPlayerCell - lastXCell;
        var deltaZCell = zPlayerCell - lastZCell;
        if (deltaXCell != 0 || deltaZCell != 0)
        {
            bool loadZ = deltaZCell != 0;
            if (loadZ)
                LoadRow(deltaZCell, true);
            else
                LoadRow(deltaXCell, false);
        }
    }
    
    private void LoadRow(int deltaCell, bool loadZ)
    {
        var length = loadDistance * 2 + 1;
        
        // Find last row
        int lastRowIndex;
        int firstRowIndex;
        if (deltaCell > 0)
        {
            lastRowIndex = 0;
            firstRowIndex = length - 1;
        }
        else
        {
            lastRowIndex = length - 1;
            firstRowIndex = 0;
        }

        // Delete last row
        for (int j = 0; j < length; j++)
        {
            if (loadZ)
            {
                Destroy(loadedChunks[j, lastRowIndex]);
            }
            else
                Destroy(loadedChunks[lastRowIndex, j]);
        }


        // Move rows

        if (deltaCell > 0)
        {
            for (int i = 0; i < length - 1; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (loadZ)
                    {
                        loadedChunks[j, i] = loadedChunks[j, (i + 1)];
                    }
                    else
                        loadedChunks[i, j] = loadedChunks[i + 1, j];

                }
            }
        }
        else
        {
            for (int i = length - 1; i > 0; i--)
            {
                for (int j = 0; j < length; j++)
                {
                    if (loadZ)
                    {
                        loadedChunks[j, i] = loadedChunks[j, i - 1];
                    }
                    else
                        loadedChunks[i, j] = loadedChunks[i - 1, j];
                }
            }
        }



        
        for (var i = -loadDistance; i <= loadDistance; i++)        
        {
            if (loadZ)
            {
                var xPos = (i + xPlayerCell) * 513f;
                var zPos = (zPlayerCell + loadDistance * deltaCell) * 513f;
                
                loadedChunks[i + loadDistance, firstRowIndex] = Instantiate(terrain, 
                    new Vector3(xPos, 0, zPos), 
                    new Quaternion(0,0,0,0)
                );
                
            }
            else
            {
                var xPos = (xPlayerCell + loadDistance * deltaCell) * 513f;
                var zPos = (i + zPlayerCell) * 513f;
                
                loadedChunks[firstRowIndex, i + loadDistance] = Instantiate(terrain, 
                    new Vector3(xPos, 0, zPos), 
                    new Quaternion(0,0,0,0)
                );
                
            }
        }
        
        // Add row
        /*for (var i = zPlayerCell - loadDistance; i <= zPlayerCell + loadDistance; i++)        
        {
            if (loadZ)
            {
                loadedChunks[length - 1, i - xPlayerCell + loadDistance] = Instantiate(terrain, 
                    new Vector3(i * 513f, 0, (xPlayerCell + loadDistance * deltaCell) * 513f), 
                    new Quaternion(0,0,0,0)
                );
            }
            else
            {
                loadedChunks[length - 1, i - zPlayerCell + loadDistance] = Instantiate(terrain, 
                    new Vector3((xPlayerCell + loadDistance * deltaCell) * 513f, 0, i * 513f), 
                    new Quaternion(0,0,0,0)
                );
            }
        }*/
    }
    // private void LoadCellsAround(int xCell, int zCell)
    // {
    //     for (var x = xCell - loadDistance; x <= xCell + loadDistance; x++)
    //     {
    //         for (var z = zCell - loadDistance; z <= zCell + loadDistance; z++)
    //         {
    //             Instantiate(terrain, new Vector3(x*513f, 0, z*513f), new Quaternion(0,0,0,0));
    //         }
    //     }
    // }

    // void GenerateTerrain(int x, int y)
    // {
    //     TerrainData terrainData = new TerrainData();
    //     // terrainData.size = new Vector3(512,512,512);
    //     terrainData.size = new Vector3(width, depth, length);
    //     terrainData.heightmapResolution = width;
    //     terrainData.SetHeights(0,0, GenerateHeights());
    //     Terrain.CreateTerrainGameObject(terrainData);
    // }
    //
    // float[,] GenerateHeights()
    // {
    //     float[,] heights = new float[width, length];
    //
    //     for (int x = 0; x < width; x++)
    //     {
    //         for (int y = 0; y < length; y++)
    //         {
    //             heights[x, y] = CalculateNoise(x,y,TerrainLoader.Instance.seed,macroScale);
    //         }
    //     }
    //
    //     return heights;
    // }
    //
    // float CalculateNoise(int x, int y, float seed, float scale)
    // {
    //     var position = transform.position;
    //     float xNorm = (float) x / width * scale + seed + position.x;
    //     float yNorm = (float) y / length * scale + seed + position.y;
    //     
    //     return Mathf.PerlinNoise(xNorm, yNorm);
    // }

    // Update is called once per frame
    // void Update()
    // {
    //     // Set position to the same position as the player
    //     var playerPosition = player.position;
    //     transform.position = new Vector3(playerPosition.x, 0, playerPosition.z);
    //     
    //     
    // }
}
