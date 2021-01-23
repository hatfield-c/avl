using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleState
{
    protected float heading;
    protected float speed;
    protected float timer;
    protected bool isBraking;
    public Color color;

    public float GetHeading() {
        return this.heading;
    }

    public float GetSpeed() {
        return this.speed;
    }

    public float GetTimer() {
        return this.timer;
    }

    public bool GetIsBraking() {
        return this.isBraking;
    }

    public void SetHeading(float heading) {
        this.heading = heading;
    }

    public void SetSpeed(float speed) {
        this.speed = speed;
    }

    public void SetTimer(float timer) {
        this.timer = timer;
    }

    public void SetIsBraking(bool state) {
        this.isBraking = state;
    }
}
