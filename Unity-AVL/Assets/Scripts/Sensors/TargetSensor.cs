using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSensor : AbstractDevice
{
    [SerializeField]
    protected Transform egoVehicle = null;

    [SerializeField]
    protected Transform spinner = null;

    [SerializeField]
    protected Transform target = null;

    override public void ReadDevice(float[] memory, int[,,] empty) {

        if(this.target == null) {
            memory[0] = 0f;
        }

        Vector3 direction = new Vector3(
            this.target.position.x - this.egoVehicle.transform.position.x,
            0,
            this.target.position.z - this.egoVehicle.transform.position.z
        );

        float angle = Vector3.SignedAngle(direction.normalized, this.egoVehicle.transform.forward, Vector3.up);

        memory[0] = angle;
    }

    public override void CommandDevice(float[] empty) { }

    void FixedUpdate() {
        if(this.target == null) {
            return;
        }

        this.spinner.LookAt(this.target.position);
        this.spinner.eulerAngles = new Vector3(0, this.spinner.eulerAngles.y, 0);
    }
}
