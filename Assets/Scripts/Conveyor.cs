using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : Building
{
    Vector2 posToSend = new Vector2();

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if(Time.time < 2)
        {
            return;
        }
        ChangeItemsPos();
    }


    public void ChangeItemsPos()
    {
        int i = 0;
        foreach (GameObject item in itemsToEject)
        {
            if (item != null)
            {
                //  Where to go by item placement
                Vector3 toGo = new Vector3();
                if (direction == 0)
                {
                    toGo = this.transform.position + new Vector3(0.5f, 0.3f, 1 - (1f / itemsStockedMax) * i);
                }
                else if (direction == 1)
                {
                    toGo = this.transform.position + new Vector3(1 - (1f / itemsStockedMax) * i, 0.3f, 0.5f);
                }
                else if (direction == 2)
                {
                    toGo = this.transform.position + new Vector3(0.5f, 0.3f, (1f / itemsStockedMax) * i);
                }
                else if (direction == 3)
                {
                    toGo = this.transform.position + new Vector3((1f / itemsStockedMax) * i, 0.3f, 0.5f);
                }

                //  This conveyor placement
                if (i != 0 && itemsToEject[i - 1] == null && itemsToEject[i].transform.position == toGo)
                {
                    itemsToEject[i - 1] = itemsToEject[i];
                    itemsToEject[i] = null;
                }

                //  If ores is already to the less position check a conveyor or something else
                else if (i == 0)
                {
                    Eject();
                }

                if (item.transform.position.z > toGo.z)
                {
                    item.transform.position += new Vector3(0, 0, -Time.deltaTime * speed);

                    if (item.transform.position.z < toGo.z)
                    {
                        item.transform.position = toGo;
                    }
                } else if (item.transform.position.z < toGo.z)
                {
                    item.transform.position += new Vector3(0, 0, Time.deltaTime * speed);

                    if (item.transform.position.z > toGo.z)
                    {
                        item.transform.position = toGo;
                    }
                }

                if (item.transform.position.x > toGo.x)
                {
                    item.transform.position += new Vector3(-Time.deltaTime * speed, 0, 0);

                    if (item.transform.position.x < toGo.x)
                    {
                        item.transform.position = toGo;
                    }
                } else if (item.transform.position.x < toGo.x)
                {
                    item.transform.position += new Vector3(Time.deltaTime * speed, 0, 0);

                    if (item.transform.position.x > toGo.x)
                    {
                        item.transform.position = toGo;
                    }
                }
            }
            i++;
        }
    }


}
