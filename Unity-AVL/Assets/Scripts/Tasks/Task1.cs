using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task1 : TaskInterface
{
    public void Execute(DeviceRegistry devices) {
        //Debug.Log($"{devices.pixels[3,7,0]}, {devices.pixels[3, 7, 1]}, {devices.pixels[3, 7, 2]}");

        devices.transmitterControl[0] = 1;
        devices.transmitterControl[1] = BridgeAlarmReceiver.SIGNAL_CROSS;

        float lattitude = devices.gps[1];

        Debug.Log($"Sound: {devices.microphone[0]}");
    }
}
