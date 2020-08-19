using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10;

    public string forwardKey = "z";
    public string leftKey = "q";
    public string RightKey = "d";
    public string backKey = "s";

    Vector3 toGo = new Vector3();


    [Header("Rotation")]
    public GameObject cam;
    public GameObject body;
    public float mouseSensitivity = 100;
    Vector3 toLook = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        toGo = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Movement();
    }
    void Movement()
    {
        Vector3 forward = cam.transform.forward;
        forward.y = 0;

        Vector3 right = cam.transform.right;
        right.y = 0;

        toGo = this.transform.position;
        
        if (Input.GetKey(forwardKey))
        {
            toGo += forward * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(backKey))
        {
            toGo += -forward * Time.deltaTime * (moveSpeed * 0.8f);
        }
        if (Input.GetKey(RightKey))
        {
            toGo += right * Time.deltaTime * (moveSpeed * 0.8f);
        }
        if (Input.GetKey(leftKey))
        {
            toGo += -right * Time.deltaTime * (moveSpeed * 0.8f);
        }

        //  Set de la position y pour les mouvements de haut en bas.
        toGo.y = this.transform.position.y;

        //this.transform.position = Vector3.Lerp(this.transform.position, toGo, Time.deltaTime * 8);
        this.transform.position = toGo;
    }
    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        toLook.y += mouseX;
        toLook.x -= mouseY;

        toLook.x = Mathf.Clamp(toLook.x, -60, 60);

        //  Rotation du personnage de gauche à droite
        Quaternion target = Quaternion.Euler(0, toLook.y, 0);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, target, Time.deltaTime * 8);

        //  S'il regarde au dessus de 10, alors on bloque la rotation du personnage pour ne pas qu'il soit couché
        if (toLook.x < -10)
        {
            target = Quaternion.Euler(-10, 0, 0);
            body.transform.rotation = Quaternion.Slerp(body.transform.rotation, target, Time.deltaTime * 8);
            target = Quaternion.Euler(body.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            body.transform.rotation = target;

            target = Quaternion.Euler(toLook.x, 0, 0);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, target, Time.deltaTime * 8);
            target = Quaternion.Euler(cam.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            cam.transform.rotation = target;
        }
        //  S'il regarde en dessous de 20, alors on bloque la rotation du personnage pour ne pas qu'il soit couché
        else if (toLook.x > 20)
        {
            target = Quaternion.Euler(20, 0, 0);
            body.transform.rotation = Quaternion.Slerp(body.transform.rotation, target, Time.deltaTime * 8);
            target = Quaternion.Euler(body.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            body.transform.rotation = target;

            target = Quaternion.Euler(toLook.x, 0, 0);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, target, Time.deltaTime * 8);
            target = Quaternion.Euler(cam.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            cam.transform.rotation = target;
        }
        //  Sinon le laisser se baisser ou se monter
        else
        {
            target = Quaternion.Euler(toLook.x, 0, 0);
            body.transform.rotation = Quaternion.Slerp(body.transform.rotation, target, Time.deltaTime * 8);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, target, Time.deltaTime * 8);

            target = Quaternion.Euler(body.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            body.transform.rotation = target;
            target = Quaternion.Euler(cam.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            cam.transform.rotation = target;
        }
    }

}
