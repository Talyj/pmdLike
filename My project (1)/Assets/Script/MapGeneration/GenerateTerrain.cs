using Assets.Script.Actions;
using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenerateTerrain : MonoBehaviour
{   
    //Maybe separate the spawn of the ground and the player/enemies
    [SerializeField] private int mapWidthInTiles, mapDepthInTiles;
    [SerializeField] private GameObject tilePrefab;
    private List<GameObject> tiles = new List<GameObject>();

    public List<Ground> grounds = new List<Ground>();
    //public int numberOfEnemy=0;
    private int tileWidth, tileDepth;

    private void OnEnable()
    {
        // get the tile dimensions from the tile Prefab
        Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
        tileWidth = (int)tileSize.x;
        tileDepth = (int)tileSize.z;

        if (SceneManager.GetActiveScene().name.ToLower() != "firstdungeon")
        {
            var rdmChoice1 = UnityEngine.Random.Range(3, 11);
            var rdmChoice2 = UnityEngine.Random.Range(3, 11);

            mapWidthInTiles = rdmChoice1;
            mapDepthInTiles = rdmChoice2;
        }
        GenerateMap();
    }

    void Start()
    {
        //// get the tile dimensions from the tile Prefab
        //Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
        //tileWidth = (int)tileSize.x;
        //tileDepth = (int)tileSize.z;

        //if(SceneManager.GetActiveScene().name.ToLower() != "firstdungeon")
        //{
        //    var rdmChoice1 = UnityEngine.Random.Range(3, 11);
        //    var rdmChoice2 = UnityEngine.Random.Range(3, 11);

        //    mapWidthInTiles = rdmChoice1;
        //    mapDepthInTiles = rdmChoice2;
        //}
        //GenerateMap();
    }

    private void Update()
    {
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
                //Maybe add a different map generation ? (less square shaped and more like a random dungeon)


                // instantiate a new Tile
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
                tiles.Add(tile);
            }
        }        
    }
}          

