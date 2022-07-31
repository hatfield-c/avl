using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionSensor : AbstractDevice
{
    [SerializeField]
    protected Transform egoVehicle = null;

    [SerializeField]
    protected Transform spinner = null;

    [SerializeField]
    protected Vector3 targetDirection = Vector3.forward;

    override public byte[] CommandDevice(byte[] command) {
        byte[] sensorData;

        float angle = Vector3.SignedAngle(this.targetDirection, this.egoVehicle.transform.forward, Vector3.up);
        sensorData = System.BitConverter.GetBytes(angle);

        return sensorData;
    }

    void FixedUpdate() {
        this.spinner.LookAt(this.spinner.position + this.targetDirection.normalized);
        this.spinner.eulerAngles = new Vector3(0, this.spinner.eulerAngles.y, 0);
    }
}
