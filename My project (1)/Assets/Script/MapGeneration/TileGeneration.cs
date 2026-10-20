using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class TerrainType
{
    public string name;
    public float height;
    public Color color;

}

public class TileGeneration : MonoBehaviour
{
    [SerializeField] private Ground sol;

    [SerializeField] private TerrainType[] terrainTypes;
    [SerializeField] public float[,] heightMap;

    [SerializeField] NoiseMapGeneration noiseMapGeneration;
    [SerializeField] private MeshRenderer tileRenderer;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshCollider meshCollider;
    [SerializeField] float mapScale;

    private GenerateTerrain obj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        obj = FindAnyObjectByType<GenerateTerrain>();
        GenerateMap();
    }

    void GenerateMap()
    {
        Vector3[] meshVertices = this.meshFilter.mesh.vertices;
        int tileDepth = (int)Mathf.Sqrt(meshVertices.Length);
        int tileWidth = tileDepth;

        float offsetX = -this.gameObject.transform.position.x;
        float offsetZ = -this.gameObject.transform.position.z;

        heightMap = this.noiseMapGeneration.GenerateNoiseMap(tileDepth, tileWidth, this.mapScale, offsetX, offsetZ);

        Texture2D tileTexture = BuildTexture(heightMap, offsetX, offsetZ);
        this.tileRenderer.material.mainTexture = tileTexture;

    }

    private Texture2D BuildTexture(float[,] heightMap, float offsetX, float offsetZ)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);
        Color[] colorMap = new Color[tileDepth * tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                //can remove that once the ground generation works
                int colorIndex = zIndex * tileWidth + xIndex;
                float height = heightMap[zIndex, xIndex];

                //Get the terrain
                TerrainType terrainType = ChooseTerrainType(height);
                colorMap[colorIndex] = terrainType.color;

                GenerateTiles(terrainType, xIndex, zIndex);
            }
        }
        Texture2D tileTexture = new Texture2D(tileWidth, tileDepth);
        tileTexture.wrapMode = TextureWrapMode.Clamp;
        tileTexture.SetPixels(colorMap);
        tileTexture.Apply();
        return tileTexture;
    }

    private void GenerateTiles(TerrainType terrainType, int xIndex, int zIndex)
    {
        if (terrainType.name == "ground")
        {
            //Debug.Log(xIndex + " " + zIndex);
            var newVEctor = new Vector3((this.transform.position.x + xIndex * -1) + 5 , 0, (this.transform.position.z + zIndex * -1) + 5);
            Ground ground = Instantiate(sol, newVEctor, Quaternion.identity);
            obj.grounds.Add(ground);
            //ajouter une seconde
        }       
    }

    TerrainType ChooseTerrainType(float height)
    {
        // for each terrain type, check if the height is lower than the one for the terrain type
        foreach (TerrainType terrainType in terrainTypes)
        {
            // return the first terrain type whose height is higher than the generated one
            if (height < terrainType.height)
            {
                return terrainType;
            }
        }
        return terrainTypes[terrainTypes.Length - 1];
    }
}
