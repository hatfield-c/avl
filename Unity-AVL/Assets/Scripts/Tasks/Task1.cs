using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task1 : TaskInterface
{
    public void Execute(DataBusInterface sensorBus, string actuatorBus) {
        byte[] command0 = { 32, 0 };
        byte[] command1 = { 32, 4 };
        byte[] command2 = { 32, 8 };
        byte[] command3 = { 32, 12 };
        byte[] command4 = { 10, 0 };
        byte[] command5 = { 10, 1 };

        byte[] data0 = sensorBus.ReadData(command0);
        byte[] data1 = sensorBus.ReadData(command1);
        byte[] data2 = sensorBus.ReadData(command2);
        byte[] data3 = sensorBus.ReadData(command3);
        byte[] data4 = sensorBus.ReadData(command4);
        byte[] data5 = sensorBus.ReadData(command5);

        float distance0 = System.BitConverter.ToSingle(data0, 0);
        float distance1 = System.BitConverter.ToSingle(data1, 0);
        float distance2 = System.BitConverter.ToSingle(data2, 0);
        float distance3 = System.BitConverter.ToSingle(data3, 0);
        float latitude = System.BitConverter.ToSingle(data4, 0);
        float longitude = System.BitConverter.ToSingle(data5, 0);

        Debug.Log($"{distance0}, {distance1}, {distance2}, {distance3} {latitude}, {longitude}");
    }
}
