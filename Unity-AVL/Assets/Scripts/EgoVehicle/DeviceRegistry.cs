using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceRegistry : MonoBehaviour
{
    [SerializeField]
    protected List<AbstractDevice> deviceRegistry = new List<AbstractDevice>();

    protected Dictionary<int, AbstractDevice> devices = new Dictionary<int, AbstractDevice>();

    void Start()
    {
        AbstractDevice device;

        for (int i = 0; i < this.deviceRegistry.Count; i++) {
            device = this.deviceRegistry[i];
            int address = device.GetAddress();

            this.devices[address] = device;
        }
    }

    public Dictionary<int, AbstractDevice> GetDevices() {
        return this.devices;
    }
}
