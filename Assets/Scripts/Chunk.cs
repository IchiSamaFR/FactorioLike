using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UIElements;

public class Chunk : MonoBehaviour
{
    MapGenerator mapGen;
    public bool active = true;

    [Header("Needed")]
    public int width;
    public int length;
    public float scale = 1;

    [Header("Seed")]
    public int xPos;
    public int zPos;

    public int seed;


    [Header("Others")]
    public GameObject[,] groundBlocks = new GameObject[16, 16];
    public GameObject[,] buildedBlocks = new GameObject[16, 16];
    [SerializeField]
    List<MapGenLevel> levels = new List<MapGenLevel>();

    public GameObject Conveyor;

    #region ----- Setup -----
    void Start()
    {
        SpeedMapGen();
    }

    public void Set(int width, int length, float scale, int xPos, int zPos, int seed, List<MapGenLevel> levels, MapGenerator mapGen)
    {
        this.width = width;
        this.length = length;
        this.scale = scale;
        this.xPos = xPos;
        this.zPos = zPos;
        this.seed = seed;
        this.levels = levels;
        this.mapGen = mapGen;
    }
    #endregion
    /*
    public void GenBuild()
    {
        int posX = 2;
        int posZ = 15;
        GameObject newConveyor = Instantiate(Conveyor, this.transform);
        newConveyor.transform.position = this.transform.position + new Vector3(posX, 1, posZ);
        newConveyor.GetComponent<Conveyor>().Set(0, 4, this, posX, posZ);
        buildedBlocks[posX, posZ] = newConveyor;

        posX = 2;
        posZ = 0;
        newConveyor = Instantiate(Conveyor, this.transform);
        newConveyor.transform.position = this.transform.position + new Vector3(posX, 1, posZ);
        newConveyor.GetComponent<Conveyor>().Set(2, 4, this, posX, posZ);
        buildedBlocks[posX, posZ] = newConveyor;
        
        posX = 3;
        posZ = 0;
        newConveyor = Instantiate(Conveyor, this.transform);
        newConveyor.transform.position = this.transform.position + new Vector3(posX, 1, posZ);
        newConveyor.GetComponent<Conveyor>().Set(1, 4, this, posX, posZ);
        buildedBlocks[posX, posZ] = newConveyor;

        posX = 3;
        posZ = 15;
        newConveyor = Instantiate(Conveyor, this.transform);
        newConveyor.transform.position = this.transform.position + new Vector3(posX, 1, posZ);
        newConveyor.GetComponent<Conveyor>().Set(3, 4, this, posX, posZ);
        buildedBlocks[posX, posZ] = newConveyor;
        
    }
    */


    public bool GetInfoAt(int x, int z)
    {
        if(x < 0 || z < 0 ||
           x >= width || z >= length)
        {
            return true;
        } else if(groundBlocks[x, z])
        {
            return true;
        }

        return false;
    }

    public GameObject GetBlockAt(int x, int z)
    {
        if (x < 0 || z < 0 ||
           x >= width || z >= length)
        {
            int searchX = xPos;
            int searchZ = zPos;
            int blockX = x;
            int blockZ = z;

            if(x < 0)
            {
                searchX = xPos - 1;
                blockX += 16;
            } 
            else if (x >= width)
            {
                searchX = xPos + 1;
                blockX -= 16;
            }
            if (z < 0)
            {
                searchZ = zPos - 1;
                blockZ += 16;
            }
            else if (z >= length)
            {
                searchZ = zPos + 1;
                blockZ -= 16;
            }
            
            GameObject obj = mapGen.GetChunkAt(searchX, searchZ);

            if(obj != null)
            {
                return obj.GetComponent<Chunk>().GetBlockAt(blockX, blockZ);
            }

            return null;
        }
        else if (buildedBlocks[x, z])
        {
            return buildedBlocks[x, z];
        }

        return null;
    }

    public void DestroyBuild(int x, int z)
    {
        GameObject Save = buildedBlocks[x, z];
        Destroy(Save);
        buildedBlocks[x, z] = null;
        RefreshBlocs(x, z);
    }

    public void RefreshBlocs(int posX, int posZ)
    {
        GameObject ToCheck;
        if((ToCheck = GetBlockAt(posX + 1, posZ)) != null)
        {
            ToCheck.GetComponent<Building>().SetModel();
        }
        if ((ToCheck = GetBlockAt(posX, posZ + 1)) != null)
        {
            ToCheck.GetComponent<Building>().SetModel();
        }
        if ((ToCheck = GetBlockAt(posX - 1, posZ)) != null)
        {
            ToCheck.GetComponent<Building>().SetModel();
        }
        if ((ToCheck = GetBlockAt(posX, posZ - 1)) != null)
        {
            ToCheck.GetComponent<Building>().SetModel();
        }
    }


    public void SpeedMapGen()
    {
        //StartCoroutine("IE_SpeedMapGen");

        
        List<BlocToGen> mapBlocks = new List<BlocToGen>();

        mapBlocks = NoiseMap.Create(width, length, scale, xPos, zPos, seed, levels);

        foreach (BlocToGen tile in mapBlocks)
        {
            GameObject cube = Instantiate(Tools.GetMapGenLev(levels, tile.id).model, this.transform);
            cube.transform.position = this.transform.position + new Vector3(tile.x, 0, tile.z);
            cube.GetComponent<Block>().Set(tile.x, tile.z, tile.id, this);
            groundBlocks[(int)tile.x, (int)tile.z] = cube;
        }
        
    }
    /*
    public IEnumerator IE_SpeedMapGen()
    {
        float time = 2;
        float timeStart = Time.time;
        int blockToGen = 256;
        int blockGen = 0;
        List<BlocToGen> mapBlocks = new List<BlocToGen>();


        mapBlocks = NoiseMap.Create(width, length, scale, xPos, zPos, seed, levels);

        foreach (BlocToGen tile in mapBlocks)
        {
            GameObject cube = Instantiate(levels[tile.id].model, this.transform);
            cube.transform.position = this.transform.position + new Vector3(tile.x, 0, tile.z);
            cube.GetComponent<Block>().Set(tile.x, tile.z, tile.id, this);
            groundBlocks[(int)tile.x, (int)tile.z] = cube;

            blockGen++;

            float percentTime = (Time.time - timeStart) / time;
            float percentBuild = blockGen / blockGen;
            if (percentTime > percentBuild)
            {
                yield return null;
            } else
            {
                yield return new WaitForSeconds(percentTime - percentBuild * time);
            }
        }
    }*/




    #region ----- Optimization -----
    public void DontShowChild()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.GetComponent<MeshRenderer>())
            {
                child.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        active = false;
    }

    public void ShowChild()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.GetComponent<MeshRenderer>())
            {
                child.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        active = true;
    }
    #endregion
}

public class BlocToGen {
    public string id;
    public int x;
    public int z;

    public BlocToGen(string id, int x, int z)
    {
        this.id = id;
        this.x = x;
        this.z = z;
    }
}
