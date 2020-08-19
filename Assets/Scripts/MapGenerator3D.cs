﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator3D : MonoBehaviour
{
    public GameObject wall;
    public GameObject spawnPoint;


    private string[][,] MapWalls;


    // Start is called before the first frame update
    void Start()
    {
        MapWalls = new string[1][,];
        MapWalls[0] = new string[,]{
            {"D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D"," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," "," ","D" },
            {"D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D","D" }
        };


        Load();
    }


    void Load()
    {
        int height = MapWalls[0].GetLength(0);
        int width = MapWalls[0].GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //if (MapWalls[0][y, x] == "D")
                //{
                    GameObject Wall = Instantiate(wall, this.transform);

                    float posX = x - width / 2 + 0f;
                    float posY = -y + height / 2 + 0f;

                    Wall.transform.position = new Vector3(posX, 0 , posY);

                    setWall(Wall, x, y, MapWalls[0]);
                //}
            }
        }
    }



    void setWall(GameObject Obj, int x, int y, string[,] Map)
    {

    }
}
