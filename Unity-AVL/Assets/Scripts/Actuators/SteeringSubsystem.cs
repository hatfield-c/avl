using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringSubsystem : AbstractDevice {
    [SerializeField]
    protected PhysicsBody body = null;

    protected float currentAngle = 0f;

    public void PhysicsUpdate() {
        if (this.currentAngle != 0f) {
            this.body.Rotate(this.currentAngle);
            this.currentAngle = 0f;
        }
    }

    public override void ReadDevice(float[] memory, int[,,] empty) {
        memory[0] = this.currentAngle;
    }

    public override void CommandDevice(float[] options) {
        this.currentAngle = options[1];
    }

}
