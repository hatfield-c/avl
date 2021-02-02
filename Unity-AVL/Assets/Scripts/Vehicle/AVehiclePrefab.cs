using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AVehiclePrefab : MonoBehaviour
{
    [Header("Base Parameters")]
    [SerializeField] protected VehiclePhysicsParams physicsParams = new VehiclePhysicsParams();

    abstract public void Init(VehicleState vehicleState);
    abstract public void UpdateState(VehicleState state);

    public VehiclePhysicsParams GetPhysicsParams() {
        return this.physicsParams;
    }
}
