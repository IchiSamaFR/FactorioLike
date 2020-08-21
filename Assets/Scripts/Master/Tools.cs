using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
    public static int Count(GameObject[] toCount)
    {
        int x = 0;
        foreach(GameObject count in toCount) 
        {
            if(count != null)
                x ++;
        }
        return x;
    }

    public static MapGenLevel GetMapGenLev(List<MapGenLevel> list, string id)
    {
        int x = 0;
        foreach (MapGenLevel obj in list)
        {
            if (obj.id == id)
            {
                return obj;
            }
        }
        return null;
    }
}
