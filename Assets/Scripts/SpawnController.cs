using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnController : MonoBehaviour
{
    public GameObject eggPrefab; // Reference to the egg prefab
    public Tilemap tilemap; // Reference to your Tilemap containing the biomes

    void Start()
    {
        SpawnEggsOnBiomes();
    }

    void SpawnEggsOnBiomes()
    {
        BoundsInt bounds = tilemap.cellBounds;

        // Create a list to keep track of the tile types already processed
        // This ensures only one egg is spawned per unique tile type
        List<Sprite> processedTiles = new List<Sprite>();

        // Loop through each cell in the Tilemap
        foreach (Vector3Int cellPosition in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(cellPosition);

            // Check if the tile is not null (i.e., there is a tile at this position)
            if (tile != null && tile is Tile)  // Ensure the tile is of type Tile
            {
                Sprite tileSprite = ((Tile)tile).sprite;

                // Check if this tile type has already been processed
                if (!processedTiles.Contains(tileSprite))
                {
                    // Spawn an egg at the center of the tile
                    Vector3 tileCenter = tilemap.GetCellCenterWorld(cellPosition);
                    Instantiate(eggPrefab, tileCenter, Quaternion.identity);

                    // Mark this tile type as processed
                    processedTiles.Add(tileSprite);
                }
            }
        }
    }

}