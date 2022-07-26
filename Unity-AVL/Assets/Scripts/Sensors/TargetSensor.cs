using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSensor : AbstractDevice
{
    [SerializeField]
    protected Transform egoVehicle = null;

    [SerializeField]
    protected Transform spinner = null;

    [SerializeField]
    protected Transform target = null;

    override public void CommandDevice(byte[] command, byte[] memory) {
        byte[] sensorData;

        if(this.target == null) {
            for(int i = 0; i < 4; i++) {
                memory[i] = 0;
            }
        }

        Vector3 direction = new Vector3(
            this.target.position.x - this.egoVehicle.transform.position.x,
            0,
            this.target.position.z - this.egoVehicle.transform.position.z
        );

        float angle = Vector3.SignedAngle(direction.normalized, this.egoVehicle.transform.forward, Vector3.up);
        sensorData = System.BitConverter.GetBytes(angle);

        for(int i = 0; i < sensorData.Length; i++) {
            memory[i] = sensorData[i];
        }
    }

    void FixedUpdate() {
        if(this.target == null) {
            return;
        }

        this.spinner.LookAt(this.target.position);
        this.spinner.eulerAngles = new Vector3(0, this.spinner.eulerAngles.y, 0);
    }
}
