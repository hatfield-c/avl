using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarSensor : AbstractSensor
{
    [SerializeField]
    protected float maxDistance = 10;

    override public byte[] ReadSensor() {
        RaycastHit rayData;
        byte[] sensorData;

        bool isHit = Physics.Raycast(this.transform.position, this.transform.up, out rayData, this.maxDistance);

        if (isHit) {
            sensorData = System.BitConverter.GetBytes(rayData.distance);
        } else {
            sensorData = System.BitConverter.GetBytes(this.maxDistance);
        }

        return sensorData;
    }

}
