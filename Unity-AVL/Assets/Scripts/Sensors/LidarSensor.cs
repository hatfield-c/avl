using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarSensor : AbstractSensor
{

    override public byte[] ReadSensor() {
        byte[] sensorData = new byte[] { 1, 0, 0, 1 };

        return sensorData;
    }

}
