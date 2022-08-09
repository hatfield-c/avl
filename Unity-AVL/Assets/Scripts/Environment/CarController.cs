using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    protected Transform carContainer = null;

    [SerializeField]
    protected List<Material> carMaterials = null;

    void Start()
    {
        foreach(Transform car in this.carContainer) {
            CarObstacle carObstacle = car.gameObject.GetComponent<CarObstacle>();

            Material randomMat = this.GetRandomMat();
            carObstacle.SetMaterial(randomMat);
        }

        int emptyIndex = Random.Range(0, this.carContainer.childCount);
        Transform emptySpace = this.carContainer.GetChild(emptyIndex);
        emptySpace.parent = null;
        Destroy(emptySpace.gameObject);
    }

    protected Material GetRandomMat() {
        int index = Random.Range(0, this.carMaterials.Count);
        return this.carMaterials[index];
    }

}
