using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnController : MonoBehaviour
{
    public GameObject eggPrefab; // Reference to the egg prefab - make sure they appear in inspector 
    public Tilemap tilemap; // Reference to the Tilemap containing the biomes - make sure they appear in inspector 



    public GameObject[] badDinosaurPrefabs; // Reference to the enemy AI prefab - make sure they appear in inspector 
    private int maxDinosaurs = 7;
    private List<GameObject> badDinosaurs = new List<GameObject>();


    void Start()
    {
        SpawnEggsOnBiomes();
        SpawnBadDinosaursRandomly();
    }


    void Update()
    {
        // Check if any dinosaurs have been destroyed and respawn them
        for (int i = badDinosaurs.Count - 1; i >= 0; i--)
        {
            if (badDinosaurs[i] == null)
            {
                badDinosaurs.RemoveAt(i);
                SpawnBadDinosaur();
            }
        }
    }

    void SpawnEggsOnBiomes()
    {
        BoundsInt bounds = tilemap.cellBounds; // Defining the boundary as cells on the tile map

        // Create a dictionary to keep track of the tiles for each biome
        Dictionary<Sprite, List<Vector3Int>> biomeTiles = new Dictionary<Sprite, List<Vector3Int>>();

        // Loop through each cell in the Tilemap
        foreach (Vector3Int cellPosition in bounds.allPositionsWithin) //allPositionWithin is just the entire boundary
        {
            TileBase tile = tilemap.GetTile(cellPosition);

            // Check if the tile is not null (i.e., there is a tile at this position)
            if (tile != null && tile is Tile)  // Make sure that the tile is in fact a Tile, if this isnt here weird shit occurs 
            {
                Sprite tileSprite = ((Tile)tile).sprite;

                // If the biome is not in the dictionary, add it
                if (!biomeTiles.ContainsKey(tileSprite))
                {
                    biomeTiles[tileSprite] = new List<Vector3Int>();
                }

                // Add the cell position to the list for this biome
                biomeTiles[tileSprite].Add(cellPosition);
            }
        }

        // Loop through each biome and spawn one egg randomly
        foreach (var kvp in biomeTiles) // Iterate through each key-value pair in the biomeTiles dictionary
        {
            List<Vector3Int> tilesInBiome = kvp.Value; // Retrieve the list of Vector3Int tiles associated with the current biome
            Biomes tempBiome = Biomes.None; // Initialize a temporary variable to store the current biome (default to Biomes.None)

            foreach (Biomes biome in Enum.GetValues(typeof(Biomes))) // Cycle through each value in the Biomes enum
            {
                if (kvp.Key.ToString().Contains(biome.ToString())) // Check if the returns the same biome name. Biome data here is saved as a SPRITE

                {
                    tempBiome = biome; // If true, set tempBiome to the current biome
                }
            }

            // Ensure there are tiles in the biome before spawning an egg
            if (tilesInBiome.Count > 0)
            {
                // Randomly select a tile from the biome
                Vector3Int randomTilePosition = tilesInBiome[UnityEngine.Random.Range(0, tilesInBiome.Count)];

                // Spawn an egg at the center of the randomly selected tile
                Vector3 tileCenter = tilemap.GetCellCenterWorld(randomTilePosition);


                GameObject egg = Instantiate(eggPrefab, tileCenter, Quaternion.identity);

                egg.GetComponent<EggController>().SetBiome(tempBiome);
            }
        }

        foreach (var kvp in biomeTiles)
        {
            List<Vector3Int> tilesInBiome = kvp.Value;
            Biomes tempBiome = Biomes.None;

            string spriteName = kvp.Key.name;

            Debug.Log("Sprite Name: " + spriteName);

            if (spriteName == "DeepWater")
                tempBiome = Biomes.DeepWater;
            else if (spriteName == "Plains")
                tempBiome = Biomes.Plains;
            else if (spriteName == "FreshWater")
                tempBiome = Biomes.FreshWater;
            else if (spriteName == "Dirt")
                tempBiome = Biomes.Dirt;
            else if (spriteName == "Forest")
                tempBiome = Biomes.Forest;
            else if (spriteName == "Snow")
                tempBiome = Biomes.Snow;
            else if (spriteName == "Sand")
                tempBiome = Biomes.Sand;

            Debug.Log("Temp Biome: " + tempBiome);

            // Ensure there are tiles in the biome before spawning an egg
            if (tilesInBiome.Count > 0)
            {
                // Randomly select a tile from the biome
                Vector3Int randomTilePosition = tilesInBiome[UnityEngine.Random.Range(0, tilesInBiome.Count)];

                // Spawn an egg at the center of the randomly selected tile
                Vector3 tileCenter = tilemap.GetCellCenterWorld(randomTilePosition);

                GameObject egg = Instantiate(eggPrefab, tileCenter, Quaternion.identity);

                egg.GetComponent<EggController>().SetBiome(tempBiome);
            }
        }
    }





    void SpawnBadDinosaursRandomly()
    {
        for (int i = 0; i < maxDinosaurs; i++)
        {
            SpawnBadDinosaur();
        }
    }

    void SpawnBadDinosaur()
    {
        Vector3 randomPosition = GetRandomSpawnPosition();
        GameObject badDinosaurPrefab = badDinosaurPrefabs[UnityEngine.Random.Range(0, badDinosaurPrefabs.Length)];

        GameObject badDinosaur = Instantiate(badDinosaurPrefab, randomPosition, Quaternion.identity);
        badDinosaurs.Add(badDinosaur);
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPosition;

        do
        {
            randomPosition = new Vector3(
                UnityEngine.Random.Range(tilemap.cellBounds.x, tilemap.cellBounds.x + tilemap.cellBounds.size.x),
                UnityEngine.Random.Range(tilemap.cellBounds.y, tilemap.cellBounds.y + tilemap.cellBounds.size.y),
                0);
        }
        
        while (!IsTileBelow(randomPosition));


        return tilemap.GetCellCenterWorld(tilemap.WorldToCell(randomPosition));
    }

    bool IsTileBelow(Vector3 position)
    {
        // Raycast downward to check if there is a tile below the specified position
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, 1f, LayerMask.GetMask("Default"));

        return hit.collider != null;
    }

    bool IsPositionOccupied(Vector3 position)
    {
        foreach (GameObject dinosaur in badDinosaurs)
        {
            if (Vector3.Distance(dinosaur.transform.position, position) < 2f)  // I can define how far apart they need to spawn from each other here
            {
                return true;
            }
        }
        return false;
    }

}


