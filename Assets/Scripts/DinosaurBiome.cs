using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinosaurBiome : MonoBehaviour
{
    [SerializeField]
    private Biomes dinoBiome;
   

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Biomes GetDinoBiome()
    {
        return dinoBiome;
    }
}
