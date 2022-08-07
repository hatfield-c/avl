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
        }

        if (stateMessage == BridgeAlarmReceiver.SIGNAL_CROSS && this.currentstate == AlarmState.silent) {
            this.currentstate = AlarmState.bridge_crossing;
        }

        if (stateMessage == BridgeAlarmReceiver.SIGNAL_RAISED) {
            this.currentstate = AlarmState.bridge_raising;
        }
    }
}
