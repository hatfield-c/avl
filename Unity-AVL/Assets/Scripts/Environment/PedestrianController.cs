using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianController : MonoBehaviour
{
    [SerializeField]
    protected Transform egoVehicle = null;

    [SerializeField]
    protected Vector3 bounds = new Vector3();

    void Start()
    {
        foreach(Transform pedTransform in this.transform) {
            Pedestrian pedestrian = pedTransform.gameObject.GetComponent<Pedestrian>();
            pedestrian.Init(this.egoVehicle, this.transform.position, this.bounds);
        }
    }

}
