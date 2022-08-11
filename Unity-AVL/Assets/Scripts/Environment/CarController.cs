using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    protected Transform egoVehicle = null;

    [SerializeField]
    protected Transform carContainer = null;

    [SerializeField]
    protected List<Material> carMaterials = null;

    [SerializeField]
    protected float minTime = 0f;

    [SerializeField]
    protected float maxTime = 0f;

    [SerializeField]
    protected bool isCarsActive = false;

    protected float timer = 0f;

    void FixedUpdate() {
        if (!this.isCarsActive) {
            return;
        }

        this.timer -= Time.fixedDeltaTime;

        if(this.timer <= 0) {
            this.MoveCar();
        }
    }

    void Start()
    {
        foreach(Transform car in this.carContainer) {
            CarObstacle carObstacle = car.gameObject.GetComponent<CarObstacle>();

            Material randomMat = this.GetRandomMat();
            carObstacle.SetMaterial(randomMat);
        }

        CarObstacle emptySpace = this.GetRandomCar();
        emptySpace.transform.parent = null;
        Destroy(emptySpace.gameObject);

        if (this.isCarsActive) {
            this.MoveCar();
        }
    }

    protected void MoveCar() {
        this.timer = Random.Range(this.minTime, this.maxTime);

        CarObstacle car = this.GetRandomCar();
        car.Move(this.egoVehicle);
    }

    protected CarObstacle GetRandomCar() {
        int index = Random.Range(0, this.carContainer.childCount - 1);
        Transform carTransform = this.carContainer.GetChild(index);

        return carTransform.gameObject.GetComponent<CarObstacle>();
    }

    protected Material GetRandomMat() {
        int index = Random.Range(0, this.carMaterials.Count);
        return this.carMaterials[index];
    }

}
