using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerator : AbstractDevice
{
    [SerializeField]
    protected PhysicsBody body = null;

    [SerializeField]
    protected float gasPower = 1.5f;

    protected float targetSpeed = 0f;

    void FixedUpdate() {
        if(this.targetSpeed >= 0) {
            if (this.body.GetSpeed() < this.targetSpeed) {
                this.body.AddSpeed(this.gasPower);
            }
        } else {
            if (this.body.GetSpeed() > this.targetSpeed) {
                this.body.AddSpeed(-this.gasPower);
            }
        }

        
    }

    public override void ReadDevice(float[] memory, int[,,] empty) {
        memory[0] = this.body.GetSpeed();
    }

    public override void CommandDevice(float[] options) {
        this.targetSpeed = options[1];
    }
}
