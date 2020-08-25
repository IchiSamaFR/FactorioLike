using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smeltery : Building
{
    SmeltRecipes smeltRecipes;
    Items items;
    public GameObject[] itemsToTransform = new GameObject[0];
    float timer;

    void Start()
    {
        smeltRecipes = SmeltRecipes.instance;
        items = Items.instance;

        itemsToTransform = new GameObject[itemsStockedMax];
    }

    void Update()
    {
        ChangeItemsPos();
        Smelt();
        Eject();
    }

    void Smelt()
    {
        if (speed > 0)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if (itemsToEject[itemsStockedMax - 1] == null && itemsToTransform[0] != null)
            {
                Item itemRessource = itemsToTransform[0].GetComponent<Item>();
                SmeltRecipe recipe;

                if((recipe = smeltRecipes.GetResult(itemRessource.id)) != null)
                {
                    itemsToEject[itemsStockedMax - 1] = Instantiate(items.GetItem(recipe.resultId), this.transform);
                    GameObject toDelete;
                    toDelete = itemsToTransform[0];

                    itemsToTransform[0] = null;
                    Destroy(toDelete);
                    timer = speed;
                }
            }
        }
    }

    void ChangeItemsPos()
    {
        int pos = 0;
        foreach (GameObject item in itemsToEject)
        {
            if (item != null)
            {
                if (pos != 0 && itemsToEject[pos - 1] == null)
                {
                    itemsToEject[pos - 1] = itemsToEject[pos];
                    itemsToEject[pos] = null;
                }

                float corrector = 0;
                if (pos % 2 == 0)
                {
                    corrector = 0.3f;
                }

                Vector3 posToGet = new Vector3();
                if (direction == 0)
                {
                    posToGet = this.transform.position + new Vector3(0.35f + corrector, (int)(pos / 2) * 0.2f + 0.1f, 0.8f);
                }
                else if (direction == 1)
                {
                    posToGet = this.transform.position + new Vector3(0.8f, (int)(pos / 2) * 0.2f + 0.1f, 0.35f + corrector);
                }
                else if (direction == 2)
                {
                    posToGet = this.transform.position + new Vector3(0.65f - corrector, (int)(pos / 2) * 0.2f + 0.1f, 0.2f);
                }
                else if (direction == 3)
                {
                    posToGet = this.transform.position + new Vector3(0.2f, (int)(pos / 2) * 0.2f + 0.1f, 0.65f - corrector);
                }

                if (item.transform.position != posToGet)
                {
                    item.transform.position = posToGet;
                }
            }
            pos++;
        }

        pos = 0;
        foreach (GameObject item in itemsToTransform)
        {
            if(pos != 0 && item != null)
            {
                if(itemsToTransform[pos - 1] == null)
                {
                    itemsToTransform[pos - 1] = itemsToTransform[pos];
                    itemsToTransform[pos] = null;
                }
            }
            pos++;
        }

    }


    public new bool GetItem(GameObject newItem, int pos)
    {
        if (itemsToTransform[pos] == null)
        {
            GameObject itemInstatiate = Instantiate(newItem, this.transform);
            itemsToTransform[pos] = itemInstatiate;


            foreach (Transform obj in itemInstatiate.transform)
            {
                obj.GetComponent<MeshRenderer>().enabled = false;
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
            GameObject itemInstatiate = Instantiate(newItem, this.transform);
            itemsToTransform[itemsStockedMax - 1] = itemInstatiate;

            foreach (Transform obj in itemInstatiate.transform)
            {
                obj.GetComponent<MeshRenderer>().enabled = false;
            }

            return true;
        }
        else
        {
            return false;
        }
    }
}
