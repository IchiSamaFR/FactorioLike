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

        string show = "";
        if (LBR[2])
        {
            show += "R / ";
        }
        if (LBR[0])
        {
            show += "L / ";
        }
        if (LBR[1])
        {
            show += "B  / ";
        }


        GameObject model = null;
        if (LBR[0])
        {
            if (LBR[1])
            {
                if (LBR[2])
                {
                    DestroyModels();
                    model = Instantiate(models[6], this.transform);
                    model.transform.position = this.transform.position + new Vector3(0.5f, 0, 0.5f);
                } 
                else
                {
                    DestroyModels();
                    model = Instantiate(models[2], this.transform);
                    model.transform.position = this.transform.position + new Vector3(0.5f, 0, 0.5f);
                }
            }
            else
            {
                if (LBR[2])
                {
                    DestroyModels();
                    model = Instantiate(models[5], this.transform);
                    model.transform.position = this.transform.position + new Vector3(0.5f, 0, 0.5f);
                }
                else
                {
                    DestroyModels();
                    model = Instantiate(models[1], this.transform);
                    model.transform.position = this.transform.position + new Vector3(0.5f, 0, 0.5f);
                }
            }
        } 
        else
        {
            if (LBR[1])
            {
                if (LBR[2])
                {
                    DestroyModels();
                    model = Instantiate(models[4], this.transform);
                    model.transform.position = this.transform.position + new Vector3(0.5f, 0, 0.5f);
                }
                else
                {
                    DestroyModels();
                    model = Instantiate(models[0], this.transform);
                    model.transform.position = this.transform.position + new Vector3(0.5f, 0, 0.5f);
                }
            }
            else
            {
                if (LBR[2])
                {
                    DestroyModels();
                    model = Instantiate(models[3], this.transform);
                    model.transform.position = this.transform.position + new Vector3(0.5f, 0, 0.5f);
                }
                else
                {
                    DestroyModels();
                    model = Instantiate(models[0], this.transform);
                    model.transform.position = this.transform.position + new Vector3(0.5f, 0, 0.5f);
                }
            }
        }
        if (model != null)
        {
            model.transform.rotation = Quaternion.Euler(0, direction * 90, 0);
        }
    }

    void DestroyModels()
    {
        foreach(Transform test in transform)
        {
            Destroy(test.gameObject);
        }
    }
}
