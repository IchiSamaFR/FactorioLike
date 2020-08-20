using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ores : MonoBehaviour
{
    public static Ores instance;
    public List<Ore> ores = new List<Ore>();


    void Awake()
    {
        instance = this;
    }

    public Ore GetOre(int id)
    {
        foreach(Ore ore in ores)
        {
            if(ore.id == id)
            {
                return ore;
            }
        }

        return null;
    }




}


[System.Serializable]
public class Ore
{
    public int id;
    public string name;
    public GameObject prefab;
}
