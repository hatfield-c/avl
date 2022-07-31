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

    public byte[] WriteBus(byte[] address) {
        if (address.Length != this.GetSize()) {
            Debug.LogError($"Error: Tried to put {address.Length} bytes on the address bus, but the bus has size of {this.GetSize()} bytes. Aborting address placement.");
            this.bus = new byte[this.GetSize()];

            return new byte[] { 0 };
        }

        this.bus = address;

        return new byte[] { 1 };
    }

    public int GetSize() {
        return AddressBus.BUS_SIZE;
    }
}
