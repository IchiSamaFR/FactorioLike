using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public static Items instance;
    public List<Item> items = new List<Item>();


    void Awake()
    {
        instance = this;
    }

    public Item GetOreByTile(string id)
    {
        string idToGet = "";
        if (id == "iron_ore")
        {
            idToGet = "";
        } 
        else if (id == "tile_copper")
        {
            idToGet = "copper_ore";
        }

        foreach(Item ore in items)
        {
            if(ore.id == idToGet)
            {
                return ore;
            }
        }
        return null;
    }
}


[System.Serializable]
public class Item
{
    public string id;
    public string type;
    public GameObject prefab;
}
