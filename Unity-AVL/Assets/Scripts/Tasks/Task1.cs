﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task1 : TaskInterface
{
    public void Execute(DeviceRegistry devices) {

        float angle = devices.compass[0];

        Debug.Log($"Sound: {angle}");
    }
}
