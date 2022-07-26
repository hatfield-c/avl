using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarArraySensor : AbstractDevice
{
    [SerializeField]
    protected float maxDistance = 10;

    [SerializeField]
    protected List<Transform> lidarArray = new List<Transform>();

    override public void CommandDevice(byte[] command, byte[] memory) {
        RaycastHit rayData;

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
                memory[(i * sensorData.Length) + j] = sensorData[j];
            }
        }
    }

}
