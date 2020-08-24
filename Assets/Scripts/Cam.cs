using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public GameObject Copper;
    public PlayerMovement playerMovement;

    string interactionKey;
    string turnKey;


    [Header("Build")]
    Builder builder;

    sbyte direction = 0;

    // Start is called before the first frame update
    void Start()
    {
        interactionKey = playerMovement.interactionKey;
        turnKey = playerMovement.turnKey;
        builder = Builder.instance;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("a"))
        {
            if (Time.timeScale == 0.1f)
            {
                Time.timeScale = 1f;
            } else
            {
                Time.timeScale = 0.1f;
            }
        }
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

            if (hited.GetComponent<Block>())
            {
                if (hited.tag == "GroundTile")
                {
                    HitMarker.instance.Change(new Color(180, 0, 0));

                    builder.Preshow(hited, direction);
                }
                else
                {
                    HitMarker.instance.Change(new Color(255, 255, 255));
                }

                if (Input.GetMouseButton(1))
                {
                    builder.Build(hited, direction);
                }
                else if (Input.GetMouseButton(0))
                {
                    builder.DestroyBuild(hited);
                }

                
                if (Input.GetKeyDown(interactionKey))
                {
                    if (hited.GetComponent<Block>())
                    {
                        Block hitedScript = hited.GetComponent<Block>();
                        GameObject buildedBlock;
                        if (buildedBlock = hitedScript.chunk.buildedBlocks[hitedScript.posX, hitedScript.posZ])
                        {
                            if (buildedBlock.GetComponent<Conveyor>())
                            {
                                buildedBlock.GetComponent<Conveyor>().GetItem(Copper);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            HitMarker.instance.Change(new Color(255, 255, 255));
            builder.StopPreshowBuild();
        }
    }
}
