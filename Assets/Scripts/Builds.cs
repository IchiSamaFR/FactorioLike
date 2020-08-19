using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builds : MonoBehaviour
{
    public static Builds instance;

    public List<Build> buildsAvailable = new List<Build>();
    public Build selected = null;

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
        else if (Input.GetKeyDown("escape"))
        {
            Unselect();
        }
    }

    void Select(int i)
    {
        selected = buildsAvailable[i];
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