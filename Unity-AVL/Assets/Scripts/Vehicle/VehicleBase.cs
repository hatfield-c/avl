using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBase : MonoBehaviour, IStorable
{


    [Header("Init Data")]
    [SerializeField] protected AVehicleRepository vehicleRepo = null;

    [Header("Init Paramters")]
    [SerializeField] protected int defaultPrefabIndex = 0;
    [SerializeField] protected VehicleFactory.VehicleTypes vehicleType = VehicleFactory.VehicleTypes.None;

    [Header("Internal References")]
    [SerializeField] protected Rigidbody rb = null;

    protected VehicleState vehicleState = new VehicleState();
    protected AVehiclePrefab prefab = null;

    public void Init(VehicleInitData initData) {
        this.name = initData.vehicleId;
        this.vehicleState.Init(initData);

        this.prefab.Init(this.vehicleState);
        this.Enable();
    }

    public void UpdateState(VehicleUpdateData updateData) {
        this.vehicleState.Update(updateData, this.transform);
        this.prefab.UpdateState(this.vehicleState);
    }

    public VehicleState GetVehicleState() {
        return this.vehicleState;
    }

    public void BuildCar(VehicleFactory.VehicleTypes vehicleType) {
        this.vehicleType = vehicleType;

        AVehiclePrefab prefabBlueprint = this.vehicleRepo.GetRandomPrefab();

        this.prefab = Instantiate(prefabBlueprint, this.transform.position, this.transform.rotation, this.transform);

        VehiclePhysicsParams physicsParams = this.prefab.GetPhysicsParams();
        physicsParams.InitRigidbody(this.rb);
    }

    public GameObject GetMyGameObject() {
        return this.gameObject;
    }

    public string GetArchetype() {
        return this.vehicleType.ToString();
    }

    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.name = this.vehicleType.ToString() + "_UNUSED";
        this.vehicleState.Reset();
        this.gameObject.SetActive(false);
    }

    public string GetId() {
        return this.vehicleState.GetId();
    }
}
