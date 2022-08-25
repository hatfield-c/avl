using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task1 : TaskInterface
{
    public void Execute(DeviceRegistry devices) {

        devices.speedControl[0] = 1;
        devices.speedControl[1] = 0.2f;

        float angle = devices.compass[0];

        Debug.Log($"Sound: {angle}");
    }
}
