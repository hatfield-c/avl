using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgoSpawn : MonoBehaviour
{
    [SerializeField]
    protected Transform egoVehicle = null;

    [SerializeField]
    protected Vector3 range = new Vector3(10, 0, 10);

    void Start()
    {
        Vector3 position = new Vector3(
            this.transform.position.x + Random.Range(-this.range.x, this.range.x),
            this.egoVehicle.transform.position.y,
            this.transform.position.z + Random.Range(-this.range.z, this.range.z)
        );

        float angle = Random.Range(-179.99f, 179.99f);

        this.egoVehicle.position = position;
        this.egoVehicle.eulerAngles = new Vector3(0, angle, 0);
    }
}
