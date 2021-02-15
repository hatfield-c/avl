using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected VehicleManager vehicleManager = null;
    [SerializeField] protected TerrainManager terrainManager = null;

    [Header("Parameters")]
    [SerializeField] protected TcpServer tcpServer = new TcpServer();

    protected string messageBuffer = "";

    void Awake() {
        this.vehicleManager.Init();
        this.tcpServer.StartClient();
    }

    void FixedUpdate() {
        while (this.tcpServer.HasMessages()) {
            this.messageBuffer = tcpServer.GetMessage();
            this.ProcessMessage(this.messageBuffer);
        }
    }

    void OnDestroy() {
        TcpServer.KillClient();
    }

    public void ProcessMessage(string message) {
        if (message == null) {
            return;
        }

        string[] messageComponents = message.Split(TcpServer.MSG_DELIM);

        string destination = messageComponents[0];
        string command = messageComponents[1];
        string rawData = messageComponents[2];

        if (destination != TcpServer.TO_UNITY) {
            return;
        }

        switch (command) {
            case TcpServer.UNITY_DELT_CAR:
                this.vehicleManager.DeleteCars(rawData);
                break;
            case TcpServer.UNITY_INIT_CAR:
                this.vehicleManager.InitCars(rawData);
                break;
            case TcpServer.UNITY_UPDT_CAR:
                this.vehicleManager.UpdateCars(rawData);
                break;
            case TcpServer.UNITY_INIT_JUNC:
                this.terrainManager.CreateJunctions(rawData);
                break;
            case TcpServer.UNITY_INIT_EDGE:
                this.terrainManager.CreateEdges(rawData);
                break;
            default:
                break;
        }

    }

}
