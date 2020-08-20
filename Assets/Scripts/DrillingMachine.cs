using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrillingMachine : MonoBehaviour
{
    Chunk chunk;
    public GameObject[] ores = new GameObject[0];
    public Ore ore;

    public float speed = 2;
    float timer;
    public int oresStockedMax = 4;
    public int posX;
    public int posZ;


    public sbyte direction = 0;
    sbyte directionCanceled = 0;
    Vector2 posToSend = new Vector2();


    public DrillingMachine(sbyte direction, int oresStockedMax, Chunk chunk, int posX, int posZ, int oreId)
    {
        Set(direction, oresStockedMax, chunk, posX, posZ, oreId);
    }
    public void Set(sbyte direction, int oresStockedMax, Chunk chunk, int posX, int posZ, int oreId)
    {
        //  Set of all variable
        this.direction = direction;
        this.oresStockedMax = oresStockedMax;
        this.chunk = chunk;
        this.posX = posX;
        this.posZ = posZ;

        ore = Ores.instance.GetOre(oreId - 1);
        ores = new GameObject[oresStockedMax];



        //  Set of the position
        if (direction == 0)
        {
            posToSend = new Vector2(posX, posZ + 1);
            directionCanceled = 2;
        }
        else if (direction == 1)
        {
            posToSend = new Vector2(posX + 1, posZ);
            directionCanceled = 3;
        }
        else if (direction == 2)
        {
            posToSend = new Vector2(posX, posZ - 1);
            directionCanceled = 0;
        }
        else if (direction == 3)
        {
            posToSend = new Vector2(posX - 1, posZ);
            directionCanceled = 1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = speed;


        if (direction == 0)
        {
            this.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == 1)
        {
            this.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (direction == 2)
        {
            this.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direction == 3)
        {
            this.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 270, 0);
        }
    }
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        } else if (Tools.Count(ores) != oresStockedMax)
        {
            AddOre();
            timer = speed;
        }
        Eject();
    }

    void AddOre()
    {
        GameObject obj = Instantiate(ore.prefab, this.transform);
        ores[Tools.Count(ores)] = obj;
        obj.transform.position = this.transform.position;
    }

        // Update is called once per frame
    void Eject()
    {
        int i = 0;
        foreach(GameObject ore in ores)
        {
            if(ore != null)
            {
                if (i != 0 && ores[i - 1] == null)
                {
                    ores[i - 1] = ores[i];
                    ores[i] = null;
                }
                else if (i == 0)
                {
                    GameObject obj = null;
                    obj = chunk.GetBlockAt((int)posToSend.x, (int)posToSend.y);


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
                            }
                            else if (NextConveyor.direction == direction && NextConveyor.ores[NextConveyor.ores.Length - 1] == null)
                            {
                                NextConveyor.GetOre(ores[0], NextConveyor.ores.Length - 1);
                                GameObject toDelete = ores[0];
                                ores[0] = null;
                                Destroy(toDelete);
                            }
                        }
                    }
                }
            }
            i++;
        }
    }
}
