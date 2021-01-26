using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    [Header("Dev")]
    [SerializeField] protected List<string> carNameList = new List<string>();

    protected Dictionary<string, VehicleBase> activeVehicles = new Dictionary<string, VehicleBase>();

    protected VehicleBase vehicleBuffer = null;
    protected VehicleUpdateData updateBuffer = null;
    protected VehicleState stateBuffer = null;
    protected Vector3 vector3Buffer = new Vector3();
    protected Vector2 positionOffset = new Vector2();

    public void Init(Vector2 positionOffset) {
        this.positionOffset = positionOffset;

        foreach (string carName in this.carNameList) {
            this.activeVehicles.Add(carName, GameObject.Find(carName).GetComponent<VehicleBase>());
        }
    }

    public void UpdateCars(string rawData) {
        string[] dataPerVehicle = rawData.Split(scr_TCP.DATA_DELIM);

        for (int i = 0; i < dataPerVehicle.Length; i++) {
            this.updateBuffer = JsonUtility.FromJson<VehicleUpdateData>(dataPerVehicle[i]);

            if (this.updateBuffer.vehicleId == null) {
                Debug.LogError("Vehicle data received from SUMO which lacks a proper vehicle ID.");
                continue;
            }

            if (!this.activeVehicles.ContainsKey(this.updateBuffer.vehicleId)) {

                Debug.LogError("Vehicle ID [" + this.updateBuffer.vehicleId + "] was not found in the dictionary of active vehicles.");
                continue;

            }

            this.vehicleBuffer = this.activeVehicles[this.updateBuffer.vehicleId];

            this.vector3Buffer = this.vehicleBuffer.transform.position;
            this.vector3Buffer.x = (float)(this.updateBuffer.position[0] + this.positionOffset.x);
            this.vector3Buffer.z = (float)(this.updateBuffer.position[1] + this.positionOffset.y);

            this.vehicleBuffer.transform.position = this.vector3Buffer;
            this.vehicleBuffer.transform.rotation = Quaternion.AngleAxis(this.updateBuffer.heading, Vector3.up);

            this.stateBuffer = this.vehicleBuffer.GetVehicleState();
            this.stateBuffer.SetHeading(this.updateBuffer.heading);
            this.stateBuffer.SetSpeed(this.updateBuffer.speed);
            this.stateBuffer.SetTimer(Time.fixedDeltaTime);
            this.stateBuffer.SetIsBraking(this.updateBuffer.brake);

            this.vehicleBuffer.UpdateState();
        }
    }
}
