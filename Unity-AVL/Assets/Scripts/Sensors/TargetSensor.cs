using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSensor : AbstractSensor
{
    [SerializeField]
    protected Transform egoVehicle = null;

    [SerializeField]
    protected Transform spinner = null;

    [SerializeField]
    protected Transform target = null;

    override public byte[] ReadSensor(byte option) {
        byte[] sensorData;

        if(this.target == null) {
            return new byte[4];
        }

        Vector3 direction = new Vector3(
            this.target.position.x - this.egoVehicle.transform.position.x,
            0,
            this.target.position.z - this.egoVehicle.transform.position.z
        );

        float angle = Vector3.SignedAngle(direction.normalized, this.egoVehicle.transform.forward, Vector3.up);
        sensorData = System.BitConverter.GetBytes(angle);

        return sensorData;
    }

    void FixedUpdate() {
        if(this.target == null) {
            return;
        }

        this.spinner.LookAt(this.target.position);
        this.spinner.eulerAngles = new Vector3(0, this.spinner.eulerAngles.y, 0);
    }
}
