using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GpsSensor : AbstractSensor
{
    
    [SerializeField]
    protected Transform target = null;

    [SerializeField]
    protected Transform spinner = null;

    [SerializeField]
    protected float spinRate = 1f;

    override public byte[] ReadSensor(byte option) {
        byte[] sensorData;

        int index = (int)option;

        if(index != 0 && index != 1) {
            Debug.LogError($"Error: Tried to read data from GPS sensor using index '{index}', but index must be either 0 for Latitude or 1 for Longitude. An empty byte array will be returned.");

            return new byte[4];
        }

        if (index == 0) {
            sensorData = System.BitConverter.GetBytes(this.target.position.x);
        } else {
            sensorData = System.BitConverter.GetBytes(this.target.position.z);
        }

        return sensorData;
    }

    void FixedUpdate() {
        this.spinner.eulerAngles += Vector3.up * this.spinRate;
    }

}
