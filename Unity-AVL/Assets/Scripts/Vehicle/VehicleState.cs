using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleState
{
    protected string vehicleId;
    protected float heading;
    protected float speed;
    protected float timer;
    protected bool isBraking;
    protected Color color;

    protected Vector3 vector3Buffer = new Vector3();

    public void Init(VehicleInitData initData) {
        this.vehicleId = initData.vehicleId;

        Color color;
        ColorUtility.TryParseHtmlString(initData.colorHex, out color);
        this.color = color;
    }

    public void Update(VehicleUpdateData updateData, Transform vehicleTransform) {
        this.vector3Buffer = vehicleTransform.position;
        this.vector3Buffer.x = (float)(updateData.position[0]);
        this.vector3Buffer.z = (float)(updateData.position[1]);

        vehicleTransform.position = this.vector3Buffer;
        vehicleTransform.rotation = Quaternion.AngleAxis(updateData.heading, Vector3.up);

        this.heading = updateData.heading;
        this.speed = updateData.speed;
        this.timer = Time.fixedDeltaTime;
        this.isBraking = updateData.brake;
    }

    public void Reset() {
        this.vehicleId = null;
        this.heading = 0;
        this.speed = 0;
        this.timer = 0;
        this.isBraking = false;
        this.color = Color.white;
    }

    public string GetId() {
        return this.vehicleId;
    }

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

    public Color GetColor() {
        return this.color;
    }
}
