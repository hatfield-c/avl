using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class TerrainManager : MonoBehaviour
{
    [SerializeField] protected GameObject junctionPrefab = null;
    [SerializeField] protected GameObject roadPrefab = null;

    [SerializeField] protected GameObject junctionContainer = null;

    protected Dictionary<string, SumoJunction> junctionRepo = new Dictionary<string, SumoJunction>();

    protected const string JUNC_INIT_ERR_MSG = "[Terrain Manager: Junction Init Error]";
    protected const string EDGE_INIT_ERR_MSG = "[Terrain Manager: Edge Init Error]";

    public void CreateJunctions(string rawData) {

        string[] dataPerVehicle = rawData.Split(TcpServer.DATA_DELIM);

        JunctionInitData initData;
        for (int i = 0; i < dataPerVehicle.Length; i++) {
            initData = JsonConvert.DeserializeObject<JunctionInitData>(dataPerVehicle[i]);

            if (initData.junctionId == null) {
                this.LogError(
                    TerrainManager.JUNC_INIT_ERR_MSG,
                    "Junction ID received from SUMO is null."
                );
                continue;
            }

            GameObject junctionObject = Instantiate(this.junctionPrefab);
            SumoJunction junction = junctionObject.GetComponent<SumoJunction>();
            junction.Init(initData);
            junction.transform.parent = this.junctionContainer.transform;

            if (this.junctionRepo.ContainsKey(initData.junctionId)) {
                this.LogError(
                    TerrainManager.JUNC_INIT_ERR_MSG,
                    "Fatal error! Trying to init junction with ID '" +
                    initData.junctionId +
                    "', but a junction with this ID already exists in the simulation!"
                );

                TcpServer.KillClient();
            }

            this.junctionRepo.Add(initData.junctionId, junction);
        }
    }

    public void CreateEdges(string rawData) {
        /*
        string[] dataPerVehicle = rawData.Split(TcpServer.DATA_DELIM);

        VehicleInitData initData;
        for (int i = 0; i < dataPerVehicle.Length; i++) {
            initData = JsonUtility.FromJson<VehicleInitData>(dataPerVehicle[i]);

            if (initData.vehicleId == null) {
                this.LogError(
                    VehicleManager.INIT_ERR_MSG,
                    "Vehicle ID received from SUMO is null."
                );
                continue;
            }

            initData.vehicleType = this.DetermineType(initData.vehicleClass);

            if (!this.warehouse.HasItem(initData.vehicleType.ToString())) {
                this.LogError(
                    VehicleManager.INIT_ERR_MSG,
                    "Fatal error! The maximum number of vehicles of type '" +
                    initData.vehicleType.ToString() +
                    "' has been reached. Please increase the maximum allowable number for this vehicle type, " +
                    "or decrease the number needed for the simulation."
                );

                TcpServer.KillClient();
            }

            VehicleBase vehicle = (VehicleBase)this.warehouse.FetchItem(initData.vehicleType.ToString());
            vehicle.Init(initData);
            vehicle.transform.parent = this.vehicleContainer.transform;


            if (this.activeVehicles.ContainsKey(initData.vehicleId)) {
                this.LogError(
                    VehicleManager.INIT_ERR_MSG,
                    "Fatal error! Trying to init vehicle with ID '" +
                    initData.vehicleId +
                    "', but a vehicle with this ID already exists in the simulation!"
                );

                TcpServer.KillClient();
            }

            this.activeVehicles.Add(initData.vehicleId, vehicle);
        
        }
        */
    }

    protected void LogError(string type, string msg) {
        Debug.LogError(type + ": " + msg);
    }
}
