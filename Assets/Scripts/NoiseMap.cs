using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseMap
{
    public static List<BlocToGen> Create(int width, int length, float scale, int xPos, int zPos, int seed, List<MapGenLevel> levels)
    {
        List<BlocToGen> mapTiles = new List<BlocToGen>();

        float z = 0.0f;
        float newXPos = xPos * scale;
        float newZPos = zPos * scale;

        while (z < length)
        {
            float x = 0.0f;
            while (x < width)
            {
                float xCoord = seed + newXPos + x / width * scale;
                float zCoord = seed + newZPos + z / length * scale;
                float yCoord = Mathf.PerlinNoise(xCoord, zCoord);
                int id = 0;

                float count = 0;
                foreach (MapGenLevel map in levels)
                {
                    count += map.level;
                    if (yCoord <= count)
                    {
                        break;
                    }
                    id++;
                }

                mapTiles.Add(new BlocToGen(id, (int)x, (int)z));
                x++;
            }
            z++;
        }

        return mapTiles;
    }
}
