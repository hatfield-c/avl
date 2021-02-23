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
    [SerializeField] protected float spawnHeight = 2f;
    [SerializeField] protected Color color = new Color();

    [Header("Internal References")]
    [SerializeField] protected Rigidbody rb = null;

    protected VehicleState vehicleState = new VehicleState();
    protected AVehiclePrefab prefab = null;

    public void Init(VehicleInitData initData) {
        this.name = initData.vehicleId;

        this.vehicleState.SetTransform(this.transform);
        this.vehicleState.SetRigidbody(this.rb);
        this.vehicleState.Init(initData);

        this.prefab.Init(this.vehicleState);
        this.Enable();
    }

    public void UpdateState(VehicleUpdateData updateData) {
        this.vehicleState.Update(updateData, this.spawnHeight);
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

    public string GetUpdateJson() {

        VehicleUpdateData data = new VehicleUpdateData();
        data.vehicleId = this.name;
        
        data.position = new List<float>();
        data.position.Add(this.transform.position.x);
        data.position.Add(this.transform.position.z);

        data.speed = this.rb.velocity.magnitude;
        data.brake = false;

        data.heading = this.transform.eulerAngles.y;

        return JsonUtility.ToJson(data);
    }

    public VehicleInitData BuildInitData() {
        VehicleInitData data = new VehicleInitData();

        data.vehicleId = this.name;
        data.colorHex = ColorUtility.ToHtmlStringRGB(this.color);

        data.heading = this.transform.eulerAngles.y;

        data.vehicleClass = VehicleManager.DetermineVehicleClass(this.vehicleType);

        data.position = new List<float>();
        data.position.Add(this.transform.position.x);
        data.position.Add(this.transform.position.z);

        return data;
    }

    public string GetInitJson() {
        VehicleInitData data = this.BuildInitData();

        return JsonUtility.ToJson(data);
    }

    void OnTriggerEnter(Collider other) {
        //Debug.Log($"{this.gameObject.name}, {other.name}");

        foreach(Collider collider in this.prefab.GetColliders()) {
            //collider.isTrigger = false;
        }
    }
}
