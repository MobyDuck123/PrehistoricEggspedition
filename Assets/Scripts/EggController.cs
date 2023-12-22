using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EggController : MonoBehaviour
{
    private Biomes eggBiome;
    public GameObject[] dinosaurPrefabs; // Array of dinosaur prefabs for each biome
    public GameObject dinosaurBiome;

    private void GetDinosaurBiome()
    {
        for (int i = 0; i < dinosaurPrefabs.Length; i++)
        {
            Debug.Log("This dinosaur is" + dinosaurPrefabs[i].name);
            Biomes tempDinoBiome = dinosaurPrefabs[i].GetComponent<DinosaurBiome>().GetDinoBiome();
            Debug.Log("Temporary Dino biome is" + tempDinoBiome);
           
            if (tempDinoBiome == eggBiome)
            {
                dinosaurBiome = dinosaurPrefabs[i];
                Debug.Log("DinosaurBiome is" + dinosaurPrefabs[i]);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if (dinosaurBiome != null)
            {
                Instantiate(dinosaurBiome, transform.position, Quaternion.identity);

                Debug.Log("Egg is in biome: " + eggBiome);

                // Destroy the egg
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Dinosaur biome not set");
            }
        }
    }

    public void SetBiome(Biomes biome)
    {
        eggBiome = biome;
        Debug.Log("My biome is " + eggBiome);
        GetDinosaurBiome();
    }
}

//// private void OnTriggerEnter2D(Collider2D collision)
//{
//    if (collision.gameObject.CompareTag("Player"))
//    {
//        // Check the biome of the egg using the information set in SetBiome
//        int biomeIndex = (int)eggBiome;

//        if (biomeIndex >= 0 && biomeIndex < dinosaurPrefabs.Length)
//        {
//            // Log the biome information for debugging
//            Debug.Log("Egg is in biome: " + eggBiome);

//            // Instantiate the corresponding dinosaur prefab at the egg's position
//            Instantiate(dinosaurPrefabs[biomeIndex], transform.position, Quaternion.identity);

//            // Destroy the egg
//            Destroy(gameObject);
//        }
//        else
//        {
//            Debug.LogError("Invalid biome index");
//        }
//    }
//}