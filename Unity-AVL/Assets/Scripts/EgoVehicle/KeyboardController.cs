using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    protected DeviceRegistry devices = null;

    [Header("Parameters")]

    [SerializeField]
    protected float turnRate = 3f;

    void FixedUpdate()
    {
        if (Input.GetKey("w")) {
            this.devices.speedControl[0] = 1f;
            this.devices.speedControl[1] = 10f;
        }

        if (Input.GetKey("s")) {
            this.devices.speedControl[0] = 1f;
            this.devices.speedControl[1] = -10f;
        }

        if (Input.GetKey("x")) {
            this.devices.speedControl[0] = 1f;
            this.devices.speedControl[1] = 0f;
        }

        if (Input.GetKey("a") || Input.GetKey("d")) {
            this.devices.steeringControl[0] = 1f;

            if (Input.GetKey("a")) {
                this.devices.steeringControl[1] = -this.turnRate;
            } else {
                this.devices.steeringControl[1] = this.turnRate;
            }
        } else {
            this.devices.steeringControl[0] = 1f;
            this.devices.steeringControl[1] = 0f;
        }

        if (Input.GetKey("space")) {
            this.devices.brakeControl[0] = 1f;
            this.devices.brakeControl[1] = Time.fixedDeltaTime;
        }
    }
}
