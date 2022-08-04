using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarArraySensor : AbstractDevice
{
    [SerializeField]
    protected float maxDistance = 10;

    [SerializeField]
    protected List<Transform> lidarArray = new List<Transform>();

    override public void ReadDevice(float[] memory, int[,,] empty) {
        RaycastHit rayData;
        Transform lidar;

        for (int i = 0; i < this.lidarArray.Count; i++) {
            lidar = this.lidarArray[i];

            bool isHit = Physics.Raycast(lidar.position, lidar.up, out rayData, this.maxDistance);

            if (isHit) {
                memory[i] = rayData.distance;
            } else {
                memory[i] = this.maxDistance;
            }
        }
    }

    public override void CommandDevice(float[] empty) { }

    public int GetLidarCount() {
        return this.lidarArray.Count;
    }

}
