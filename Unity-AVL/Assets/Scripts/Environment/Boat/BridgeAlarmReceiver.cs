using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeAlarmReceiver : AbstractReceiver {

    public const float SIGNAL_SILENT = 42.5f;
    public const float SIGNAL_CROSS = 55.6f;
    public const float SIGNAL_RAISED = 78.9f;

    [SerializeField]
    protected BridgeAlarmSpeaker speaker = null;

    public override void ReceiveMessage(float message) {
        this.speaker.ModifyState(message);
    }
}
