using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AVehiclePrefab : MonoBehaviour
{
    [Header("Base Parameters")]
    [SerializeField] protected VehiclePhysicsParams physicsParams = new VehiclePhysicsParams();
    [SerializeField] protected List<Collider> colliders = new List<Collider>();

    protected Bounds boundingBox = new Bounds();

    abstract public void Init(VehicleState vehicleState);
    abstract public void UpdateState(VehicleState state);

    public VehiclePhysicsParams GetPhysicsParams() {
        return this.physicsParams;
    }

    public List<Collider> GetColliders() {
        return this.colliders;
    }

    public Bounds GetBoundingBox() {
        if (this.colliders.Count < 1) {
            return this.boundingBox;
        }

        if (this.boundingBox.size.magnitude > 0) {
            return this.boundingBox;
        }

        Vector3 averageCenter = Vector3.zero;
        foreach(Collider collider in this.colliders) {
            averageCenter += collider.bounds.center;
        }

        averageCenter /= this.colliders.Count;

        Bounds bounds = new Bounds(averageCenter, this.colliders[0].bounds.size);

        foreach (Collider collider in this.colliders) {
            bounds.Encapsulate(collider.bounds);
        }

        return bounds;
    }
}
