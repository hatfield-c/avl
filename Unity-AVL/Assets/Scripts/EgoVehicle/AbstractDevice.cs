using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDevice : MonoBehaviour
{
    [SerializeField]
    protected int address = 0;
    
    public int GetAddress() {
        return this.address;
    }

    abstract public byte[] CommandDevice(byte[] command);

}
