using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    Chunk chunk;
    public GameObject[] items = new GameObject[0];
    public Item item;

    public float speed = 2;
    public int oresStockedMax = 4;
    public int posX;
    public int posZ;


    public sbyte direction = 0;
    sbyte directionCanceled = 0;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
