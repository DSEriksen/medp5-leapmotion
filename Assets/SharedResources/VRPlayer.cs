using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayer : MonoBehaviour {
    private CharacterController controller;
    public Transform cameraTransform;
    private CsvLogger<Vector4> collisionLogger = new CsvLogger<Vector4> ("collisions", "x,y,z,t", vec => vec.x + "," + vec.y + "," + vec.z + "," + vec.w);
    private float timer;
    private bool move, rotate;
    private float speed;
    private float turnSpeed;
    private Vector3 moveDirection = Vector3.zero;

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

            moveDirection = new Vector3(0, 0, speed);
            moveDirection = cameraTransform.TransformDirection(moveDirection);
            controller.SimpleMove (moveDirection);
        }

        if (rotate) {
            transform.Rotate (0, turnSpeed * Time.deltaTime, 0);
            Debug.Log ("rotating");
        }

    }

    public void moveController (string caseswitch) {
        switch (caseswitch) {
            case "forward":
                speed = 5f;
                move = true;
                break;
            case "backward":
                speed = -2f;
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
        Debug.Log ("got " + caseswitch + " command");
        switch (caseswitch) {
            case "left":
                turnSpeed = -60f;
                rotate = true;
                break;
            case "right":
                turnSpeed = 60f;
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