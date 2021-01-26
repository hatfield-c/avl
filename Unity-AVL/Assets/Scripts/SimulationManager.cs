using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected scr_TCP tcpServer = null;

    [Header("Space Offsets")]
    [SerializeField] protected Vector2 positionOffset = new Vector2();

    [Header("Dev")]
    [SerializeField] protected List<string> carNameList = new List<string>();

    protected Dictionary<string, VehicleBase> activeVehicles = new Dictionary<string, VehicleBase>();
    protected VehicleBase vehicleBuffer = null;
    protected VehicleUpdateData updateVehicleBuffer = null;
    protected VehicleState stateBuffer = null;
    protected GameObject objectBuffer = null;
    protected Vector3 vector3Buffer = new Vector3();

    void Start() {

        foreach (string carName in this.carNameList) {
            this.activeVehicles.Add(carName, GameObject.Find(carName).GetComponent<VehicleBase>());
        }
    }

    void FixedUpdate() {

        string msg = this.tcpServer.RxMsg();
        this.ProcessMessage(msg);
    }

    public void ProcessMessage(string message) {
        if (message == null) {
            return;
        }

        string[] messageComponents = message.Split(scr_TCP.MSG_DELIM);

        if(messageComponents[0] != scr_TCP.TO_UNITY) {
            return;
        }

        switch (messageComponents[1]) {
            case scr_TCP.UNITY_UPDT_CAR:
                this.UpdateCars(messageComponents[2]);
                break;
            default:
                break;
        }

    }

    protected void UpdateCars(string rawData) {
        string[] dataPerVehicle = rawData.Split(scr_TCP.DATA_DELIM);

        for (int i = 0; i < dataPerVehicle.Length; i++) {
            this.updateVehicleBuffer = JsonUtility.FromJson<VehicleUpdateData>(dataPerVehicle[i]);

            if (this.updateVehicleBuffer.vehicleId == null) {
                Debug.LogError("Vehicle data received from SUMO which lacks a proper vehicle ID.");
                continue;
            }

            if (!this.activeVehicles.ContainsKey(this.updateVehicleBuffer.vehicleId)) {

                Debug.LogError("Vehicle ID [" + this.updateVehicleBuffer.vehicleId + "] was not found in the dictionary of active vehicles.");
                continue;

            }

            this.vehicleBuffer = this.activeVehicles[this.updateVehicleBuffer.vehicleId];

            this.vector3Buffer = this.vehicleBuffer.transform.position;
            this.vector3Buffer.x = (float)(this.updateVehicleBuffer.position[0] + this.positionOffset.x);
            this.vector3Buffer.z = (float)(this.updateVehicleBuffer.position[1] + this.positionOffset.y);

            this.vehicleBuffer.transform.position = this.vector3Buffer;
            this.vehicleBuffer.transform.rotation = Quaternion.AngleAxis(this.updateVehicleBuffer.heading, Vector3.up);

            this.stateBuffer = this.vehicleBuffer.GetVehicleState();
            this.stateBuffer.SetHeading(this.updateVehicleBuffer.heading);
            this.stateBuffer.SetSpeed(this.updateVehicleBuffer.speed);
            this.stateBuffer.SetTimer(Time.fixedDeltaTime);
            this.stateBuffer.SetIsBraking(this.updateVehicleBuffer.brake);

            this.vehicleBuffer.UpdateState();
        }
    }
}
