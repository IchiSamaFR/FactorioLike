using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrillingMachine : MonoBehaviour
{
    Chunk chunk;
    public GameObject[] ores = new GameObject[0];
    public GameObject item;

    public float speed = 2;
    float timer;
    public int oresStockedMax = 4;
    public int posX;
    public int posZ;


    public sbyte direction = 0;
    sbyte directionCanceled = 0;
    Vector2 posToSend = new Vector2();


    public DrillingMachine(sbyte direction, int oresStockedMax, Chunk chunk, int posX, int posZ, string oreId)
    {
        Set(direction, oresStockedMax, chunk, posX, posZ, oreId);
    }
    public void Set(sbyte direction, int oresStockedMax, Chunk chunk, int posX, int posZ, string oreId)
    {
        //  Set of all variable
        this.direction = direction;
        this.oresStockedMax = oresStockedMax;
        this.chunk = chunk;
        this.posX = posX;
        this.posZ = posZ;

        item = Items.instance.GetOreByTile(oreId);
        if(item == null)
        {
            Debug.Log("L'id du bloc n'est pas reconnu");
            speed = 0;
        }
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
    }
    void Update()
    {
        CreateOre();
        Eject();
    }

    void CreateOre()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (Tools.Count(ores) != oresStockedMax)
        {
            AddOre();
            timer = speed;
        }
    }

    void AddOre()
    {
        GameObject obj = Instantiate(item, this.transform);
        int pos = Tools.Count(ores);
        ores[pos] = obj;
        ChangeItemsPos();
    }

    void ChangeItemsPos()
    {
        int pos = 0;
        foreach(GameObject ore in ores)
        {
            if(ore != null)
            {
                float corrector = 0;
                if (pos % 2 == 0)
                {
                    corrector = 0.2f;
                }

                Vector3 posToGet = new Vector3();
                if (direction == 0)
                {
                    posToGet = this.transform.position + new Vector3(0.4f + corrector, (int)((pos - 1) / 2) * 0.2f, 0.9f);
                } 
                else if (direction == 1)
                {
                    posToGet = this.transform.position + new Vector3(0.9f, (int)((pos - 1) / 2) * 0.2f, 0.4f + corrector);
                }
                else if (direction == 2)
                {
                    posToGet = this.transform.position + new Vector3(0.6f - corrector, (int)((pos - 1) / 2) * 0.2f, 0.1f);
                }
                else if (direction == 3)
                {
                    posToGet = this.transform.position + new Vector3(0.1f, (int)((pos - 1) / 2) * 0.2f, 0.6f - corrector);
                }
                if (ore.transform.position != posToGet)
                {
                    ore.transform.position = posToGet;
                }
            }
            pos++;
        }
    }

    void Eject()
    {
        int i = 0;
        bool eject = false;
        foreach(GameObject ore in ores)
        {
            if(ore != null)
            {
                if (i != 0 && ores[i - 1] == null)
                {
                    ores[i - 1] = ores[i];
                    ores[i] = null;
                    eject = true;
                }
                else if (i == 0)
                {
                    GameObject buildToDrop = null;
                    buildToDrop = chunk.GetBlockAt((int)posToSend.x, (int)posToSend.y);



                    if (buildToDrop != null && buildToDrop.GetComponent<Conveyor>())
                    {
                        Conveyor nextConveyor = null;
                        nextConveyor = buildToDrop.GetComponent<Conveyor>();

                        if (buildToDrop && nextConveyor.direction != directionCanceled)
                        {
                            if (nextConveyor.direction != direction && nextConveyor.itemsToEject[(int)(nextConveyor.itemsToEject.Length / 2)] == null)
                            {
                                nextConveyor.GetItem(ores[0], (int)(nextConveyor.itemsToEject.Length / 2));
                                GameObject toDelete = ores[0];
                                ores[0] = null;
                                eject = true;
                                Destroy(toDelete);
                            }
                            else if (nextConveyor.direction == direction && nextConveyor.itemsToEject[nextConveyor.itemsToEject.Length - 1] == null)
                            {
                                nextConveyor.GetItem(ores[0], nextConveyor.itemsToEject.Length - 1);
                                GameObject toDelete = ores[0];
                                ores[0] = null;
                                eject = true;
                                Destroy(toDelete);
                            }
                        }
                    }
                }
            }
            i++;
        }

        if (eject)
        {
            ChangeItemsPos();
        }
    }
}
