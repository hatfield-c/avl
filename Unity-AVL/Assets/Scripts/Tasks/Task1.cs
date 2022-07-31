using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task1 : TaskInterface
{
    public void Execute(DataBusInterface addressBus, DataBusInterface commandBus) {
        byte[] address = System.BitConverter.GetBytes(16);
        byte[] command = new byte[commandBus.GetSize()];

        command[0] = 0;
        
        byte[] confirmation = addressBus.WriteBus(address);
        byte[] result = commandBus.WriteBus(command);

        int index = 52 * 3;

        int r = result[index];
        int g = result[index + 1];
        int b = result[index + 2];

        Debug.Log($"{r}, {g}, {b}");
    }
}
