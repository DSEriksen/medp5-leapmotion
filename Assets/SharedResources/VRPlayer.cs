using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayer : MonoBehaviour {
    private CharacterController controller;
    private CsvLogger<Vector4> collisionLogger = new CsvLogger<Vector4> ("collisions", "x,y,z,t", vec => vec.x + "," + vec.y + "," + vec.z + "," + vec.w);
    private float timer;
    private bool move, rotate;
    private float speed;
    private float turnSpeed;


    void Start () {
        controller = GetComponent<CharacterController> ();
        timer = 2.0f;
        speed = 5f;
        turnSpeed = 90f;
    }

    void OnApplicationQuit () {
        collisionLogger.PrintAndSave ();
    }

    // Update is called once per frame
    void Update () {
        timer -= Time.deltaTime;

        if (timer < 0) {
            if ((controller.collisionFlags & CollisionFlags.Sides) != 0) {
                timer = 2.0f;
                Debug.Log ("Collide with wall.");

                Vector4 point = new Vector4 (
                    transform.position.x,
                    transform.position.y,
                    transform.position.z,
                    Time.time
                );

                collisionLogger.Log (point);
            }
        }

        if (move) {
            Vector3 velocity = transform.forward * speed;
            controller.SimpleMove (velocity);
        }
        if (rotate) {
            transform.Rotate (0, turnSpeed * Time.deltaTime, 0);
        }


    }

    public void moveController (string caseswitch) {
        switch(caseswitch){
            case "forward":
                speed = 5f;
                move = true;
                break;
            case "backward":
                speed = -5f;
                move = true;
                break;
            case "stop":
                move = false;
                break;
            default:
                break;
            
        }
    }

    public void rotateController (string caseswitch) {
        switch(caseswitch){
            case "left":
                turnSpeed = -90f;
                rotate = true;
                break;
            case "right":
                turnSpeed = 90f;
                rotate = true;
                break;
            case "stop":
                rotate = false;
                break;
            default: 
                break;
        }
    }
}