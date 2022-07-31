using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarArraySensor : AbstractDevice
{
    [SerializeField]
    protected float maxDistance = 10;

    [SerializeField]
    protected List<Transform> lidarArray = new List<Transform>();

    override public byte[] CommandDevice(byte[] command) {
        RaycastHit rayData;
        byte[] lidarData = new byte[4 * this.lidarArray.Count];

        for (int i = 0; i < this.lidarArray.Count; i++) {
            Transform lidar = this.lidarArray[i];
            byte[] sensorData;

            bool isHit = Physics.Raycast(lidar.position, lidar.up, out rayData, this.maxDistance);

            if (isHit) {
                sensorData = System.BitConverter.GetBytes(rayData.distance);
            } else {
                sensorData = System.BitConverter.GetBytes(this.maxDistance);
            }

            for(int j = 0; j < sensorData.Length; j++) {
                lidarData[(i * sensorData.Length) + j] = sensorData[j];
            }
        }

        return lidarData;
    }

}
