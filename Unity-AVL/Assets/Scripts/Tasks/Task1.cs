using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task1 : TaskInterface
{
    public void Execute(DataBusInterface addressBus, DataBusInterface commandBus) {
        byte[] address = System.BitConverter.GetBytes(10);
        byte[] command = new byte[commandBus.GetSize()];

        command[0] = 0;
        
        byte[] confirmation = addressBus.WriteBus(address);
        byte[] result = commandBus.WriteBus(command);

        byte[] latRaw = new byte[4];
        byte[] lonRaw = new byte[4];
        for(int i = 0; i < 4; i++) {
            latRaw[i] = result[i];
            lonRaw[i] = result[i + 4];
        }

        float distance0 = System.BitConverter.ToSingle(latRaw, 0);
        float distance1 = System.BitConverter.ToSingle(lonRaw, 0);

        Debug.Log($"{distance0}, {distance1}");
    }
}
