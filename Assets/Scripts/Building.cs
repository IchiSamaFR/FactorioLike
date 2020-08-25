using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Chunk chunk;
    public GameObject[] itemsToEject = new GameObject[0];

    public float speed = 2;
    public int itemsStockedMax = 4;
    public int posX;
    public int posZ;


    public sbyte direction = 0;
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
        SetRotation();
    }

    void SetRotation()
    {
        this.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, direction * 90, 0);
    }

    public void Eject()
    {
        if (itemsToEject[0] != null)
        {
            GameObject obj_buildToDrop = null;
            if ((obj_buildToDrop = chunk.GetBlockAt((int)posToEject.x, (int)posToEject.y)))
            {
                Building buildToDrop = null;
                if((buildToDrop = obj_buildToDrop.GetComponent<Building>()) && buildToDrop.direction != directionCanceled)
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
                    else if (obj_buildToDrop.GetComponent<Smeltery>())
                    {
                        if (buildToDrop.GetComponent<Smeltery>().GetItem(itemsToEject[0]))
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

    public bool GetItem(GameObject newItem, int pos)
    {
        if (itemsToEject[pos] == null)
        {
            GameObject oreInstantiate = Instantiate(newItem, this.transform);
            oreInstantiate.transform.position = newItem.transform.position;
            itemsToEject[pos] = oreInstantiate;

            if (chunk.active)
            {
                oreInstantiate.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                oreInstantiate.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            }
            return true;
        } else
        {
            return false;
        }
    }

    public bool GetItem(GameObject newItem)
    {
        if (itemsToEject[itemsStockedMax - 1] == null)
        {
            GameObject oreInstantiate = Instantiate(newItem, this.transform);
            itemsToEject[itemsStockedMax - 1] = oreInstantiate;

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
        } else
        {
            return false;
        }
    }
}
