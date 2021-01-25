using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBase : MonoBehaviour
{
    [Header("Init Data")]
    [SerializeField] protected AVehicleRepository vehicleRepo = null;

    [Header("Init Paramters")]
    [SerializeField] protected int defaultPrefabIndex = 0;

    [Header("Internal References")]
    [SerializeField] protected Rigidbody rb = null;

    protected VehicleState vehicleState = new VehicleState();
    protected AVehiclePrefab prefab = null;

    public void UpdateState() {
        this.prefab.UpdateState(this.vehicleState);
    }

    public VehicleState GetVehicleState() {
        return this.vehicleState;
    }

    public void Init() {

    }

    void Start() {
        AVehiclePrefab prefabBlueprint = this.vehicleRepo.GetVehiclePrefab(this.defaultPrefabIndex);

        this.prefab = Instantiate(prefabBlueprint, this.transform.position, this.transform.rotation, this.transform);
        
        VehiclePhysicsParams physicsParams = this.prefab.GetPhysicsParams();
        physicsParams.InitRigidbody(this.rb);

        VehicleInitData data = new VehicleInitData();
        data.colorHex = "#080000";
        this.prefab.Init(data);
    }
}
