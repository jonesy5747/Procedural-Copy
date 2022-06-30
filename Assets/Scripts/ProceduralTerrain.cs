using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralTerrain : MonoBehaviour
{
    public int width, /*height,*/ startPos;
    public int minRockHeight, maxRockHeight;
    public int maxTerrainHeight;
    public int rubyChancePercent;
    public int coalChancePercent;
    public int diamondChancePercent;

    //public GameObject dirt, grass, rock;
    //public GameObject[] gems;
    public GameObject[] trees;
    public GameObject[] rocks;

    public Tilemap dirtTilemap, grassTilemap, rockTilemap;
    public Tile dirt, grass, rock, coal, ruby, diamond;
    public RuleTile ruleGrass, ruleRock;

    [Range(0,100)]
    public float heightValue, smoothness;

    private float seed;

    // Start is called before the first frame update
    void Start()
    {
        seed = Random.Range(-1000000, 1000000);
        Generation();
        GameObject.FindGameObjectWithTag("manager").GetComponent<Manager>().terrains.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Generation()
    {
        for (int x = startPos; x < width + startPos; x++)
        {
            int height = Mathf.RoundToInt(heightValue * Mathf.PerlinNoise(x / smoothness, seed));

            int cliffChance = Random.Range(0, 100);

            if (height == maxTerrainHeight)
            {
                height -= 1;
            }
            if (height == 0)
            {
                height += 1;
            }

            if (cliffChance == 1)
            {
                int cliffChanceDirection = Random.Range(0, 1);
                if (cliffChanceDirection == 0)
                {
                    if (height + 20 <= maxTerrainHeight)
                    {
                        height += 20;
                    }
                }
                if (cliffChanceDirection == 1)
                {
                    if (height - 20 > 0)
                    {
                        height -= 20;
                    }
                }
            }

            int minRockDistance = height - minRockHeight;
            int maxRockDistance = height - maxRockHeight;
            int totalRockDistance = Random.Range(minRockDistance, maxRockDistance);

            for (int y = 0; y < height; y++)
            {
                int rubyChance = Random.Range(0, rubyChancePercent);
                int coalChance = Random.Range(0, coalChancePercent);
                int diamondChance = Random.Range(0, diamondChancePercent);

                rockTilemap.SetTile(new Vector3Int(x, y, 0), ruleRock);

                if (y < totalRockDistance)
                {
                    /*if (rubyChance == 1)
                    {
                        //spawnObject(gems[Random.Range(0, gems.Length)], x, y);
                        rockTilemap.SetTile(new Vector3Int(x, y, 0), ruby);
                        continue;
                    }
                    if (coalChance == 1)
                    {
                        //spawnObject(gems[Random.Range(0, gems.Length)], x, y);
                        rockTilemap.SetTile(new Vector3Int(x, y, 0), coal);
                        continue;
                    }
                    if (diamondChance == 1)
                    {
                        //spawnObject(gems[Random.Range(0, gems.Length)], x, y);
                        rockTilemap.SetTile(new Vector3Int(x, y, 0), diamond);
                        continue;
                    }
                    //spawnObject(rock, x, y);*/
                    
                }

                //else
                    //spawnObject(dirt, x, y);
                    //dirtTilemap.SetTile(new Vector3Int(x, y, 0), ruleGrass);
            }

            //spawnObject(grass, x, height);
            dirtTilemap.SetTile(new Vector3Int(x, height, 0), ruleGrass);

            int treeChance = Random.Range(0, 10);

            if (treeChance == 1)
            {
                spawnObject(trees[Random.Range(0, trees.Length)], x, height);
            }

            int rockChance = Random.Range(0, 50);

            if (rockChance == 1)
            {
                spawnObject(rocks[Random.Range(0, rocks.Length)], x, height);
            }

            //GameObject.FindWithTag("nextTile").GetComponent<NextTile>().height = height;
        }
    }

    void spawnObject(GameObject obj, int width, int height) {
        obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);
        obj.transform.parent = this.transform;
    }
}
