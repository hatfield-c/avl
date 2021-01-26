using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected scr_TCP tcpServer = null;
    [SerializeField] protected VehicleManager vehicleManager = null;

    [Header("Space Offsets")]
    [SerializeField] protected Vector2 positionOffset = new Vector2();

    void Awake() {
        this.vehicleManager.Init(this.positionOffset);
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
                this.vehicleManager.UpdateCars(messageComponents[2]);
                break;
            default:
                break;
        }

    }

 
}
