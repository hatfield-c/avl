using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    protected CameraSensor cameraSensor = null;

    [SerializeField]
    protected LidarArraySensor lidarSensor = null;

    [SerializeField]
    protected CameraVisualizer cameraVisualizer = null;

    [SerializeField]
    protected LidarVisualizer lidarVisualizer = null;

    [SerializeField]
    protected BoatController boatController = null;

    [SerializeField]
    protected CarController carController = null;

    [Header("Camera Debugging")]

    [SerializeField]
    protected float pixelWidth = 1f;

    [SerializeField]
    protected float pixelDistance = 8f;

    [SerializeField]
    protected bool showCameraRays = false;

    [SerializeField]
    protected float cameraRayLength = 5f;

    [Header("Lidar Debugging")]

    [SerializeField]
    protected bool showLidarRays = false;

    void Start()
    {
        this.cameraVisualizer.Init(this.cameraSensor, this.pixelDistance, this.pixelWidth, this.showCameraRays, this.cameraRayLength);
        this.lidarVisualizer.Init(this.lidarSensor, this.showLidarRays);

    }

    public void ActivateBoat() {
        if(this.boatController == null || !Application.isPlaying) {
            return;
        }

        this.boatController.TriggerBoat();
    }

    public void RandomEmptySpace() {
        if(this.carController == null || !Application.isPlaying) {
            return;
        }

        this.carController.SetAction(CarController.SpawnAction.RandomSpace);
    }

    public void RandomFirstRow() {
        if (this.carController == null || !Application.isPlaying) {
            return;
        }

        this.carController.SetAction(CarController.SpawnAction.RandomFirstRow);
    }
}
