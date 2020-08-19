﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    Chunk chunk;
    public GameObject[] ores = new GameObject[4];
    public float speed = 2;
    public int oresPerConveyor = 4;
    public int posX;
    public int posZ;

    public sbyte direction = 0;

    public Conveyor(sbyte direction, int oresPerConveyor, Chunk chunk, int posX, int posZ)
    {
        this.direction = direction;
        this.oresPerConveyor = oresPerConveyor;
        this.chunk = chunk;
        this.posX = posX;
        this.posZ = posZ;
    }
    public void Set(sbyte direction, int oresPerConveyor, Chunk chunk, int posX, int posZ)
    {
        this.direction = direction;
        this.oresPerConveyor = oresPerConveyor;
        this.chunk = chunk;
        this.posX = posX;
        this.posZ = posZ;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(direction < 2)
        {
            this.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
        } else
        {
            this.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        int i = 0;
        foreach (GameObject ore in ores)
        {
            if (ore != null)
            {
                if (direction == 0)
                {
                    ore.transform.position = this.transform.position + new Vector3(0.5f, 0.3f, 1-(1f / oresPerConveyor) * i);
                }
                else if (direction == 1)
                {
                    ore.transform.position = this.transform.position + new Vector3(0.5f, 0.3f, (1f / oresPerConveyor) * i);
                }
                else if (direction == 2)
                {
                    ore.transform.position = this.transform.position + new Vector3(1-(1f / oresPerConveyor) * i, 0.3f, 0.5f);
                }
                else if (direction == 3)
                {
                    ore.transform.position = this.transform.position + new Vector3((1f / oresPerConveyor) * i, 0.3f, 0.5f);
                }
            }
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time < 2)
        {
            return;
        }

        OreMovement();
    }


    //  Movement of ores / items
    void OreMovement()
    {
        int i = 0;
        foreach (GameObject ore in ores)
        {
            if (ore != null)
            {
                Vector3 toGo = new Vector3();
                if (direction == 0)
                {
                    toGo = this.transform.position + new Vector3(0.5f, 0.3f, 1-(1f / oresPerConveyor) * i);
                } 
                else if (direction == 1)
                {
                    toGo = this.transform.position + new Vector3(0.5f, 0.3f, (1f / oresPerConveyor) * i);
                }
                else if (direction == 2)
                {
                    toGo = this.transform.position + new Vector3(1-(1f / oresPerConveyor) * i, 0.3f, 0.5f);
                }
                else if (direction == 3)
                {
                    toGo = this.transform.position + new Vector3((1f / oresPerConveyor) * i, 0.3f, 0.5f);
                }

                if (i != 0 && ores[i - 1] == null && ores[i].transform.position == toGo)
                {
                    ores[i - 1] = ores[i];
                    ores[i] = null;
                } 
                else if (i == 0)
                {
                    GameObject obj = null;
                    sbyte directionCanceled = 0;
                    if (direction == 0)
                    {
                        obj = chunk.GetBlockAt(posX, posZ + 1);
                        directionCanceled = 1;
                    }
                    else if (direction == 1)
                    {
                        obj = chunk.GetBlockAt(posX, posZ - 1);
                        directionCanceled = 0;
                    }
                    else if (direction == 2)
                    {
                        obj = chunk.GetBlockAt(posX + 1, posZ);
                        directionCanceled = 3;
                    }
                    else if (direction == 3)
                    {
                        obj = chunk.GetBlockAt(posX - 1, posZ);
                        directionCanceled = 2;
                    }

                    Conveyor NextConveyor = null;

                    if (obj != null && obj.GetComponent<Conveyor>())
                    {
                        NextConveyor = obj.GetComponent<Conveyor>();
                        if (obj && NextConveyor.direction != directionCanceled)
                        {
                            if (NextConveyor.direction != direction && NextConveyor.ores[(int)(NextConveyor.ores.Length / 2)] == null)
                            {
                                NextConveyor.GetOre(ores[0], (int)(NextConveyor.ores.Length / 2));
                                GameObject toDelete = ores[0];
                                ores[0] = null;
                                Destroy(toDelete);
                            } else if (NextConveyor.direction == direction && NextConveyor.ores[NextConveyor.ores.Length - 1] == null)
                            {
                                NextConveyor.GetOre(ores[0], NextConveyor.ores.Length - 1);
                                GameObject toDelete = ores[0];
                                ores[0] = null;
                                Destroy(toDelete);
                            }
                        }
                    }

                }

                
                if (ore.transform.position.z > toGo.z)
                {
                    ore.transform.position += new Vector3(0, 0, -Time.deltaTime * speed);

                    if (ore.transform.position.z < toGo.z)
                    {
                        ore.transform.position = toGo;
                    }
                } else if (ore.transform.position.z < toGo.z)
                {
                    ore.transform.position += new Vector3(0, 0, Time.deltaTime * speed);

                    if (ore.transform.position.z > toGo.z)
                    {
                        ore.transform.position = toGo;
                    }
                }

                if (ore.transform.position.x > toGo.x)
                {
                    ore.transform.position += new Vector3(-Time.deltaTime * speed, 0, 0);

                    if (ore.transform.position.x < toGo.x)
                    {
                        ore.transform.position = toGo;
                    }
                } else if (ore.transform.position.x < toGo.x)
                {
                    ore.transform.position += new Vector3(Time.deltaTime * speed, 0, 0);

                    if (ore.transform.position.x > toGo.x)
                    {
                        ore.transform.position = toGo;
                    }
                }
            }
            i++;
        }
    }

    public void GetOre(GameObject newOre, int pos)
    {
        GameObject oreInstantiate = Instantiate(newOre, this.transform);
        oreInstantiate.transform.position = newOre.transform.position;
        ores[pos] = oreInstantiate;

        if (chunk.active)
        {
            oreInstantiate.GetComponent<MeshRenderer>().enabled = true;
        } 
        else
        {
            oreInstantiate.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}