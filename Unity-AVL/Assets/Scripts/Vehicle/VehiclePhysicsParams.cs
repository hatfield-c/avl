using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VehiclePhysicsParams
{
    [Header("Rigidbody Parameters")]
    [SerializeField] protected float mass = 1f;
    [SerializeField] protected float drag = 0f;
    [SerializeField] protected float angularDrag = 0.05f;
    [SerializeField] protected bool useGravity = true;
    [SerializeField] protected bool isKinematic = false;
    [SerializeField] protected RigidbodyInterpolation interpolate = RigidbodyInterpolation.None;
    [SerializeField] protected CollisionDetectionMode collisionMode = CollisionDetectionMode.Discrete;

    public void InitRigidbody(Rigidbody rb) {
        rb.mass = this.mass;
        rb.drag = this.drag;
        rb.angularDrag = this.angularDrag;
        rb.useGravity = this.useGravity;
        rb.isKinematic = this.isKinematic;
        rb.interpolation = this.interpolate;
        rb.isKinematic = this.isKinematic;
        rb.collisionDetectionMode = this.collisionMode;
    }

    public float GetMass() {
        return this.mass;
    }

    public float GetDrag() {
        return this.drag;
    }

    public float GetAngularDrag() {
        return this.angularDrag;
    }

    public bool GetUseGravity() {
        return this.useGravity;
    }

    public bool GetIsKinematic() {
        return this.isKinematic;
    }

    public RigidbodyInterpolation GetInterpolation() {
        return this.interpolate;
    }

    public CollisionDetectionMode GetCollisionMode() {
        return this.collisionMode;
    }
}
