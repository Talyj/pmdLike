using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
    public float heightSpawn = 3.5f;
    [SerializeField] private int mapWidthInTiles, mapDepthInTiles;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject npc;
    [SerializeField] Camera camera;
    [SerializeField] TurnManagement turnManager;
    private List<GameObject> tiles = new List<GameObject>();

    public List<Ground> grounds = new List<Ground>();
    public int numberOfEnemy=0;


    private int tileWidth, tileDepth;
    void Start()
    {
        //grounds = new List<Ground>();
        // get the tile dimensions from the tile Prefab
        Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
        tileWidth = (int)tileSize.x;
        tileDepth = (int)tileSize.z;

        GenerateMap();
        //GenerateWalkingTerrain(tiles, tileDepth, tileWidth);
        SpawnPlayer();
        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if(grounds.Count() > 0 && numberOfEnemy < 10)
            {
                var test = UnityEngine.Random.Range(0, grounds.Count());

                GameObject enemy = Instantiate(npc, new Vector3(grounds[test].transform.position.x, heightSpawn, grounds[test].transform.position.z), Quaternion.identity);
                enemy.GetComponent<NpcMovement>().isCompanion = false;
                turnManager.enemies.Add(enemy);
                numberOfEnemy++;
            }
            Debug.Log("oue");
            yield return new WaitForSeconds(2);
        }
    }

    void SpawnPlayer()
    {
        //Player
        GameObject play = Instantiate(player, new Vector3(0, 3.5f, 0), Quaternion.identity);
        turnManager.player = play;
        camera.GetComponent<PlayerFollow>().PlayerTransform = play.GetComponent<PlayerController>();

        //Companions
        GameObject companions = Instantiate(npc, new Vector3(1, heightSpawn, 0), Quaternion.identity);
        turnManager.companions.Add(companions);
    }

    void GenerateMap()
    {
        for(int xTileIndex = 0; xTileIndex < mapWidthInTiles; xTileIndex++)
        {
            for(int zTileIndex = 0; zTileIndex < mapDepthInTiles; zTileIndex++)
            {
                Vector3 tilePosition = new Vector3(this.gameObject.transform.position.x + xTileIndex * tileWidth,
                this.gameObject.transform.position.y,
                this.gameObject.transform.position.z + zTileIndex * tileDepth);
                // instantiate a new Tile
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
                tiles.Add(tile);
            }
        }        
    }

    void GenerateWalkingTerrain(List<GameObject> tiles, int tileDepth, int tileWidth)
    {
        foreach (var tile in tiles)
        {
            /*GenerateWalkingTerrain(tile.GetComponent<TileGeneration>(), tileDepth, tileWidth)*/;
            for (int zIndex = 0; zIndex < tileDepth; zIndex++)
            {
                for (int xIndex = 0; xIndex < tileWidth; xIndex++)
                {
                    int colorIndex = zIndex * tileWidth + xIndex;
                    //float height = tile.GetComponent<TileGeneration>().heightMap[zIndex, xIndex];

                }
            }
        }
    }
}          

