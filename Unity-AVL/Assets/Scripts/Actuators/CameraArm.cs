using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArm : AbstractDevice {

    [SerializeField]
    protected Transform cameraArm = null;

    [SerializeField]
    protected float minAngle = -20f;

    [SerializeField]
    protected float maxAngle = 20f;

    protected float angle = 0f;

    public override void CommandDevice(float[] options) {
        this.angle = options[1];

        if(this.angle < 0f && this.angle < this.minAngle) {
            this.angle = this.minAngle;
        } else if (this.angle > 0f && this.angle > this.maxAngle) {
            this.angle = this.maxAngle;
        }

        this.cameraArm.eulerAngles = new Vector3(
            -angle,
            this.cameraArm.eulerAngles.y,
            this.cameraArm.eulerAngles.z
        );
    }

    public override void ReadDevice(float[] memory, int[,,] memoryPixels) {
        memory[0] = this.angle;
    }
}
