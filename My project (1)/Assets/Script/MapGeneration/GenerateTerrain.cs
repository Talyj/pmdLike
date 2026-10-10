using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
    [SerializeField] private int mapWidthInTiles, mapDepthInTiles;
    [SerializeField] private GameObject tilePrefab;
    private List<GameObject> tiles;

    private int tileWidth, tileDepth;
    void Start()
    {
        // get the tile dimensions from the tile Prefab
        Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
        tileWidth = (int)tileSize.x;
        tileDepth = (int)tileSize.z;

        GenerateMap();
        GenerateWalkingTerrain(tiles, tileDepth, tileWidth);
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
                    float height = tile.GetComponent<TileGeneration>().heightMap[zIndex, xIndex];

                }
            }
        }
    }
}          

