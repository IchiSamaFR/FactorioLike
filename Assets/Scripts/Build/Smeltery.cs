using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smeltery : Building
{
    SmeltRecipes smeltRecipes;
    Items items;
    public GameObject[] itemsToTransform = new GameObject[0];
    float timer;


    public override void _init()
    {
        smeltRecipes = SmeltRecipes.instance;
        items = Items.instance;

        itemsToTransform = new GameObject[itemsStockedMax];
        timer = speed;
    }

    void Update()
    {
        ChangeItemsPos();
        Smelt();
        Eject();
    }

    void Smelt()
    {
        if (speed > 0 && itemsToTransform[0] != null)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if (itemsToEject[itemsStockedMax - 1] == null)
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
        int nullBef = 0;
        foreach (GameObject item in itemsToEject)
        {
            if (item != null)
            {
                if (nullBef > 0)
                {
                    itemsToEject[pos - nullBef] = itemsToEject[pos];
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
            else
            {
                nullBef++;
            }
            pos++;
        }

        pos = 0;
        nullBef = 0;
        foreach (GameObject item in itemsToTransform)
        {
            if(item != null)
            {
                if (nullBef > 0)
                {
                    itemsToTransform[pos - nullBef] = itemsToTransform[pos];
                    itemsToTransform[pos] = null;
                }
            }
            else
            {
                nullBef++;
            }
            pos++;
        }

    }


    public override bool GetItem(GameObject newItem, int pos)
    {
        if (itemsToTransform[pos] == null && smeltRecipes.GetResult(newItem.GetComponent<Item>().id) != null)
        {
            GameObject itemInstatiate = Instantiate(newItem, this.transform);
            itemsToTransform[pos] = itemInstatiate;


            
            itemInstatiate.SetActive(false);
            return true;
        }
        else if (smeltRecipes.GetResult(newItem.GetComponent<Item>().id) == null)
        {
            int posToEject = 0;
            foreach (GameObject obj in itemsToEject)
            {
                if (obj == null)
                {
                    GameObject itemInstatiate = Instantiate(newItem, this.transform);
                    itemsToEject[posToEject] = itemInstatiate;

                    return true;
                }
                else
                {
                    posToEject++;
                }
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    public override bool GetItem(GameObject newItem)
    {
        if (itemsToTransform[itemsStockedMax - 1] == null && smeltRecipes.GetResult(newItem.GetComponent<Item>().id) != null)
        {
            GameObject itemInstatiate = Instantiate(newItem, this.transform);
            itemsToTransform[itemsStockedMax - 1] = itemInstatiate;

            itemInstatiate.SetActive(false);
            return true;
        }
        else if (smeltRecipes.GetResult(newItem.GetComponent<Item>().id) == null)
        {
            int posToEject = 0;
            foreach (GameObject obj in itemsToEject)
            {
                if (obj == null)
                {
                    GameObject itemInstatiate = Instantiate(newItem, this.transform);
                    itemsToEject[posToEject] = itemInstatiate;

                    return true;
                }
                else
                {
                    posToEject++;
                }
            }
            return false;
        }
        else
        {
            return false;
        }
    }
}
