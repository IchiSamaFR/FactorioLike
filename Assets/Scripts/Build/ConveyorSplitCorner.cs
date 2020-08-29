using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorSplitCorner : Building
{
    Vector2[] dirToEject = new Vector2[2];
    sbyte[] dirCanceled = new sbyte[2];
    sbyte dirEject = 0;
    sbyte dir = 0;

    // Start is called before the first frame update
    void Update()
    {
        ChangeItemsPos();
    }

    public override void _init()
    {
        if (direction == 0)
        {
            dirToEject[0] = new Vector2(posX, posZ + 1);
            dirToEject[1] = new Vector2(posX + 1, posZ);
            dirCanceled[0] = 2;
            dirCanceled[1] = 3;
        }
        else if (direction == 1)
        {
            dirToEject[0] = new Vector2(posX + 1, posZ);
            dirToEject[1] = new Vector2(posX, posZ - 1);
            dirCanceled[0] = 3;
            dirCanceled[1] = 0;
        }
        else if (direction == 2)
        {
            dirToEject[0] = new Vector2(posX, posZ - 1);
            dirToEject[1] = new Vector2(posX - 1, posZ);
            dirCanceled[0] = 0;
            dirCanceled[1] = 1;
        }
        else if (direction == 3)
        {
            dirToEject[0] = new Vector2(posX - 1, posZ);
            dirToEject[1] = new Vector2(posX, posZ + 1);
            dirCanceled[0] = 1;
            dirCanceled[1] = 2;
        }

        this.itemsStockedMax = 3;
        itemsToEject = new GameObject[itemsStockedMax];
    }

    public override void Eject()
    {
        if (itemsToEject[0] != null)
        {
            GameObject obj_buildToDrop = null;
            if ((obj_buildToDrop = chunk.GetBlockAt((int)dirToEject[0].x, (int)dirToEject[0].y)))
            {
                Building buildToDrop = null;
                if ((buildToDrop = obj_buildToDrop.GetComponent<Building>()) && buildToDrop.CanDrop(dirCanceled[0]))
                {
                    if (obj_buildToDrop.GetComponent<Conveyor>())
                    {
                        if (buildToDrop.GetItem(itemsToEject[0], (int)(buildToDrop.itemsToEject.Length / 2)))
                        {
                            GameObject toDelete = itemsToEject[0];
                            itemsToEject[0] = null;
                            Destroy(toDelete);
                        }
                        else if (buildToDrop.GetItem(itemsToEject[0]))
                        {
                            GameObject toDelete = itemsToEject[0];
                            itemsToEject[0] = null;
                            Destroy(toDelete);
                        }
                    }
                    else if (buildToDrop.GetItem(itemsToEject[0]))
                    {
                        GameObject toDelete = itemsToEject[0];
                        itemsToEject[0] = null;
                        Destroy(toDelete);
                    }
                }
            }
        }
        if (itemsToEject[1] != null)
        {
            GameObject obj_buildToDrop = null;
            if ((obj_buildToDrop = chunk.GetBlockAt((int)dirToEject[1].x, (int)dirToEject[1].y)))
            {
                Building buildToDrop = null;
                if ((buildToDrop = obj_buildToDrop.GetComponent<Building>()) && buildToDrop.CanDrop(dirCanceled[1]))
                {
                    if (obj_buildToDrop.GetComponent<Conveyor>())
                    {
                        if (buildToDrop.GetItem(itemsToEject[1], (int)(buildToDrop.itemsToEject.Length / 2)))
                        {
                            GameObject toDelete = itemsToEject[1];
                            itemsToEject[1] = null;
                            Destroy(toDelete);
                        }
                        else if (buildToDrop.GetItem(itemsToEject[1]))
                        {
                            GameObject toDelete = itemsToEject[1];
                            itemsToEject[1] = null;
                            Destroy(toDelete);
                        }
                    }
                    else if (buildToDrop.GetItem(itemsToEject[1]))
                    {
                        GameObject toDelete = itemsToEject[1];
                        itemsToEject[1] = null;
                        Destroy(toDelete);
                    }
                }
            }
        }
    }
    public override bool CanDrop(sbyte dir)
    {
        if (dir == dirCanceled[0] || dir == dirCanceled[1])
        {
            return true;
        }
        else
        {
            return false;
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
                    if(i == 0)
                    {
                        toGo = this.transform.position + new Vector3(0.5f, 0.3f, 1f);
                    } 
                    else if (i == 1)
                    {
                        toGo = this.transform.position + new Vector3(1f, 0.3f, 0.5f);
                    }
                    else
                    {
                        toGo = this.transform.position + new Vector3(0.5f, 0.3f, 0.5f);
                    }
                }
                else if (direction == 1)
                {
                    if (i == 0)
                    {
                        toGo = this.transform.position + new Vector3(1f, 0.3f, 0.5f);
                    }
                    else if (i == 1)
                    {
                        toGo = this.transform.position + new Vector3(0.5f, 0.3f, 0f);
                    }
                    else
                    {
                        toGo = this.transform.position + new Vector3(0.5f, 0.3f, 0.5f);
                    }
                }
                else if (direction == 2)
                {
                    if (i == 0)
                    {
                        toGo = this.transform.position + new Vector3(0.5f, 0.3f, 0f);
                    }
                    else if (i == 1)
                    {
                        toGo = this.transform.position + new Vector3(0f, 0.3f, 0.5f);
                    }
                    else
                    {
                        toGo = this.transform.position + new Vector3(0.5f, 0.3f, 0.5f);
                    }
                }
                else if (direction == 3)
                {
                    if (i == 0)
                    {
                        toGo = this.transform.position + new Vector3(0f, 0.3f, 0.5f);
                    }
                    else if (i == 1)
                    {
                        toGo = this.transform.position + new Vector3(0.5f, 0.3f, 1f);
                    }
                    else
                    {
                        toGo = this.transform.position + new Vector3(0.5f, 0.3f, 0.5f);
                    }
                }

                //  This conveyor placement
                if (i == 2)
                {
                    if(itemsToEject[i].transform.position == toGo && itemsToEject[i - 1 - dir] == null)
                    {
                        itemsToEject[i - 1 - dir] = itemsToEject[i];
                        itemsToEject[i] = null;

                        ChangeDirEject();
                    }
                    else if (itemsToEject[i].transform.position == toGo && itemsToEject[i - 1 - dir] != null)
                    {
                        ChangeDirEject();
                        if (itemsToEject[i - 1 - dir] == null)
                        {
                            itemsToEject[i - 1 - dir] = itemsToEject[i];
                            itemsToEject[i] = null;
                        }
                        ChangeDirEject();
                    }
                }
                else if (i == 0 || i == 1)
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
                }
                else if (item.transform.position.z < toGo.z)
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
                }
                else if (item.transform.position.x < toGo.x)
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
    void ChangeDirEject()
    {
        if (dir == 0)
        {
            dir = 1;
        }
        else
        {
            dir = 0;
        }
    }
}
