using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Conveyor : Building
{
    bool isInit = false;
    void Update()
    {
        ChangeItemsPos();
    }

    public override void _init()
    {
        if (isInit)
        {
            return;
        }
        else
        {
            isInit = true;
            SetModel();
        }
        foreach (Transform test in transform)
        {
            Destroy(test.gameObject);
        }
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

    public override void SetModel()
    {
        
        bool[] LBR = new bool[3];

        GameObject toCheck;
        if ((toCheck = chunk.GetBlockAt(posX, posZ + 1)) != null && direction != 0)
        {
            Building buildingCheck;
            if ((buildingCheck = toCheck.GetComponent<Building>()) != null)
            {
                if (buildingCheck.HasDirection(2))
                {
                    if (direction - 1 < 0)
                    {
                        LBR[direction - 1 + 4] = true;
                    }
                    else
                    {
                        LBR[direction - 1] = true;
                    }
                }
            }
        }
        if ((toCheck = chunk.GetBlockAt(posX + 1, posZ)) != null && direction != 1)
        {
            Building buildingCheck;
            if ((buildingCheck = toCheck.GetComponent<Building>()) != null)
            {
                if (buildingCheck.HasDirection(3))
                {
                    if (direction - 2 < 0)
                    {
                        LBR[direction - 2 + 4] = true;
                    }
                    else
                    {
                        LBR[direction - 2] = true;
                    }
                }
            }
        }
        if ((toCheck = chunk.GetBlockAt(posX, posZ - 1)) != null && direction != 2)
        {
            Building buildingCheck;
            if ((buildingCheck = toCheck.GetComponent<Building>()) != null)
            {
                if (buildingCheck.HasDirection(0))
                {
                    if (direction - 3 < 0)
                    {
                        LBR[direction - 3 + 4] = true;
                    }
                    else
                    {
                        LBR[direction - 3] = true;
                    }
                }
            }
        }
        if ((toCheck = chunk.GetBlockAt(posX - 1, posZ)) != null && direction != 3)
        {
            Building buildingCheck;
            if ((buildingCheck = toCheck.GetComponent<Building>()) != null)
            {
                if (buildingCheck.HasDirection(1))
                {
                    if (direction - 4 < 0)
                    {
                        LBR[direction - 4 + 4] = true;
                    }
                    else
                    {
                        LBR[direction - 4] = true;
                    }
                }
            }
        }

        GameObject model = null;
        if (LBR[0])
        {
            if (LBR[1])
            {
                if (LBR[2])
                {
                    model = models[6];
                } 
                else
                {
                    model = models[2];
                }
            }
            else
            {
                if (LBR[2])
                {
                    model = models[5];
                }
                else
                {
                    model = models[1];
                }
            }
        } 
        else
        {
            if (LBR[1])
            {
                if (LBR[2])
                {
                    model = models[4];
                }
                else
                {
                    model = models[0];
                }
            }
            else
            {
                if (LBR[2])
                {
                    model = models[3];
                }
                else
                {
                    model = models[0];
                }
            }
        }

        if ((modelSet == null && model != null) || model != modelSet)
        {
            Destroy(modelSet);
            modelSet = Instantiate(model, this.transform);
            modelSet.transform.position = this.transform.position + new Vector3(0.5f, 0, 0.5f);
            modelSet.transform.rotation = Quaternion.Euler(0, direction * 90, 0);
        }
    }
}
