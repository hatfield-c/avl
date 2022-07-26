using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task1 : TaskInterface
{
    public void Execute(DataBusInterface addressBus, DataBusInterface commandBus, byte[] memory) {
        byte[] address = System.BitConverter.GetBytes(32);
        byte[] command = new byte[commandBus.GetSize()];

        command[0] = 0;
        command[1] = 0;
        
        addressBus.WriteBus(address);
        commandBus.WriteBus(command);

        byte[] data = new byte[] { memory[0], memory[1], memory[2], memory[3] };

        float distance0 = System.BitConverter.ToSingle(data, 0);

        Debug.Log(distance0);
    }
}
