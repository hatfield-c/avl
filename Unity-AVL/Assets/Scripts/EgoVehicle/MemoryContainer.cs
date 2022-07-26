using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryContainer : MonoBehaviour
{
    protected byte[] mem = new byte[RTOS.GetMemSize()];

    public byte[] GetMemory() {
        return this.mem;
    }
}
