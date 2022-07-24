using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected GameObject vehicleContainer = null;
    [SerializeField] protected VehicleFactory factory = null;
    [SerializeField] protected Warehouse warehouse = null;

    [Header("Parameters")]
    [SerializeField] protected VehicleManifest vehicleManifest = new VehicleManifest();
    [SerializeField] protected List<VehicleBase> egoVehicleList = new List<VehicleBase>();

    protected Dictionary<string, VehicleBase> sumoVehicles = new Dictionary<string, VehicleBase>();
    protected Dictionary<string, VehicleBase> egoVehicles = new Dictionary<string, VehicleBase>();

    protected const string UPDATE_ERR_MSG = "[Vehicle Manager:Update Error]";
    protected const string DELETE_ERR_MSG = "[Vehicle Manager:Delete Error]";
    protected const string INIT_ERR_MSG = "[Vehicle Manager:Init Error]";

    protected float offset = 0;

    public void Init() {
        List<VehicleBase> vehicleList = this.factory.CreateAllVehicles(this.vehicleManifest);
        List<IStorable> storableList = vehicleList.Cast<IStorable>().ToList();
        this.warehouse.InitFromList(storableList);

        foreach (VehicleBase egoVehicle in this.egoVehicleList) {
            VehicleInitData dummyData = egoVehicle.BuildInitData();
            egoVehicle.BuildCar(dummyData.vehicleType);
            egoVehicle.Init(dummyData);

            this.egoVehicles.Add(egoVehicle.GetId(), egoVehicle);
        }
    }

    public string EncodeInitData() {
        if (this.egoVehicles.Count < 1) {
            return null;
        }

        string egoData = "";

        VehicleBase vehicle;

        foreach (KeyValuePair<string, VehicleBase> pair in this.egoVehicles) {
            vehicle = pair.Value;

            string jsonData = vehicle.GetInitJson();

            egoData += jsonData + TcpProtocol.DATA_DELIM;
        }

        egoData = egoData.Substring(0, (egoData.Length - 1));

        return egoData;
    }

    public string EncodeUpdateData() {
        if(this.egoVehicles.Count < 1) {
            return null;
        }

        string egoData = "";

        string vehicleId;
        VehicleBase vehicle;

        foreach (KeyValuePair<string, VehicleBase> pair in this.egoVehicles) {
            vehicleId = pair.Key;
            vehicle = pair.Value;

            string jsonData = vehicle.GetUpdateJson();

            egoData += jsonData + TcpProtocol.DATA_DELIM;
        }

        egoData = egoData.Substring(0, (egoData.Length - 1));
        
        return egoData;
    }

    public string GetEgoInitMessage() {
        string initData = this.EncodeInitData();

        if(initData == null) {
            return null;
        }

        string initMessage = UnityServer.CompileMessage(
            TcpProtocol.TO_SUMO,
            TcpProtocol.SUMO_INIT_EGO,
            initData
        );

        return initMessage;
    }

    public void UpdateCars(string rawData) {
        string[] dataPerVehicle = rawData.Split(TcpProtocol.DATA_DELIM);

        VehicleUpdateData updateData;
        for (int i = 0; i < dataPerVehicle.Length; i++) {
            updateData = JsonUtility.FromJson<VehicleUpdateData>(dataPerVehicle[i]);

            if (updateData.vehicleId == null) {
                this.LogError(
                    VehicleManager.UPDATE_ERR_MSG,
                    "Vehicle ID received from SUMO is null"
                );
                continue;
            }

            if (!this.sumoVehicles.ContainsKey(updateData.vehicleId)) {
                this.LogError(
                        VehicleManager.UPDATE_ERR_MSG,
                        "Vehicle ID '" + 
                        updateData.vehicleId +
                        "' was not found in the dictionary of active vehicles."
                );
                continue;
            }

            VehicleBase vehicle = this.sumoVehicles[updateData.vehicleId];
            vehicle.UpdateState(updateData);
        }
    }

    public void InitCars(string rawData) {
        string[] dataPerVehicle = rawData.Split(TcpProtocol.DATA_DELIM);
        
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

            initData.vehicleType = VehicleManager.DetermineType(initData.vehicleClass);

            if (!this.warehouse.HasItem(initData.vehicleType.ToString())) {
                this.LogError(
                    VehicleManager.INIT_ERR_MSG,
                    "Fatal error! The maximum number of vehicles of type '" +
                    initData.vehicleType.ToString() +
                    "' has been reached. Please increase the maximum allowable number for this vehicle type, " +
                    "or decrease the number needed for the simulation."
                );

                UnityListener.StopListening();
            }

            VehicleBase vehicle = (VehicleBase)this.warehouse.FetchItem(initData.vehicleType.ToString());
            
            vehicle.Init(initData);
            vehicle.transform.parent = this.vehicleContainer.transform;
            vehicle.transform.position += Vector3.right * this.offset;
            this.offset += 5;
            

            if (this.sumoVehicles.ContainsKey(initData.vehicleId)) {
                this.LogError(
                    VehicleManager.INIT_ERR_MSG,
                    "Fatal error! Trying to init vehicle with ID '" +
                    initData.vehicleId +
                    "', but a vehicle with this ID already exists in the simulation!"
                );

                UnityListener.StopListening();
            }

            this.sumoVehicles.Add(initData.vehicleId, vehicle);
        }
    }

    public void DeleteCars(string rawData) {
        string[] dataPerVehicle = rawData.Split(TcpProtocol.DATA_DELIM);

        VehicleDeleteData deleteData;
        for (int i = 0; i < dataPerVehicle.Length; i++) {
            deleteData = JsonUtility.FromJson<VehicleDeleteData>(dataPerVehicle[i]);

            if (deleteData.vehicleId == null) {
                this.LogError(
                    VehicleManager.DELETE_ERR_MSG,
                    "Vehicle ID received from SUMO is null"
                );
                continue;
            }

            if (!this.sumoVehicles.ContainsKey(deleteData.vehicleId)) {
                this.LogError(
                        VehicleManager.DELETE_ERR_MSG,
                        "Vehicle ID '" +
                        deleteData.vehicleId +
                        "' was not found in the dictionary of active vehicles."
                );
                continue;
            }

            VehicleBase vehicle = this.sumoVehicles[deleteData.vehicleId];
            this.sumoVehicles.Remove(deleteData.vehicleId);

            vehicle.Disable();
            this.warehouse.StockItem((IStorable)vehicle);
        }
    }

    public static VehicleFactory.VehicleTypes DetermineType(string vehicleClass) {
        VehicleFactory.VehicleTypes vehicleType;

        switch (vehicleClass) {
            case "passenger":
                vehicleType = VehicleFactory.VehicleTypes.Car;
                break;
            case "trailer":
                vehicleType = VehicleFactory.VehicleTypes.Trailer;
                break;
            default:
                vehicleType = VehicleFactory.VehicleTypes.None;
                break;
        }

        return vehicleType;
    }

    public static string DetermineVehicleClass(VehicleFactory.VehicleTypes vehicleType) {
        string vehicleClass = "";

        switch (vehicleType) {
            case VehicleFactory.VehicleTypes.Car:
                vehicleClass = "passenger";
                break;
            case VehicleFactory.VehicleTypes.Trailer:
                vehicleClass = "trailer";
                break;
            case VehicleFactory.VehicleTypes.None:
                vehicleClass = "custom1";
                break;
            default:
                vehicleClass = null;
                break;
        }

        return vehicleClass;
    }

    public static void OnVehicleCollision(VehicleBase vehicle) {
        string initMessage = UnityServer.CompileMessage(
            TcpProtocol.TO_SUMO,
            TcpProtocol.SUMO_INIT_EGO,
            vehicle.GetInitJson()
        );

        UnityServer.SendMessage(initMessage);
    }

    protected void LogError(string type, string msg) {
        Debug.LogError(type + ": " + msg);
    }
}
