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

    public void UpdateState() {
        this.prefab.UpdateState(this.vehicleState);
    }

    public VehicleState GetVehicleState() {
        return this.vehicleState;
    }

    public void Init(VehicleInitData initData) {
        AVehiclePrefab prefabBlueprint = this.vehicleRepo.GetVehiclePrefab(this.defaultPrefabIndex);

        this.prefab = Instantiate(prefabBlueprint, this.transform.position, this.transform.rotation, this.transform);

        VehiclePhysicsParams physicsParams = this.prefab.GetPhysicsParams();
        physicsParams.InitRigidbody(this.rb);

        this.prefab.Init(initData);
    }

    public void SetType(VehicleFactory.VehicleTypes vehicleType) {
        this.vehicleType = vehicleType;
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
        this.gameObject.SetActive(false);
    }

    void Start() {
        VehicleInitData data = new VehicleInitData();
        data.colorHex = "#080000";
        this.Init(data);
    }
}
