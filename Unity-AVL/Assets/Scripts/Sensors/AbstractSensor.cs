using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSensor : MonoBehaviour
{
    [SerializeField]
    protected int address = 0;
    
    public int GetAddress() {
        return this.address;
    }

    abstract public byte[] ReadSensor();

}
