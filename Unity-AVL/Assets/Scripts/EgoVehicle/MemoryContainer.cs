using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryContainer : MonoBehaviour
{
    [SerializeField]
    protected int memSize = 256;

    protected byte[] mem = null;

    void Start() {
        this.mem = new byte[this.memSize];
    }

    public byte[] GetMemory() {
        return this.mem;
    }

    public int GetMemSize() {
        return this.memSize;
    }
}
