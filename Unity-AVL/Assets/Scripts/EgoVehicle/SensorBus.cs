using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorBus : MonoBehaviour, DataBusInterface
{

    [SerializeField]
    protected List<AbstractSensor> sensorRegistry = new List<AbstractSensor>();

    protected Dictionary<int, AbstractSensor> sensors = new Dictionary<int, AbstractSensor>();

    protected byte[] bus = null;

    void Start() {
        AbstractSensor sensor;
        
        for (int i = 0; i < this.sensorRegistry.Count; i++) {
            sensor = this.sensorRegistry[i];
            int address = sensor.GetAddress();
            
            this.sensors[address] = sensor;
        }
    }

    public byte[] ReadData(byte address) {
        int addr = (int)address;

        if (!this.sensors.ContainsKey(addr)) {
            Debug.LogError($"Error: Tried to read data from sensor with address '{addr}', but no sensor with that address has been registered with the RTOS. An empty byte array will be returned.");

            return new byte[4]; ;
        }

        AbstractSensor sensor = this.sensors[addr];

        return sensor.ReadSensor();
    }

    public void WriteData(byte[] data) { }
}
