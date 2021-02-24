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

    protected bool active;
    protected Rigidbody rb = null;
    protected Transform transform = null;

    protected Vector3 vector3Buffer = new Vector3();

    protected Quaternion previousOrientation = new Quaternion();

    public void Init(VehicleInitData initData) {
        this.vehicleId = initData.vehicleId;
        this.active = true;

        Color color;
        ColorUtility.TryParseHtmlString(initData.colorHex, out color);
        this.color = color;
        
        this.rb.velocity = Vector3.zero;
        this.rb.angularVelocity = Vector3.zero;
    }

    public void Update(VehicleUpdateData updateData, float yOffset) {
        this.vector3Buffer.x = (float)(updateData.position[0]);
        this.vector3Buffer.y = yOffset;
        this.vector3Buffer.z = (float)(updateData.position[1]);


        this.heading = updateData.heading;
        this.speed = updateData.speed;
        this.isBraking = updateData.brake;

        this.previousOrientation = this.transform.rotation;

        this.rb.MovePosition(this.vector3Buffer);
        this.rb.MoveRotation(Quaternion.AngleAxis(updateData.heading, Vector3.up));
    }

    public void Collision() {
        if (!this.active) {
            return;
        }

        this.active = false;

        this.rb.velocity = this.transform.forward * this.speed;

        Quaternion difference = this.transform.rotation * Quaternion.Inverse(this.previousOrientation);
        Vector3 angles = difference.eulerAngles * Mathf.Deg2Rad;
        angles /= Time.fixedDeltaTime;

        this.rb.angularVelocity = angles;
    }

    public void Reset() {
        this.vehicleId = null;
        this.heading = 0;
        this.speed = 0;
        this.timer = Time.fixedDeltaTime;
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

    public void SetRigidbody(Rigidbody rb) {
        this.rb = rb;
    }

    public void SetTransform(Transform transform) {
        this.transform = transform;
    }
}
