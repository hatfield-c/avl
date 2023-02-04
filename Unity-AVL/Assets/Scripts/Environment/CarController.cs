using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public enum SpawnAction {
        DoNothing,
        RandomSpace,
        RandomFirstRow,
        FinishedAction
    };

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
    protected int emptyIndex = -1;
    protected SpawnAction currentAction = SpawnAction.FinishedAction;

    public void SetAction(SpawnAction newAction) {
        if (this.currentAction != SpawnAction.DoNothing) {
            return;
        }

        this.currentAction = newAction;
    }

    protected void SetEmptySpace(int index) {
        if(this.emptyIndex >= 0) {
            CarObstacle emptySpace = this.carContainer.GetChild(this.emptyIndex).GetComponent<CarObstacle>();
            emptySpace.gameObject.SetActive(true);
        }

        this.emptyIndex = index;

        CarObstacle newEmptySpace = this.carContainer.GetChild(index).GetComponent<CarObstacle>();
        newEmptySpace.gameObject.SetActive(false);
        
    }

    protected void RandomEmptySpace() {
        int index = Random.Range(0, this.carContainer.childCount - 1);
        this.SetEmptySpace(index);
    }

    protected void RandomFirstRow() {
        int index = Random.Range(0, 9);
        this.SetEmptySpace(index);
    }

    protected void TakeAction(SpawnAction action) {
        if(action == SpawnAction.DoNothing) {
            return;
        }

        if(action == SpawnAction.RandomSpace) {
            this.RandomEmptySpace();
            this.currentAction = SpawnAction.FinishedAction;
            return;
        }

        if (action == SpawnAction.RandomFirstRow) {
            this.RandomFirstRow();
            this.currentAction = SpawnAction.FinishedAction;
            return;
        }
    }

    void FixedUpdate() {
        if(this.currentAction == SpawnAction.FinishedAction) {
            this.currentAction = SpawnAction.DoNothing;
        }

        this.TakeAction(this.currentAction);

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

        this.SetAction(SpawnAction.RandomSpace);

        if (this.isCarsActive) {
            this.MoveCar();
        }
    }

    protected CarObstacle GetRandomCar() {
        int index = Random.Range(0, this.carContainer.childCount - 1);
        Transform carTransform = this.carContainer.GetChild(index);

        return carTransform.gameObject.GetComponent<CarObstacle>();
    }


    protected void MoveCar() {
        this.timer = Random.Range(this.minTime, this.maxTime);

        CarObstacle car = this.GetRandomCar();
        car.Move(this.egoVehicle);
    }

    protected Material GetRandomMat() {
        int index = Random.Range(0, this.carMaterials.Count);
        return this.carMaterials[index];
    }
}
