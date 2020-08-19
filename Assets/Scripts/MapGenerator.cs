using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    [Header("Chunks Size")]
    public int width = 16;
    public int length = 16;
    public int height = 2;
    public float scale = 0.5f;

    [Header("Chunk")]
    public GameObject chunkPrefab;
    public int viewDistance = 3;

    [Header("Levels")]
    [SerializeField]
    List<MapGenLevel> levels = new List<MapGenLevel>();

    [Header("Seed")]
    public int seed;

    List<Chunk> mapChunks = new List<Chunk>();
    GameObject[,] chunksXZ = new GameObject[128, 128];
    GameObject[,] chunks_XZ = new GameObject[128, 128];
    GameObject[,] chunksX_Z = new GameObject[128, 128];
    GameObject[,] chunks_X_Z = new GameObject[128, 128];

    public GameObject player;
    Vector3 playerPos;

    void Start()
    {
        //seed = Random.Range(0, 2000);
        seed = 667;
        ChunkCreator();
    }


    public GameObject GetChunkAt(int x, int z)
    {
        //  Check by world pos
        if (x >= 0)
        {
            if(z >= 0 && chunksXZ[x, z])
            {
                return chunksXZ[x, z];
            } else if (z < 0 && chunksX_Z[x, -z])
            {
                return chunksX_Z[x, -z];
            }
        } 
        else if(x < 0)
        {
            if (z >= 0 && chunks_XZ[-x, z])
            {
                return chunks_XZ[-x, z];
            }
            else if (z < 0 && chunks_X_Z[-x, -z])
            {
                return chunks_X_Z[-x, -z];
            }
        }
        return null;
    }

    void Update()
    {
        CheckPlayerPos();
    }

    void CheckPlayerPos()
    {
        int playerX = (int)(player.transform.position.x / 16);
        int playerZ = (int)(player.transform.position.z / 16);

        if(player.transform.position.x < 0)
        {
            playerX -= 1;
        }
        if (player.transform.position.z < 0)
        {
            playerZ -= 1;
        }

        if (playerPos.x != playerX || playerPos.z != playerZ)
        {
            playerPos = new Vector3(playerX, 0, playerZ);
            ChunkCreator();
        }
    }

    void ChunkCreator()
    {
        DeleteMap();

        int xCorrector = 0;
        int zCorrector = 0;

        // For each view distance, add a chunk
        for (int x = 0; x < viewDistance * 2 + 1; x++)
        {
            for (int z = 0; z < viewDistance * 2 + 1; z++)
            {
                int zIndex = (int)(z - viewDistance + playerPos.z + zCorrector);
                int xIndex = (int)(x - viewDistance + playerPos.x + xCorrector);

                /* Create 4 Region and for each region depends of where is the player,
                 * Set the chunk in a region
                 * +X +Z
                 * +X -Z
                 * -X +Z
                 * -X -Z
                 * 
                 * Instatiate the chunk
                 * Set the Chunk (Pos / Stats)
                 * and set it in the region
                 * 
                 */
                if (xIndex >= 0)
                {
                    if (zIndex >= 0)
                    {
                        if (!chunksXZ[xIndex, zIndex])
                        {
                            GameObject newChunk = Instantiate(chunkPrefab, this.transform);
                            newChunk.transform.position = new Vector3(xIndex * width, 0, zIndex * length);
                            chunksXZ[xIndex, zIndex] = newChunk;

                            newChunk.GetComponent<Chunk>().Set(width, length, scale, xIndex, zIndex, seed, levels, this);

                            newChunk.GetComponent<Chunk>().SpeedMapGen();

                            mapChunks.Add(newChunk.GetComponent<Chunk>());
                        }
                        else
                        {
                            chunksXZ[xIndex, zIndex].GetComponent<Chunk>().ShowChild();
                        }
                    }
                    else if (zIndex < 0)
                    {
                        if (!chunksX_Z[xIndex, -zIndex])
                        {
                            GameObject newChunk = Instantiate(chunkPrefab, this.transform);
                            newChunk.transform.position = new Vector3(xIndex * width, 0, zIndex * length);
                            chunksX_Z[xIndex, -zIndex] = newChunk;

                            newChunk.GetComponent<Chunk>().Set(width, length, scale, xIndex, zIndex, seed, levels, this);

                            newChunk.GetComponent<Chunk>().SpeedMapGen();

                            mapChunks.Add(newChunk.GetComponent<Chunk>());
                        }
                        else
                        {
                            chunksX_Z[xIndex, -zIndex].GetComponent<Chunk>().ShowChild();
                        }
                    }
                }
                else if (xIndex < 0)
                {
                    if (zIndex >= 0)
                    {
                        if (!chunks_XZ[-xIndex, zIndex])
                        {
                            GameObject newChunk = Instantiate(chunkPrefab, this.transform);
                            newChunk.transform.position = new Vector3(xIndex * width, 0, zIndex * length);
                            chunks_XZ[-xIndex, zIndex] = newChunk;

                            newChunk.GetComponent<Chunk>().Set(width, length, scale, xIndex, zIndex, seed, levels, this);

                            newChunk.GetComponent<Chunk>().SpeedMapGen();

                            mapChunks.Add(newChunk.GetComponent<Chunk>());
                        }
                        else
                        {
                            chunks_XZ[-xIndex, zIndex].GetComponent<Chunk>().ShowChild();
                        }
                    }
                    else if (zIndex < 0 )
                    {
                        if (!chunks_X_Z[-xIndex, -zIndex])
                        {
                            GameObject newChunk = Instantiate(chunkPrefab, this.transform);
                            newChunk.transform.position = new Vector3(xIndex * width, 0, zIndex * length);
                            chunks_X_Z[-xIndex, -zIndex] = newChunk;

                            newChunk.GetComponent<Chunk>().Set(width, length, scale, xIndex, zIndex, seed, levels, this);

                            newChunk.GetComponent<Chunk>().SpeedMapGen();

                            mapChunks.Add(newChunk.GetComponent<Chunk>());
                        }
                        else
                        {
                            chunks_X_Z[-xIndex, -zIndex].GetComponent<Chunk>().ShowChild();
                        }
                    }
                }

            }
        }
    }

    int CheckChunkInList(int x, int z)
    {
        int count = 0;
        foreach (Chunk entity in mapChunks)
        {
            if(entity.xPos == x && entity.zPos == z)
            {
                return count;
            }
            count++;
        }
        return -1;
    }

    void DeleteMap()
    {
        List<GameObject> toDelete = new List<GameObject>();
        foreach (Chunk entity in mapChunks)
        {
            if (entity.active)
            {
                if (!(entity.xPos - playerPos.x >= -viewDistance && entity.xPos - playerPos.x <= viewDistance) ||
                !(entity.zPos - playerPos.z >= -viewDistance && entity.zPos - playerPos.z <= viewDistance))
                {
                    entity.DontShowChild();
                    //toDelete.Add(entity.gameObject);
                }
            }
            
        }

        foreach (GameObject entity in toDelete)
        {
            mapChunks.Remove(entity.GetComponent<Chunk>());
            Destroy(entity);
        }
    }
}
