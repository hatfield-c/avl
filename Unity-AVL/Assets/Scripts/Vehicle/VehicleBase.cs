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
    [SerializeField] protected float spawnHeight = 1f;

    [Header("Ego Parameters")]
    [SerializeField] protected Color color = new Color();
    [SerializeField] protected float width = 1.8f;
    [SerializeField] protected float length = 5f;
    [SerializeField] protected float height = 1.5f;

    [Header("Internal References")]
    [SerializeField] protected Rigidbody rb = null;

    protected VehicleState vehicleState = new VehicleState();
    
    protected AVehiclePrefab prefab = null;

    public void Init(VehicleInitData initData) {
        this.Enable();

        this.name = initData.vehicleId;
        
        this.vehicleState.SetTransform(this.transform);
        this.vehicleState.SetRigidbody(this.rb);
        this.vehicleState.Init(initData);

        this.prefab.Init(this.vehicleState);
        this.ScaleVehicle(initData);

        this.transform.position = new Vector3(
            this.transform.position.x,
            this.spawnHeight,
            this.transform.position.z
        );

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

        this.transform.localScale = Vector3.one;
        this.prefab.transform.localScale = Vector3.one;

        this.transform.position = Vector3.zero;
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

        data.length = this.length;
        data.width = this.width;
        data.height = this.height;
        
        data.vehicleClass = VehicleManager.DetermineVehicleClass(this.vehicleType);

        return data;
    }

    public string GetInitJson() {
        VehicleInitData data = this.BuildInitData();

        return JsonUtility.ToJson(data);
    }

    protected void ScaleVehicle(VehicleInitData initData) {
        float width = initData.width;
        float length = initData.length;
        float height = initData.height;

        Bounds bounds = this.prefab.GetBoundingBox();

        float widthScale = width / bounds.size.x;
        float lengthScale = length /bounds.size.z;
        float heightScale = height / bounds.size.y;

        this.prefab.transform.localScale = new Vector3(
            widthScale, 
            heightScale, 
            lengthScale
        );
    }

    void OnTriggerEnter(Collider other) {
        /*string msg = $"{this.gameObject.name}, {other.name}, {other.tag}";

        if (other.transform.parent != null) {
            msg += $", {other.transform.parent.name}";
        }*/

        if (other.tag == "road" || other.tag == "Untagged" || other.transform.IsChildOf(this.transform)) {
            return;
        }

        foreach (Collider collider in this.prefab.GetColliders()) {
            collider.isTrigger = false;
        }

        this.rb.isKinematic = false;

        VehicleManager.OnVehicleCollision(this);
    }


}
