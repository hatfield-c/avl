using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected VehicleManager vehicleManager = null;

    [Header("Parameters")]
    [SerializeField] protected TcpServer tcpServer = new TcpServer();
    [SerializeField] protected Vector2 positionOffset = new Vector2();

    protected string messageBuffer = "";

    void Awake() {
        this.vehicleManager.Init(this.positionOffset);
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

        if(messageComponents[0] != TcpServer.TO_UNITY) {
            return;
        }

        switch (messageComponents[1]) {
            case TcpServer.UNITY_DELT_CAR:
                this.vehicleManager.DeleteCars(messageComponents[2]);
                break;
            case TcpServer.UNITY_INIT_CAR:
                this.vehicleManager.InitCars(messageComponents[2]);
                break;
            case TcpServer.UNITY_UPDT_CAR:
                this.vehicleManager.UpdateCars(messageComponents[2]);
                break;
            default:
                break;
        }

    }

}
