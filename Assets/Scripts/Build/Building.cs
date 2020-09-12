using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("Models")]
    public GameObject modelSet = null;
    public GameObject[] models;


    [Header("Chunk")]
    public Chunk chunk;

    [Header("Items Stocked")]
    public GameObject[] itemsToEject = new GameObject[0];

    [Header("Stats")]
    public float speed = 2;
    public int itemsStockedMax = 4;
    public int posX;
    public int posZ;


    public sbyte direction = 0;
    public bool multipleDir = false;
    public sbyte directionCanceled = 0;
    public Vector2 posToEject = new Vector2();

    public void Set(sbyte direction, int itemsStockedMax, Chunk chunk, int posX, int posZ)
    {
        this.direction = direction;
        this.itemsStockedMax = itemsStockedMax;
        this.chunk = chunk;
        this.posX = posX;
        this.posZ = posZ;

        itemsToEject = new GameObject[itemsStockedMax];

        if (direction == 0)
        {
            posToEject = new Vector2(posX, posZ + 1);
            directionCanceled = 2;
        }
        else if (direction == 1)
        {
            posToEject = new Vector2(posX + 1, posZ);
            directionCanceled = 3;
        }
        else if (direction == 2)
        {
            posToEject = new Vector2(posX, posZ - 1);
            directionCanceled = 0;
        }
        else if (direction == 3)
        {
            posToEject = new Vector2(posX - 1, posZ);
            directionCanceled = 1;
        }
        _init();
        SetRotation();
        SetModel();
    }

    public virtual void _init()
    {
    }

    void SetRotation()
    {
        transform.GetChild(0).transform.rotation = Quaternion.Euler(0, direction * 90, 0);
    }

    public virtual void SetModel()
    {
    }

    public virtual void Eject()
    {
        if (itemsToEject[0] != null)
        {
            GameObject obj_buildToDrop = null;
            if ((obj_buildToDrop = chunk.GetBlockAt((int)posToEject.x, (int)posToEject.y)))
            {
                Building buildToDrop = null;
                if((buildToDrop = obj_buildToDrop.GetComponent<Building>()) && buildToDrop.CanDrop(directionCanceled))
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
    }

    public virtual bool CanDrop(sbyte directionCanceled)
    {
        //  If the direction is not canceled it can be droped
        if(directionCanceled != direction)
        {
            return true;
        } 
        else
        {
            return false;
        }
    }

    public virtual sbyte GetDirection()
    {
        return direction;
    }
    public virtual sbyte GetDirection(Vector2 dir)
    {
        return direction;
    }
    public virtual bool HasDirection(int dirWanted)
    {
        if(direction == dirWanted)
        {
            return true;
        }
        return false;
    }

    public virtual bool GetItem(GameObject newItem, int pos)
    {
        if (itemsToEject[pos] == null)
        {
            GameObject itemInstantiate = Instantiate(newItem, this.transform);
            itemInstantiate.transform.position = newItem.transform.position;
            itemsToEject[pos] = itemInstantiate;

            if (chunk.active)
            {
                itemInstantiate.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                itemInstantiate.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            }
            return true;
        } else
        {
            return false;
        }
    }

    public virtual bool GetItem(GameObject newItem)
    {
        if (itemsToEject[itemsStockedMax - 1] == null)
        {
            GameObject itemInstantiate = Instantiate(newItem, this.transform);
            itemInstantiate.transform.position = newItem.transform.position;
            itemsToEject[itemsStockedMax - 1] = itemInstantiate;


            if (chunk.active)
            {
                itemInstantiate.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                itemInstantiate.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            }
            return true;
        } else
        {
            return false;
        }
    }
}
