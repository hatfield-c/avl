using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AVehicleAnimator : MonoBehaviour
{
    abstract public void Animate(VehicleState state);
}
