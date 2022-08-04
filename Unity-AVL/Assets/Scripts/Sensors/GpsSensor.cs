using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GpsSensor : AbstractDevice
{
    [SerializeField]
    protected Transform target = null;

    [SerializeField]
    protected Transform spinner = null;

    [SerializeField]
    protected float spinRate = 1f;

    override public void ReadDevice(float[] memory, int[,,] empty) {
        memory[0] = this.target.position.x;
        memory[1] = this.target.position.z;
    }

    public override void CommandDevice(float[] empty) { }

    void FixedUpdate() {
        this.spinner.eulerAngles += Vector3.up * this.spinRate;
    }
}
