using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GpsSensor : AbstractDevice
{
    [SerializeField]
    protected Transform target = null;

    [SerializeField]
    protected Transform spinner = null;

    [SerializeField]
    protected float spinRate = 1f;

    override public byte[] CommandDevice(byte[] command) {
        byte[] latitudeData;
        byte[] longitudeData;
        byte[] sensorData = new byte[8];

        latitudeData = System.BitConverter.GetBytes(this.target.position.x);
        longitudeData = System.BitConverter.GetBytes(this.target.position.z);

        for(int i = 0; i < latitudeData.Length; i++) {
            sensorData[i] = latitudeData[i];
            sensorData[i + 4] = longitudeData[i];
        }

        return sensorData;
    }

    void FixedUpdate() {
        this.spinner.eulerAngles += Vector3.up * this.spinRate;
    }
}
