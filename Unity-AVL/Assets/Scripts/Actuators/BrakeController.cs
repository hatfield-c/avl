using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeController : AbstractDevice
{
    [SerializeField]
    protected PhysicsBody body = null;

    [SerializeField]
    protected float brakePower = 0.9f;

    float brakeTime = 0f;

    public void PhysicsUpdate() {
        if(this.brakeTime > 0) {
            this.body.ApplyDrag(this.brakePower);

            this.brakeTime -= Time.fixedDeltaTime;
        }
    }

    public override void ReadDevice(float[] memory, int[,,] empty) {
        memory[0] = this.brakeTime;
    }

    public override void CommandDevice(float[] options) {
        this.brakeTime = options[1];
    }
}
