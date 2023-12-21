using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


// Create a scriptable object for managing biome presets
[CreateAssetMenu(fileName = "Biome Preset", menuName = "New Biome Preset")]
public class BiomeManager : ScriptableObject
{
    // Array of sprites representing different tiles in the biome
    public Sprite[] tiles;

    // Minimum height, moisture, and heat values for the biome to match a condition
    public float minHeight;
    public float minMoisture;
    public float minHeat;

    // Method to get a random tile sprite from the tiles array
    public Sprite GetTileSprite()
    {
        // Return a random sprite from the tiles array
        return tiles[Random.Range(0, tiles.Length)];
    }

    // Method to check if the given height, moisture, and heat values match the biome condition
    public bool MatchCondition(float height, float moisture, float heat)
    {
        // Return true if the given values meet the specified minimum conditions
        return height >= minHeight && moisture >= minMoisture && heat >= minHeat;
    }
}