using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public static Builder instance;

    public List<Build> buildsAvailable = new List<Build>();
    public Build selected = null;

    public GameObject preshowBuild;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        Select(0);
    }

    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            Select(0);
        }
        else if (Input.GetKeyDown("2"))
        {
            Select(1);
        }
        else if (Input.GetKeyDown("3"))
        {
            Select(2);
        }
        else if (Input.GetKeyDown("4"))
        {
            Select(3);
        }
        else if (Input.GetKeyDown("escape"))
        {
            Unselect();
        }
    }

    public void Preshow(GameObject target, sbyte direction)
    {
        if (selected != null && selected.shadowPrefab != preshowBuild)
        {
            Destroy(preshowBuild);
            preshowBuild = Instantiate(selected.shadowPrefab);
            preshowBuild.transform.position = target.transform.position + new Vector3(0, 1, 0);
            preshowBuild.transform.GetChild(0).rotation = Quaternion.Euler(0, direction * 90, 0);
        }
        else
        {
            Destroy(preshowBuild);
        }
    }

    public void Build(GameObject target, sbyte direction)
    {
        Block block = target.GetComponent<Block>();
        Chunk chunk = block.chunk;
        int x = block.posX;
        int z = block.posZ;

        if (!chunk.buildedBlocks[x, z])
        {

            GameObject newBuild = Instantiate(selected.prefab, chunk.transform);
            newBuild.transform.position = chunk.transform.position + new Vector3(x, 1, z);
            newBuild.GetComponent<Building>().Set(direction, 4, chunk, x, z);

            if (selected.prefab.GetComponent<Conveyor>())
            {
                newBuild.GetComponent<Building>().Set(direction, 2, chunk, x, z);
            }


            chunk.buildedBlocks[x, z] = newBuild;

            if (selected.prefab.GetComponent<DrillingMachine>() && chunk.groundBlocks[x, z].GetComponent<Block>().Type != "grass")
            {
                newBuild.transform.position = chunk.transform.position + new Vector3(x, 1, z);
                newBuild.GetComponent<Building>().Set(direction, 4, chunk, x, z);
                newBuild.GetComponent<DrillingMachine>().AddItemToProduce(chunk.groundBlocks[x, z].GetComponent<Block>().Type);
                chunk.buildedBlocks[x, z] = newBuild;
            }
            //block.chunk.AddBuild(block.posX, block.posZ, selected.prefab, direction);
        }
    }

    public void DestroyBuild(GameObject target)
    {
        Block block = target.GetComponent<Block>();
        block.chunk.DestroyBuild(block.posX, block.posZ);
    }

    public void StopPreshowBuild()
    {
        if (preshowBuild)
        {
            Destroy(preshowBuild);
        }
    }

    void Select(int i)
    {
        selected = null;
        if(buildsAvailable.Count >= i && buildsAvailable[i] != null)
        {
            selected = buildsAvailable[i];
        }
    }

    void Unselect()
    {
        selected = null;
    }
}

[System.Serializable]
public class Build
{
    public string name;
    public GameObject prefab;
    public GameObject shadowPrefab;
}