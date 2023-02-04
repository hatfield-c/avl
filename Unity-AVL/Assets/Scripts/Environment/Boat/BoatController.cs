using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    protected enum BridgeState { 
        bridge_lowered,
        bridge_raising,
        bridge_raised,
        bridge_lowering
    };

    protected enum BoatState {
        boat_moving,
        boat_ready
    };

    [Header("Boat Parameters")]
    [SerializeField]
    protected Transform boat = null;

    [SerializeField]
    protected float maxDistance = 10f;

    [SerializeField]
    protected float speed = 0.1f;

    [SerializeField]
    protected float minWaitTime = 5f;

    [SerializeField]
    protected float maxWaitTime = 15f;

    [Header("Bridge Parameters")]
    [SerializeField]
    protected Transform bridge = null;

    [SerializeField]
    protected float holdTime = 7f;

    [SerializeField]
    protected float holdAngle = 55f;

    [SerializeField]
    protected float raiseSpeed = 0.2f;

    [Header("Alarm Parameters")]
    [SerializeField]
    protected BridgeAlarmSpeaker alarmSpeaker = null;

    [SerializeField]
    protected BridgeAlarmReceiver alarmReceiver = null;

    protected float currentDistance = 0f;
    protected float currentTime = 0f;
    protected float currentCheckTime = 0f;
    protected BridgeState currentBridgeState = BridgeState.bridge_lowered;
    protected BoatState currentBoatState = BoatState.boat_ready;

    void Start() {
        this.currentCheckTime = Random.Range(this.minWaitTime, this.maxWaitTime);
        this.boat.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        this.CrossingCheck();
        this.UpdateBoat();
        this.UpdateBridge();
    }

    protected void CrossingCheck() {
        if(this.currentBridgeState != BridgeState.bridge_lowered || this.currentBoatState != BoatState.boat_ready) {
            return;
        }

        this.currentCheckTime -= Time.fixedDeltaTime;

        if(this.currentCheckTime <= 0) {
            this.alarmReceiver.ReceiveMessage(BridgeAlarmReceiver.SIGNAL_RAISED);
            this.currentBridgeState = BridgeState.bridge_raising;
            this.currentBoatState = BoatState.boat_moving;
            this.currentCheckTime = 0;

            this.boat.gameObject.SetActive(true);
        }
    }

    protected void UpdateBridge() {
        if(this.currentBridgeState == BridgeState.bridge_lowered) {
            return;
        }

        if(this.currentBridgeState == BridgeState.bridge_raising) {
            this.bridge.eulerAngles += Vector3.right * this.raiseSpeed;

            if(this.bridge.eulerAngles.x >= this.holdAngle) {
                this.bridge.eulerAngles = Vector3.right * this.holdAngle;

                this.currentBridgeState = BridgeState.bridge_raised;
            }

            return;
        }

        if(this.currentBridgeState == BridgeState.bridge_raised) {
            this.currentTime += Time.fixedDeltaTime;

            if(this.currentTime >= this.holdTime) {
                this.currentTime = 0f;

                this.currentBridgeState = BridgeState.bridge_lowering;
            }

            return;
        }

        if (this.currentBridgeState == BridgeState.bridge_lowering) {
            this.bridge.eulerAngles -= Vector3.right * this.raiseSpeed;

            if (this.bridge.eulerAngles.x <= 0 || this.bridge.eulerAngles.x >= 350f) {
                this.bridge.eulerAngles = Vector3.zero;

                this.currentBridgeState = BridgeState.bridge_lowered;
                this.alarmReceiver.ReceiveMessage(BridgeAlarmReceiver.SIGNAL_SILENT);
            }

            return;
        }
    }

    protected void UpdateBoat() {
        if(this.currentBoatState == BoatState.boat_ready) {
            return;
        }

        if (this.currentDistance > this.maxDistance) {
            this.currentDistance = 0f;
            this.boat.position = this.transform.position;
            this.currentBoatState = BoatState.boat_ready;
            this.boat.gameObject.SetActive(false);

            this.currentCheckTime = Random.Range(this.minWaitTime, this.maxWaitTime);

            return;
        }

        this.boat.position += this.boat.forward * this.speed;
        this.currentDistance += this.speed;
    }

    public void TriggerBoat() {
        if(this.currentBridgeState != BridgeState.bridge_lowered) {
            return;
        }

        this.currentCheckTime = 0f;
    }
}
