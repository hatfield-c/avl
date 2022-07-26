using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddressBus : MonoBehaviour, DataBusInterface
{
    protected byte[] bus = null;

    protected const int BUS_SIZE = 4;

    public byte[] ReadBus() {
        return this.bus;
    }

    public void WriteBus(byte[] address) {
        if (address.Length != this.GetSize()) {
            Debug.LogError($"Error: Tried to put {address.Length} bytes on the address bus, but the bus has size of {this.GetSize()} bytes. Aborting address placement.");
            this.bus = new byte[this.GetSize()];

            return;
        }

        int addr = System.BitConverter.ToInt32(address, 0);

        if(addr >= RTOS.GetMemSize()) {
            Debug.LogError($"Error: Tried to place address value of '{address}' on the address bus, but the maximum memory address is {RTOS.GetMemSize() - 1}. Aborting address placement.");
            this.bus = new byte[this.GetSize()];

            return;
        }

        this.bus = address;
    }

    public int GetSize() {
        return AddressBus.BUS_SIZE;
    }
}
