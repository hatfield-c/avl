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

    override public void CommandDevice(byte[] command, byte[] memory) {
        byte[] latitudeData;
        byte[] longitudeData;

        latitudeData = System.BitConverter.GetBytes(this.target.position.x);
        longitudeData = System.BitConverter.GetBytes(this.target.position.z);

        for(int i = 0; i < latitudeData.Length; i++) {
            memory[i] = latitudeData[i];
            memory[i + latitudeData.Length] = longitudeData[i];
        }
    }

    void FixedUpdate() {
        this.spinner.eulerAngles += Vector3.up * this.spinRate;
    }
}
