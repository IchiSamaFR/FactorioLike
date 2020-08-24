using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class NoiseMap
{
    public static List<BlocToGen> Create(int width, int length, float scale, int xPos, int zPos, int seed, List<MapGenLevel> levels)
    {
        List<BlocToGen> mapTiles = new List<BlocToGen>();
        string[,] mapTilesId = new string[16, 16];

        float z = 0.0f;
        float newXPos = xPos * scale;
        float newZPos = zPos * scale;


        //  Gen blank page
        while (z < length)
        {
            float x = 0.0f;
            while (x < width)
            {
                mapTilesId[(int)x, (int)z] = levels[0].id;

                x++;
            }
            z++;
        }

        //  For each ore level, add it to the layer
        int i = 0;
        foreach (MapGenLevel level in levels)
        {
            if(i != 0)
            {
                string[,] gen = GenLevel(width, length, scale, xPos, zPos, seed, 1 - level.level, level.id);

                //  Already declared
                z = 0.0f;
                while (z < length)
                {
                    float x = 0.0f;
                    while (x < width)
                    {
                        if(gen[(int)x, (int)z] != null)
                        {
                            mapTilesId[(int)x, (int)z] = level.id;
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

    //  Generation of the layer level
    static string[,] GenLevel(int width, int length, float scale, int xPos, int zPos, int seed, float level, string id)
    {
        seed = (int)(seed * level * (id.Length * 2f));

        string[,] mapTilesId = new string[16, 16];

        float z = 0.0f;
        float newXPos = xPos * scale;
        float newZPos = zPos * scale;

        while (z < length)
        {
            float x = 0.0f;
            while (x < width)
            {

                //  Perlin Noise gen and add it if it's bigger than the level requiered
                float xCoord = seed + newXPos + x / width * scale;
                float zCoord = seed + newZPos + z / length * scale;
                float yCoord = Mathf.PerlinNoise(xCoord, zCoord);

                if (yCoord > level)
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
