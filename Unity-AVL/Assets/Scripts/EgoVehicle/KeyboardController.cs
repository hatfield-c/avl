using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    protected PhysicsBody body = null;

    [SerializeField]
    protected Accelerator accelerator = null;

    [Header("Parameters")]
    [SerializeField]
    protected float gasPower = 0.1f;

    [SerializeField]
    protected float brakePower = 0.9f;

    [SerializeField]
    protected float turnRate = 3f;

    protected float[] accelCommand = new float[1];

    void FixedUpdate()
    {
        if (Input.GetKey("w")) {
            this.accelCommand[0] = 100f;
            this.accelerator.CommandDevice(this.accelCommand);
        }

        if (Input.GetKey("s")) {
            this.accelCommand[0] = 0f;
            this.accelerator.CommandDevice(this.accelCommand);
        }

        if (Input.GetKey("a")) {
            this.body.Rotate(-this.turnRate);
        }

        if (Input.GetKey("d")) {
            this.body.Rotate(this.turnRate);
        }

        if (Input.GetKey("space")) {
            this.body.ApplyDrag(this.brakePower);
        }
    }
}
