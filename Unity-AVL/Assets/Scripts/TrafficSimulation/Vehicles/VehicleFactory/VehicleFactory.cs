using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFactory : MonoBehaviour
{
    [SerializeField] protected float initSpacing;

    public enum VehicleTypes {
        None,
        Car,
        Trailer
    }

    public List<VehicleBase> CreateAllVehicles(VehicleManifest manifest) {
        List<VehicleBase> vehicleList = new List<VehicleBase>();
        VehicleBase vehicleBuffer;

        for(int i = 0; i < manifest.maxCars; i++) {
            vehicleBuffer = this.CreateVehicle(manifest.vehicleBasePrefab, VehicleTypes.Car);
            vehicleList.Add(vehicleBuffer);
        }

        for (int i = 0; i < manifest.maxTrailers; i++) {
            vehicleBuffer = this.CreateVehicle(manifest.vehicleBasePrefab, VehicleTypes.Trailer);
            vehicleList.Add(vehicleBuffer);
        }

        return vehicleList;
    }

    public VehicleBase CreateVehicle(GameObject blueprint, VehicleTypes vehicleType) {
        GameObject vehicleObject = GameObject.Instantiate(
            blueprint, 
            this.transform.position, 
            this.transform.rotation, 
            this.transform
        );
        VehicleBase vehicle = vehicleObject.GetComponent<VehicleBase>();

        vehicle.BuildCar(vehicleType);
        vehicle.Disable();

        return vehicle;
    }

    public static List<string> GetTypeList() {
        return System.Enum.GetNames(typeof(VehicleTypes)).ToList();
    }
}
