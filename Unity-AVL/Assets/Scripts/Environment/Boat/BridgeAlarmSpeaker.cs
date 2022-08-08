using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeAlarmSpeaker : AbstractSoundSource {
    protected enum AlarmState {
        silent,
        bridge_crossing,
        bridge_raising
    };

    [SerializeField]
    protected Color silentColor = new Color(0f, 0f, 1f);

    [SerializeField]
    protected Color crossingColor = new Color(0f, 1f, 0f);

    [SerializeField]
    protected Color raisingColor = new Color(1f, 0f, 0f);

    [SerializeField]
    protected MeshRenderer alarmRenderer = null;

    [SerializeField]
    protected float maxDistance = 10f;

    [SerializeField]
    protected int soundCrossing = 0;

    [SerializeField]
    protected int soundRaising = 0;

    protected AlarmState currentstate = AlarmState.silent;

    public override int GetSound(Vector3 position) {
        if(this.currentstate == AlarmState.silent) {
            return 0;
        }

        Vector3 difference = this.transform.position - position;

        if(difference.magnitude > this.maxDistance) {
            return 0;
        }

        if(this.currentstate == AlarmState.bridge_crossing) {
            return this.soundCrossing;
        }

        return this.soundRaising;
    }

    public void ModifyState(float stateMessage) {
        if (stateMessage == BridgeAlarmReceiver.SIGNAL_SILENT) {
            this.currentstate = AlarmState.silent;
            this.alarmRenderer.material.color = this.silentColor;
        }

        if (stateMessage == BridgeAlarmReceiver.SIGNAL_CROSS && this.currentstate == AlarmState.silent) {
            this.currentstate = AlarmState.bridge_crossing;
            this.alarmRenderer.material.color = this.crossingColor;
        }

        if (stateMessage == BridgeAlarmReceiver.SIGNAL_RAISED) {
            this.currentstate = AlarmState.bridge_raising;
            this.alarmRenderer.material.color = this.raisingColor;
        }
    }
}
