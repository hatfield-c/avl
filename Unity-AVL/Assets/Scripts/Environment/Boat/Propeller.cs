using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    [SerializeField]
    protected Transform propeller = null;

    [SerializeField]
    protected float spinRate = 3f;

    void FixedUpdate()
    {
        this.propeller.eulerAngles += Vector3.forward * this.spinRate;
    }
}
