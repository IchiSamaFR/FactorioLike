using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBuilding : Building
{
    public string buildType;
    public GameObject[] itemsToTransform = new GameObject[0];


    public void Craft()
    {
        foreach (GameObject item in itemsToTransform)
        {

        }
    }

    public new bool GetItem(GameObject newItem, int pos)
    {
        if (itemsToTransform[pos] == null)
        {
            GameObject oreInstantiate = Instantiate(newItem, this.transform);
            oreInstantiate.transform.position = newItem.transform.position;
            itemsToTransform[pos] = oreInstantiate;

            if (chunk.active)
            {
                oreInstantiate.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                oreInstantiate.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public new bool GetItem(GameObject newItem)
    {
        if (itemsToTransform[itemsStockedMax - 1] == null)
        {
            GameObject oreInstantiate = Instantiate(newItem, this.transform);
            itemsToTransform[itemsStockedMax - 1] = oreInstantiate;

            if (direction == 0)
            {
                oreInstantiate.transform.position = this.transform.position + new Vector3(0.5f, 0.3f, 0);
            }
            else if (direction == 1)
            {
                oreInstantiate.transform.position = this.transform.position + new Vector3(0, 0.3f, 0.5f);
            }
            else if (direction == 2)
            {
                oreInstantiate.transform.position = this.transform.position + new Vector3(0.5f, 0.3f, 1);
            }
            else if (direction == 3)
            {
                oreInstantiate.transform.position = this.transform.position + new Vector3(1, 0.3f, 0.5f);
            }

            if (chunk.active)
            {
                oreInstantiate.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                oreInstantiate.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
