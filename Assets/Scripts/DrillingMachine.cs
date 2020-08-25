using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrillingMachine : Building
{
    public GameObject itemProduce;

    float timer;


    public void AddItemToProduce(string oreId)
    {
        itemProduce = Items.instance.GetOreByTile(oreId);
        if(itemProduce == null)
        {
            Debug.Log("L'id du bloc n'est pas reconnu");
            speed = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = speed;
    }
    void Update()
    {
        Generate();
        Eject();
        ChangeItemsPos();
    }

    void Generate()
    {
        if(speed > 0)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if (itemsToEject[itemsStockedMax -1] == null)
            {
                AddOre();
                timer = speed;
            }
        }
    }

    void AddOre()
    {
        GameObject obj = Instantiate(itemProduce, this.transform);
        itemsToEject[itemsStockedMax - 1] = obj;
        ChangeItemsPos();
    }

    void ChangeItemsPos()
    {
        int pos = 0;
        foreach(GameObject item in itemsToEject)
        {
            if(item != null)
            {
                if (pos != 0 && itemsToEject[pos - 1] == null)
                {
                    itemsToEject[pos - 1] = itemsToEject[pos];
                    itemsToEject[pos] = null;
                }

                float corrector = 0;
                if (pos % 2 == 0)
                {
                    corrector = 0.2f;
                }

                Vector3 posToGet = new Vector3();
                if (direction == 0)
                {
                    posToGet = this.transform.position + new Vector3(0.4f + corrector, (int)(pos / 2) * 0.2f, 0.9f);
                } 
                else if (direction == 1)
                {
                    posToGet = this.transform.position + new Vector3(0.9f, (int)(pos / 2) * 0.2f, 0.4f + corrector);
                }
                else if (direction == 2)
                {
                    posToGet = this.transform.position + new Vector3(0.6f - corrector, (int)(pos / 2) * 0.2f, 0.1f);
                }
                else if (direction == 3)
                {
                    posToGet = this.transform.position + new Vector3(0.1f, (int)(pos / 2) * 0.2f, 0.6f - corrector);
                }

                if (item.transform.position != posToGet)
                {
                    item.transform.position = posToGet;
                }
            }
            pos++;
        }
    }
}
