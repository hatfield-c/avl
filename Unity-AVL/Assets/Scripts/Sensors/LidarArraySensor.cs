using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarArraySensor : AbstractSensor
{
    [SerializeField]
    protected float maxDistance = 10;

    [SerializeField]
    protected List<Transform> lidarArray = new List<Transform>();

    override public byte[] ReadSensor(byte option) {
        RaycastHit rayData;
        byte[] sensorData;

        int sensorIndex = (int)option;

        if(sensorIndex < 0 || sensorIndex >= this.lidarArray.Count) {
            Debug.LogError($"Error: Tried to read data from lidar sensor at index '{sensorIndex}', but index must be between 0 and {this.lidarArray.Count - 1}. An empty byte array will be returned.");

            return new byte[4];
        }

        Transform lidar = this.lidarArray[sensorIndex];

        bool isHit = Physics.Raycast(lidar.position, lidar.up, out rayData, this.maxDistance);

        if (isHit) {
            sensorData = System.BitConverter.GetBytes(rayData.distance);
        } else {
            sensorData = System.BitConverter.GetBytes(this.maxDistance);
        }

        return sensorData;
    }

}
