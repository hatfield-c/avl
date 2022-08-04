using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionSensor : AbstractDevice
{
    [SerializeField]
    protected Transform egoVehicle = null;

    [SerializeField]
    protected Transform spinner = null;

    [SerializeField]
    protected Vector3 targetDirection = Vector3.forward;

    override public void ReadDevice(float[] memory, int[,,] empty) {
        float angle = Vector3.SignedAngle(this.targetDirection, this.egoVehicle.transform.forward, Vector3.up);
        
        memory[0] = angle;
    }

    public override void CommandDevice(float[] empty) { }

    void FixedUpdate() {
        this.spinner.LookAt(this.spinner.position + this.targetDirection.normalized);
        this.spinner.eulerAngles = new Vector3(0, this.spinner.eulerAngles.y, 0);
    }
}
