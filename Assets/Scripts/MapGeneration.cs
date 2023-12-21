using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour
{
    public BiomeManager[] biomes;    // Array of BiomeManager representing different biomes
    public Tilemap tilemap;           // Reference to your Tilemap component

    [Header("Dimensions")]
    public int width = 50;            // Width of the generated map
    public int height = 50;           // Height of the generated map
    public float scale = 1.0f;        // Scale of the generated map
    public Vector2 offset;            // Offset for noise generation (used in generate map function)

    [Header("Height Map")]
    public Wave[] heightWaves;        // Array of height waves
    public float[,] heightMap;        // 2D array representing the height map

    [Header("Moisture Map")]
    public Wave[] moistureWaves;      // Array of moisture waves
    private float[,] moistureMap;     // 2D array representing the moisture map

    [Header("Heat Map")]
    public Wave[] heatWaves;          // Array of heat waves
    private float[,] heatMap;         // 2D array representing the heat map

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();    // Prints the map in the game!
    }

    // Update is called once per frame
    void Update()
    {
        // Update Functions are for losers, everything is its own function or bust
    }

    // Function to generate the map (when it works, otherwise generates headaches)
    void GenerateMap()
    {
        // Generate height map
        heightMap = GenerateNoise.Generate(width, height, scale, heightWaves, offset);
        // Generate moisture map
        moistureMap = GenerateNoise.Generate(width, height, scale, moistureWaves, offset);
        // Generate heat map
        heatMap = GenerateNoise.Generate(width, height, scale, heatWaves, offset);

        // Loop through each point in the map and set a tile based on the biome
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                // Get the tile position using Vector3Int
                Vector3Int cellPosition = new Vector3Int(x, y, 0);

                // Get the biome at the current map position
                BiomeManager biome = GetBiome(heightMap[x, y], moistureMap[x, y], heatMap[x, y]);

                // Create a new Tile instance and set its sprite
                Tile tile = ScriptableObject.CreateInstance<Tile>();
                tile.sprite = biome.tiles[Random.Range(0, biome.tiles.Length)];

                // Set the tile at the calculated position
                tilemap.SetTile(cellPosition, tile);

              
            }
        }
    }

    // Function to determine the biome at a given map position
    BiomeManager GetBiome(float height, float moisture, float heat)
    {
        BiomeManager biomeToReturn = null; // Declare biomeToReturn here
        // If things are breaking, it's probably here
        List<BiomeTempData> biomeTemp = new List<BiomeTempData>();
        // Check each biome to see if it matches the conditions
        foreach (BiomeManager biome in biomes)
        {
            if (biome.MatchCondition(height, moisture, heat))
            {
                biomeTemp.Add(new BiomeTempData(biome));
            }
        }

        float curVal = 0.0f;
        // Determine the biome with the minimum difference value
        foreach (BiomeTempData biome in biomeTemp)
        { // More math
            if (biomeToReturn == null)
            {
                biomeToReturn = biome.biome;
                curVal = biome.GetDiffValue(height, moisture, heat);
            }
            else
            {
                if (biome.GetDiffValue(height, moisture, heat) < curVal) // I don't even know what a curVal is
                {
                    biomeToReturn = biome.biome;
                    curVal = biome.GetDiffValue(height, moisture, heat);
                }
            }
        }

        // If no matching biome is found, default to the first biome
        if (biomeToReturn == null)
            biomeToReturn = biomes[0];

        return biomeToReturn;
    }
}

// Serializable class to store temporary biome data
[System.Serializable]
public class BiomeTempData
{
    public BiomeManager biome;    // Summons the BiomeManager script stuff
    public BiomeTempData(BiomeManager preset)
    {
        biome = preset;
    }

    // Function that calculates the difference between the biome and biome attributes - I think?
    public float GetDiffValue(float height, float moisture, float heat) // I'm not sure what GetDiffValue is doing here, but it's doing it well
    {
        return (height - biome.minHeight) + (moisture - biome.minMoisture) + (heat - biome.minHeat);
    }
}