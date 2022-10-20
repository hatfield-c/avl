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
    public float[] cameraControl;

    [System.NonSerialized]
    public float[] memory;

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

    [SerializeField]
    protected CameraArm cameraArm = null;

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
        this.cameraControl = new float[2];
        this.memory = new float[64];
    }

    public void ReadSensors() {
        if (this.gpsSensor != null && this.gpsSensor.gameObject.activeSelf) {
            this.gpsSensor.ReadDevice(this.gps, null);
        }

        if (this.lidarSensor != null && this.lidarSensor.gameObject.activeSelf) {
            this.lidarSensor.ReadDevice(this.lidar, null);
        }

        if (this.cameraSensor != null && this.cameraSensor.gameObject.activeSelf) {
            this.cameraSensor.ReadDevice(null, this.pixels);
        }

        if (this.directionFinder != null && this.directionFinder.gameObject.activeSelf) {
            this.directionFinder.ReadDevice(this.compass, null);
        }

        if (this.targetFinder != null && this.targetFinder.gameObject.activeSelf) {
            this.targetFinder.ReadDevice(this.targetAlignment, null);
        }

        if(this.microphoneSensor != null && this.microphoneSensor.gameObject.activeSelf) {
            this.microphoneSensor.ReadDevice(this.microphone, null);
        }

        if(this.accelerator != null && this.accelerator.gameObject.activeSelf) {
            this.accelerator.ReadDevice(this.speedometer, null);
        }
    }

    public void CommandActuators() {
        if (this.accelerator != null && this.accelerator.gameObject.activeSelf && this.speedControl[0] != 0) {
            this.accelerator.CommandDevice(this.speedControl);
            this.speedControl[0] = 0;
        }
        this.accelerator.PhysicsUpdate();

        if (this.steeringSystem != null && this.steeringSystem.gameObject.activeSelf && this.steeringControl[0] != 0) {
            this.steeringSystem.CommandDevice(this.steeringControl);
            this.steeringControl[0] = 0;
        }
        this.steeringSystem.PhysicsUpdate();

        if (this.brakeController != null && this.brakeController.gameObject.activeSelf && this.brakeControl[0] != 0) {
            this.brakeController.CommandDevice(this.brakeControl);
            this.brakeControl[0] = 0;
        }
        this.brakeController.PhysicsUpdate();

        if (this.transmitter != null && this.transmitter.gameObject.activeSelf && this.transmitterControl[0] != 0) {
            this.transmitter.CommandDevice(this.transmitterControl);
            this.transmitterControl[0] = 0;
        }

        if (this.cameraArm != null && this.cameraArm.gameObject.activeSelf && this.cameraControl[0] != 0) {
            this.cameraArm.CommandDevice(this.cameraControl);
            this.cameraControl[0] = 0;
        }
    }

}
