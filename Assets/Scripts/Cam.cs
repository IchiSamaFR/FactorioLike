﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public GameObject Copper;
    public PlayerMovement playerMovement;
    GameObject hitObject;

    string interactionKey;
    string turnKey;


    [Header("Build")]
    Builds build;
    GameObject preshowBuild;

    sbyte direction = 0;

    // Start is called before the first frame update
    void Start()
    {
        interactionKey = playerMovement.interactionKey;
        turnKey = playerMovement.turnKey;
        build = Builds.instance;
    }

    // Update is called once per frame
    void Update()
    {
        Check();

        if (Input.GetKeyDown(turnKey))
        {
            direction += 1;
            if(direction == 4)
            {
                direction = 0;
            }
        }
    }

    void Check()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 6))
        {
            GameObject hited = hit.transform.gameObject;

            //  If is hitted
            if (hited.tag == "GroundTile" || hitObject != null)
            {
                HitMarker.instance.Change(new Color(180, 0, 0));

                //  Show the preshow build if there is a build selected
                if (build.selected != null)
                {
                    Destroy(preshowBuild);
                    preshowBuild = Instantiate(build.selected.shadowPrefab);
                    preshowBuild.transform.position = hit.transform.position + new Vector3(0, 1, 0);
                    preshowBuild.transform.GetChild(0).rotation = Quaternion.Euler(0, direction * 90, 0);
                } 
                else if (preshowBuild)
                {
                    Destroy(preshowBuild);
                }
            } 
            else
            {
                HitMarker.instance.Change(new Color(255, 255, 255));
            }

            //  If there is a shadow then you could place the build
            if (preshowBuild)
            {
                if (Input.GetMouseButton(1) && hited.GetComponent<Block>())
                {
                    Block hitedScript = hited.GetComponent<Block>();
                    if(!hitedScript.chunk.buildedBlocks[hitedScript.posX, hitedScript.posZ])
                    {
                        hitedScript.chunk.AddBuild(hitedScript.posX, hitedScript.posZ, build.selected.prefab, direction);
                    }
                }
            }

            if (Input.GetMouseButton(0) && hited.GetComponent<Block>())
            {
                Block hitedScript = hited.GetComponent<Block>();
                hitedScript.chunk.DestroyBuild(hitedScript.posX, hitedScript.posZ);
            }

            if (Input.GetKey(interactionKey))
            {
                if (hited.GetComponent<Block>())
                {
                    Block hitedScript = hited.GetComponent<Block>();
                    GameObject buildedBlock;
                    if (buildedBlock = hitedScript.chunk.buildedBlocks[hitedScript.posX, hitedScript.posZ])
                    {
                        if (buildedBlock.GetComponent<Conveyor>())
                        {
                            buildedBlock.GetComponent<Conveyor>().GetOre(Copper);
                        }
                    }
                }
            }
        }
        else
        {
            HitMarker.instance.Change(new Color(255, 255, 255));
            Destroy(preshowBuild);
        }
    }
}
