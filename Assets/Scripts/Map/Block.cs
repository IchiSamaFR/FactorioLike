using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int posX;
    public int posZ;
    public string Type;
    public Chunk chunk;

    public void Set(int posX, int posZ, string Type, Chunk chunk)
    {
        this.posX = posX;
        this.posZ = posZ;
        this.Type = Type;
        this.chunk = chunk;
    }
}
