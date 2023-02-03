using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarVisualizer : MonoBehaviour
{
    [SerializeField]
    protected GameObject pixelPrefab = null;

    protected LidarArraySensor lidarSensor = null;
    protected bool showRays = false;

    public void Init(
        LidarArraySensor lidarSensor, 
        bool showRays
       ) {
        this.lidarSensor = lidarSensor;
        this.showRays = showRays;
        
        if (this.showRays) {
            this.lidarSensor.CreateRays();
        }
    }
}
