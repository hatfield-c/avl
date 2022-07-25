using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task1 : TaskInterface
{
    public void Execute(DataBusInterface sensorBus, string actuatorBus) {
        byte[] command0 = { 64, 0 };
        
        byte[] data0 = sensorBus.ReadData(command0);
        
        float distance0 = System.BitConverter.ToSingle(data0, 0);

        Debug.Log(distance0);
    }
}
