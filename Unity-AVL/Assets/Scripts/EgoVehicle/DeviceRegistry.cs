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
    public float[] microphone;

    [System.NonSerialized]
    public float[] speedometer;

    [System.NonSerialized]
    public float[] speedControl;

    [System.NonSerialized]
    public float[] brakeControl;

    [System.NonSerialized]
    public float[] steeringControl;

    [System.NonSerialized]
    public float[] transmitterControl;

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

    [SerializeField]
    protected MicrophoneSensor microphoneSensor = null;

    [Header("Actuators")]
    [SerializeField]
    protected Accelerator accelerator = null;

    [SerializeField]
    protected BrakeController brakeController = null;

    [SerializeField]
    protected SteeringSubsystem steeringSystem = null;

    [SerializeField]
    protected Transmitter transmitter = null;

    void Start()
    {
        this.gps = new float[2];
        this.lidar = new float[this.lidarSensor.GetLidarCount()];
        this.pixels = new int[this.cameraSensor.GetPixelHeight(), this.cameraSensor.GetPixelWidth(), 3];
        this.compass = new float[1];
        this.targetAlignment = new float[1];
        this.microphone = new float[1];
        this.speedometer = new float[1];
        this.speedControl = new float[2];
        this.steeringControl = new float[2];
        this.brakeControl = new float[2];
        this.transmitterControl = new float[2];
        this.memory = new byte[64, 4];
    }

    public void ReadSensors() {
        if (this.gpsSensor != null && this.gpsSensor.enabled) {
            this.gpsSensor.ReadDevice(this.gps, null);
        }

        if (this.lidarSensor != null && this.lidarSensor.enabled) {
            this.lidarSensor.ReadDevice(this.lidar, null);
        }

        if (this.cameraSensor != null && this.cameraSensor.enabled) {
            this.cameraSensor.ReadDevice(null, this.pixels);
        }

        if (this.directionFinder != null && this.directionFinder.enabled) {
            this.directionFinder.ReadDevice(this.compass, null);
        }

        if (this.targetFinder != null && this.targetFinder.enabled) {
            this.targetFinder.ReadDevice(this.targetAlignment, null);
        }

        if(this.microphoneSensor != null && this.microphoneSensor.enabled) {
            this.microphoneSensor.ReadDevice(this.microphone, null);
        }

        if(this.accelerator != null && this.accelerator.enabled) {
            this.accelerator.ReadDevice(this.speedometer, null);
        }
    }

    public void CommandActuators() {
        if (this.accelerator != null && this.accelerator.enabled && this.speedControl[0] != 0) {
            this.accelerator.CommandDevice(this.speedControl);
            this.speedControl[0] = 0;
        }

        if (this.steeringSystem != null && this.steeringSystem.enabled && this.steeringControl[0] != 0) {
            this.steeringSystem.CommandDevice(this.steeringControl);
            this.steeringControl[0] = 0;
        }

        if (this.brakeController != null && this.brakeController.enabled && this.brakeControl[0] != 0) {
            this.brakeController.CommandDevice(this.brakeControl);
            this.brakeControl[0] = 0;
        }

        if(this.transmitter != null && this.transmitter.enabled && this.transmitterControl[0] != 0) {
            this.transmitter.CommandDevice(this.transmitterControl);
            this.transmitterControl[0] = 0;
        }
    }

}
