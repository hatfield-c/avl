using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgoVehicle : MonoBehaviour
{
    public float speed = 1f;
    public float turnSpeed = 1f;

    void Update()
    {
        if (Input.GetKey(KeyCode.W)) {
            this.transform.Translate(Vector3.forward * this.speed);
        }

        if (Input.GetKey(KeyCode.A)) {
            this.transform.Rotate(new Vector3(0, -this.turnSpeed, 0));
        }

        if (Input.GetKey(KeyCode.D)) {
            this.transform.Rotate(new Vector3(0, this.turnSpeed, 0));
        }

        if (Input.GetKey(KeyCode.S)) {
            this.transform.Translate(Vector3.forward * -this.speed);
        }
    }
}
