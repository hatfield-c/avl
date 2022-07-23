using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    [Header("Parameters")]

    [SerializeField]
    protected float maxSpeed = 0.33f;

    [SerializeField]
    protected float gasPower = 0.1f;

    [SerializeField]
    protected float brakePower = 0.9f;

    [SerializeField]
    protected float turnRate = 3f;

    [SerializeField]
    protected float drag = 0.97f;

    protected float velocity = 0f;

    void FixedUpdate()
    {
        if (Input.GetKey("w") && this.velocity < this.maxSpeed) {
            this.velocity = this.velocity + this.gasPower;

            if(this.velocity > this.maxSpeed) {
                this.velocity = this.maxSpeed;
            }
        }

        if (Input.GetKey("a")) {
            this.transform.eulerAngles = this.transform.eulerAngles + (Vector3.up * this.turnRate);
        }

        if (Input.GetKey("d")) {
            this.transform.eulerAngles = this.transform.eulerAngles + (Vector3.up * -this.turnRate);
        }

        if (Input.GetKey("space")) {
            this.velocity = this.velocity * this.brakePower;
        }

        this.transform.position = this.transform.position + (this.transform.forward * this.velocity);

        this.velocity = this.velocity * this.drag;
    }
}
