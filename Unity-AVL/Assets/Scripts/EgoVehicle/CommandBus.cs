﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBus : MonoBehaviour, DataBusInterface
{
    [SerializeField]
    protected AddressBus addressBus = null;

    [SerializeField]
    protected DeviceRegistry deviceRegistry = null;

    protected Dictionary<int, AbstractDevice> devices = null;
    protected byte[] bus = null;

    protected const int BUS_SIZE = 8;

    void Start() {
        this.devices = this.deviceRegistry.GetDevices();
    }

    public byte[] ReadBus() {
        return this.bus;
    }

    public byte[] WriteBus(byte[] command) {
        if(command.Length != this.GetSize()) {
            Debug.LogError($"Error: Tried to put {command.Length} bytes on the sensor bus, but the bus has size of {this.GetSize()} bytes. Aborting command.");
            this.bus = new byte[this.GetSize()];

            return new byte[1];
        }

        this.bus = command;

        byte[] address = this.addressBus.ReadBus();
        int addr = System.BitConverter.ToInt32(address, 0);

        if (!this.devices.ContainsKey(addr)) {
            Debug.LogError($"Error: The Command Bus tried to send a command to a device with address of '{addr}', but no device with that address has been registered with the RTOS. Aborting command.");

            return new byte[1];
        }

        AbstractDevice device = this.devices[addr];
        
        return device.CommandDevice(command);
    }

    public int GetSize() {
        return CommandBus.BUS_SIZE;
    }
}
