using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AVehicleRepository : ScriptableObject
{
    abstract public AVehiclePrefab GetVehiclePrefab(int index);
    abstract public AVehiclePrefab GetRandomPrefab();
}
