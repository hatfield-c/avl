using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceRegistry : MonoBehaviour
{
    [System.NonSerialized]
    public float[] gps;

    [System.NonSerialized]
    public float[] lidar;

    [System.NonSerialized]
    public int[,,] pixels;

    [System.NonSerialized]
    public float[] compass;

    [System.NonSerialized]
    public float[] targetAlignment;

    [System.NonSerialized]
    public float[] speedometer;

    [System.NonSerialized]
    public float[] speedControl;

    [System.NonSerialized]
    public float[] brakeControl;

    [System.NonSerialized]
    public float[] steeringControl;

    [System.NonSerialized]
    public byte[,] memory;

    [Header("Sensors")]
    [SerializeField]
    protected GpsSensor gpsSensor = null;

    [SerializeField]
    protected LidarArraySensor lidarSensor = null;

    [SerializeField]
    protected CameraSensor cameraSensor = null;

    [SerializeField]
    protected DirectionSensor directionFinder = null;

    [SerializeField]
    protected TargetSensor targetFinder = null;

    [Header("Actuators")]
    [SerializeField]
    protected Accelerator accelerator = null;

    [SerializeField]
    protected BrakeController brakeController = null;

    [SerializeField]
    protected SteeringSubsystem steeringSystem = null;

    void Start()
    {
        this.gps = new float[2];
        this.lidar = new float[this.lidarSensor.GetLidarCount()];
        this.pixels = new int[this.cameraSensor.GetPixelHeight(), this.cameraSensor.GetPixelWidth(), 3];
        this.compass = new float[1];
        this.targetAlignment = new float[1];
        this.speedometer = new float[1];
        this.speedControl = new float[1];
        this.steeringControl = new float[1];
        this.brakeControl = new float[1];
        this.memory = new byte[64, 4];
    }

    public void ReadSensors() {
        if (this.gpsSensor != null) {
            this.gpsSensor.ReadDevice(this.gps, null);
        }

        if (this.lidarSensor != null) {
            this.lidarSensor.ReadDevice(this.lidar, null);
        }

        if (this.cameraSensor != null) {
            this.cameraSensor.ReadDevice(null, this.pixels);
        }

        if (this.directionFinder != null) {
            this.directionFinder.ReadDevice(this.compass, null);
        }

        if (this.targetFinder != null) {
            this.targetFinder.ReadDevice(this.targetAlignment, null);
        }

        if(this.accelerator != null) {
            this.accelerator.ReadDevice(this.speedometer, null);
        }
    }

    public void CommandActuators() {
        if (this.accelerator != null) {
            this.accelerator.CommandDevice(this.speedControl);
        }

        if (this.steeringSystem != null) {
            this.steeringSystem.CommandDevice(this.steeringControl);
        }

        if (this.brakeController != null) {
            this.brakeController.CommandDevice(this.brakeControl);
        }
    }

}
