using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionSensor : AbstractSensor
{
    [SerializeField]
    protected Transform egoVehicle = null;

    [SerializeField]
    protected Transform spinner = null;

    [SerializeField]
    protected Vector3 targetDirection = Vector3.forward;

    override public byte[] ReadSensor(byte option) {
        byte[] sensorData;

        float angle = Vector3.SignedAngle(this.targetDirection, this.egoVehicle.transform.forward, Vector3.up);
        sensorData = System.BitConverter.GetBytes(angle);

        return sensorData;
    }

    void FixedUpdate() {
        this.spinner.LookAt(this.spinner.position + this.targetDirection.normalized);
    }
}
