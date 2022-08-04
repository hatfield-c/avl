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

    [SerializeField]
    protected SteeringSubsystem steering = null;

    [SerializeField]
    protected BrakeController brakes = null;

    [Header("Parameters")]

    [SerializeField]
    protected float turnRate = 3f;

    protected float[] acceleratorCommand = new float[1];
    protected float[] rotateCommand = new float[1];
    protected float[] brakeCommand = new float[1];

    void FixedUpdate()
    {
        if (Input.GetKey("w")) {
            this.acceleratorCommand[0] = 100f;
            this.accelerator.CommandDevice(this.acceleratorCommand);
        }

        if (Input.GetKey("s")) {
            this.acceleratorCommand[0] = 0f;
            this.accelerator.CommandDevice(this.acceleratorCommand);
        }

        if (Input.GetKey("a") || Input.GetKey("d")) {
            if (Input.GetKey("a")) {
                this.rotateCommand[0] = -this.turnRate;
            } else {
                this.rotateCommand[0] = this.turnRate;
            }

            this.steering.CommandDevice(this.rotateCommand);
        } else {
            this.rotateCommand[0] = 0f;
            this.steering.CommandDevice(this.rotateCommand);
        }

        if (Input.GetKey("space")) {
            this.brakeCommand[0] = Time.fixedDeltaTime;
            this.brakes.CommandDevice(this.brakeCommand);
        }
    }
}
