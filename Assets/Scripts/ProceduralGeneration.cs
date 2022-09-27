using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGeneration : MonoBehaviour
{
    public int width, height;
    private int blankCount;
    public float smoothness;
    [SerializeField]
    private float seed;
    public float modifier;
    public TileBase groundTile;
    public Tilemap[] groundTilemap;
    public TileBase grassTile;
    public Tilemap grassTilemap;
    public Tilemap treeTilemap;
    public Tilemap caveTilemap;
    public Tilemap waterTilemap;
    public Tilemap lavaTilemap;
    public TileBase caveTile;
    public TileBase waterTile;
    public TileBase lavaTile;
    public int[,] map;
    private int perlinHeight;
    private int noOfEnemies;
    public GameObject[] treesObjects;
    public TileBase[] gemsTiles;
    public GameObject enemy;
    public GameObject player;
    public GameObject tunaFish;
    public GameObject campFire;
    public GameObject endDoor;
    public GameObject shop;
    private int lowestPoint;
    private int lowestPointX;
    private int midY;

    // Start is called before the first frame update
    void Awake()
    {
        lowestPoint = 2000;
        Generation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Generation()
    {
        seed = Random.Range(-1000, 1000);
        map = GenerateArray(width, height, true);
        map = TerrainGeneration(map);
        if (blankCount <= 40000)
        {
            RenderMap(map, groundTilemap, grassTilemap, caveTilemap, treeTilemap, groundTile, grassTile, caveTile, treesObjects, gemsTiles, waterTile, waterTilemap);
        }
        else
        {
            blankCount = 0;
            Generation();
        }
    }

    //Generate initial array; all 0s.
    public int[,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = (empty) ? 0 : 1;
                blankCount++;
            }
        }
        return map;
    }

    //Generate all terain elements including chances of items spawning and refer to these as different numbers.
    public int[,] TerrainGeneration(int[,] map)
    {
        for (int x = 0; x < width; x++)
        {
            int treeChance = Random.Range(0, 10);
            perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seed) * height);
            for (int y = 0; y < perlinHeight; y++)
            {
                int gemChance = Random.Range(0, 100);
                int caveValue = Mathf.RoundToInt(Mathf.PerlinNoise((x * modifier) + seed, (y * modifier) + seed));

                map[x, y] = (y <= perlinHeight - 25 && y < perlinHeight - 4) ? (caveValue == 0) ? 5 : 1 : 1;

                if (!(y <= perlinHeight - 25 && y < perlinHeight - 4) && gemChance == 1)
                {
                    map[x, y] = 4;
                }

                if (y <= perlinHeight - 30 && y < perlinHeight - 3 && gemChance == 1 && caveValue == 1)
                {
                    map[x, y] = 4;
                }

                if (y >= perlinHeight - 3)
                {
                    map[x, y] = 2;
                }
                if (y == perlinHeight - 1 && treeChance == 1)
                {
                    map[x, y] = 3;
                }

                if (x == width / 2)
                {
                    midY = perlinHeight;
                }
                else
                {
                    blankCount--;
                }
                GetLowestPoint(x, perlinHeight);
            }
        }
        return map;
    }

    //For water spawn.
    void GetLowestPoint(int x, int y)
    {
        if (y < lowestPoint)
        {
            lowestPoint = y;
            lowestPointX = x;
        }
    }

    //Actually spawn tiles and object in correct position.
    public void RenderMap(int[,] map, Tilemap[] rockTileMap, Tilemap grassTileMap, Tilemap caveTileMap, Tilemap treeTileMap, TileBase rockTileBase, TileBase grassTileBase, TileBase caveTileBase, GameObject[] trees, TileBase[] gems, TileBase waterTileBase, Tilemap waterTileMap)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (y < 6) {
                    lavaTilemap.SetTile(new Vector3Int(x, y, 0), lavaTile);
                }
                if (map[x, y] == 5)
                {
                    int enemyChance = Random.Range(0, 2000);
                    caveTileMap.SetTile(new Vector3Int(x, y, 0), caveTileBase);
                    if (enemyChance == 1 && noOfEnemies <= 20)
                    {
                        Instantiate(enemy, new Vector3Int(x, y, 0), Quaternion.identity);
                        noOfEnemies += 1;
                    }
                }
                if (map[x, y] == 1)
                {
                    rockTileMap[0].SetTile(new Vector3Int(x, y, 0), rockTileBase);
                }
                if (map[x, y] == 2)
                {
                    grassTileMap.SetTile(new Vector3Int(x, y, 0), grassTileBase);
                }
                if (map[x, y] == 3)
                {
                    int tree = Random.Range(0, 3);
                    grassTileMap.SetTile(new Vector3Int(x, y, 0), grassTileBase);
                    Instantiate(trees[tree], new Vector3Int(x, y + 35, 0), Quaternion.identity);
                }
                if (map[x, y] == 4)
                {
                    int gem = Random.Range(0, 10);
                    if (gem >= 9) {
                        rockTileMap[1].SetTile(new Vector3Int(x, y, 0), gems[2]);
                    }
                    if (gem >= 6 && gem < 9)
                    {
                        rockTileMap[2].SetTile(new Vector3Int(x, y, 0), gems[1]);
                    }
                    if (gem >= 0 && gem < 6)
                    {
                        rockTileMap[3].SetTile(new Vector3Int(x, y, 0), gems[0]);
                    }
                }
            }
        }

        int waterHeight = Random.Range(12, 20);

        //Generate water and fish in water. 
        for (int y = lowestPoint; y < lowestPoint + waterHeight; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int fishSpawn = Random.Range(0, 100);
                if (map[x, y] == 0)
                {
                    waterTileMap.SetTile(new Vector3Int(x, y, 0), waterTileBase);
                    map[x, y] = 6;
                    if (fishSpawn == 1 && y < lowestPoint + waterHeight - 3)
                    {
                        int flip = Random.Range(0, 2);
                        if (flip == 1)
                        {
                            tunaFish.GetComponent<SpriteRenderer>().flipX = true;
                        }
                        else
                            tunaFish.GetComponent<SpriteRenderer>().flipX = false;

                        Instantiate(tunaFish, new Vector3Int(x, y, 0), Quaternion.identity);
                    }
                }
            }
        }

        int campFirePos = GenerateSpawnOnTopOfTerrainPos();

        //Spawn camp fire.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == campFirePos)
                {
                    if (map[x, y] == 6)
                    {
                        campFirePos = GenerateSpawnOnTopOfTerrainPos();
                        x = 0;
                        break;
                    }
                    if (y > lowestPoint + waterHeight && map[x, y] == 0)
                    {
                        Instantiate(campFire, new Vector3Int(x, y, 0), Quaternion.identity);
                        break;
                    }
                }
            }
        }

        int playerPos = GenerateSpawnOnTopOfTerrainPos();

        //Spawn player.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == playerPos)
                {
                    if (map[x, y] == 6)
                    {
                        playerPos = GenerateSpawnOnTopOfTerrainPos();
                        x = 0;
                        break;
                    }
                    if (y > lowestPoint + waterHeight && map[x, y] == 0)
                    {
                        player.transform.position = new Vector3Int(x, y + 50, 0);
                        break;
                    }
                }
            }
        }

        int shopPos = GenerateSpawnOnTopOfTerrainPos();

        //Spawn shop.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == shopPos)
                {
                    if (map[x, y] == 6)
                    {
                        shopPos = GenerateSpawnOnTopOfTerrainPos();
                        x = 0;
                        break;
                    }
                    if (y > lowestPoint + waterHeight && map[x, y] == 0)
                    {
                        Instantiate(shop, new Vector3Int(x, y, 0), Quaternion.identity);
                        break;
                    }
                }
            }
        }

        Vector3Int endDoorPos = GenerateDoorPos();

        //Spawn end door.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (endDoorPos.x == x && endDoorPos.y == y) 
                {
                    if (map[x, y] != 0 && y > 6)
                    {
                        Instantiate(endDoor, endDoorPos, Quaternion.identity);
                        break;
                    }
                    else
                    {
                        endDoorPos = GenerateDoorPos();
                        x = 0;
                        y = 0;
                        break;
                    }
                }
            }
        }
    }

    int GenerateSpawnOnTopOfTerrainPos()
    {
        int pos = Random.Range(0, width);
        return pos;
    }

    Vector3Int GenerateDoorPos()
    {
        Vector3Int endDoorPos = new Vector3Int(Random.Range(0, width), Random.Range(0, height), 0);
        return endDoorPos;
    }
}
