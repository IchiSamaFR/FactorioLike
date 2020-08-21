using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseMap
{
    public static List<BlocToGen> Create(int width, int length, float scale, int xPos, int zPos, int seed, List<MapGenLevel> levels)
    {
        List<BlocToGen> mapTiles = new List<BlocToGen>();
        int[,] mapTilesId = new int[16, 16];

        float z = 0.0f;
        float newXPos = xPos * scale;
        float newZPos = zPos * scale;


        //  Gen blank page
        while (z < length)
        {
            float x = 0.0f;
            while (x < width)
            {
                mapTilesId[(int)x, (int)z] = 0;

                x++;
            }
            z++;
        }

        int i = 0;
        foreach (MapGenLevel level in levels)
        {
            if(i != 0)
            {
                int[,] gen = GenLevel(width, length, scale, xPos, zPos, seed, 1 - level.level, i);

                //  Already declared
                z = 0.0f;
                while (z < length)
                {
                    float x = 0.0f;
                    while (x < width)
                    {
                        if(gen[(int)x, (int)z] > 0)
                        {
                            mapTilesId[(int)x, (int)z] = i;
                        }
                        x++;
                    }
                    z++;
                }
            }
            i++;
        }

        z = 0.0f;
        //  Gen blank page
        while (z < length)
        {
            float x = 0.0f;
            while (x < width)
            {
                mapTiles.Add(new BlocToGen(mapTilesId[(int)x, (int)z], (int)x, (int)z));

                x++;
            }
            z++;
        }

        return mapTiles;
    }

    static int[,] GenLevel(int width, int length, float scale, int xPos, int zPos, int seed, float level, int id)
    {
        seed = (int)(seed * level * (id * 2f));

        int[,] mapTilesId = new int[16, 16];

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

                if(yCoord > level)
                {
                    mapTilesId[(int)x, (int)z] = id;
                }
                x++;
            }
            z++;
        }

        return mapTilesId;
    }
}
