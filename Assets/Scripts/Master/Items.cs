using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public static Items instance;
    public List<GameObject> items = new List<GameObject>();


    void Awake()
    {
        instance = this;
    }

    public GameObject GetOreByTile(string id)
    {
        string idToGet = "";
        if (id == "iron_tile")
        {
            idToGet = "iron_ore";
        } 
        else if (id == "copper_tile")
        {
            idToGet = "copper_ore";
        }

        foreach(GameObject ore in items)
        {
            if (!ore.GetComponent<Item>())
            {
                Debug.Log("An item don't have class.");
                return null;
            }
            else if(ore.GetComponent<Item>().id == idToGet)
            {
                return ore;
            }
        }
        return null;
    }

    public GameObject GetItem(string id)
    {
        foreach (GameObject item in items)
        {
            if (!item.GetComponent<Item>())
            {
                Debug.Log("An item don't have class.");
                return null;
            }
            else if (item.GetComponent<Item>().id == id)
            {
                return item;
            }
        }
        return null;
    }
}

