using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task1 : TaskInterface
{
    public void Execute(DataBusInterface sensorBus, string actuatorBus) {
        byte address = (byte)25;

        byte[] data = sensorBus.ReadData(address);

        Debug.Log($"{data}, {data[0]}");
    }
}
