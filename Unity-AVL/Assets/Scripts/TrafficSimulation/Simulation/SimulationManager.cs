using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected VehicleManager vehicleManager = null;
    [SerializeField] protected TerrainManager terrainManager = null;

    [Header("TCP Parameters")]
    [SerializeField] protected UnityListener unityListener = new UnityListener();
    [SerializeField] protected UnityServer unityServer = new UnityServer();

    protected string messageBuffer = "";

    void Awake() {
        this.vehicleManager.Init();
        this.unityListener.StartListening();
        UnityServer.ConnectToSumoListener();

        string egoInitMessage = this.vehicleManager.GetEgoInitMessage();

        if(egoInitMessage == null) {
            return;
        }

        UnityServer.SendMessage(egoInitMessage);
    }

    void FixedUpdate() {
        while (this.unityListener.HasMessages()) {
            this.messageBuffer = this.unityListener.GetMessage();
            this.ProcessInboundMessage(this.messageBuffer);
        }

        string egoData = this.vehicleManager.EncodeUpdateData();
        
        if(egoData == null) {
            return;
        }

        string outboundMessage = UnityServer.CompileMessage(
            TcpProtocol.TO_SUMO, 
            TcpProtocol.SUMO_UPDT_EGO, 
            egoData
        );
        UnityServer.SendMessage(outboundMessage);
    }

    void OnDestroy() {
        UnityListener.StopListening();
    }

    public void ProcessInboundMessage(string message) {
        if (message == null) {
            return;
        }

        string[] messageComponents = message.Split(TcpProtocol.MSG_DELIM);

        string destination = messageComponents[0];
        string command = messageComponents[1];
        string rawData = messageComponents[2];

        if (destination != TcpProtocol.TO_UNITY) {
            return;
        }

        switch (command) {
            case TcpProtocol.UNITY_DELT_CAR:
                this.vehicleManager.DeleteCars(rawData);
                break;
            case TcpProtocol.UNITY_INIT_CAR:
                this.vehicleManager.InitCars(rawData);
                break;
            case TcpProtocol.UNITY_UPDT_CAR:
                this.vehicleManager.UpdateCars(rawData);
                break;
            case TcpProtocol.UNITY_INIT_JUNC:
                this.terrainManager.CreateJunctions(rawData);
                break;
            case TcpProtocol.UNITY_INIT_EDGE:
                this.terrainManager.CreateEdges(rawData);
                break;
            default:
                break;
        }

    }

}
