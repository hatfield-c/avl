﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SovietCarRepository", menuName = "ScriptableObjects/SovietCarRepository", order = 1)]
public class SovietCarRepository : AVehicleRepository
{
    public List<SovietPrefab> prefabList = new List<SovietPrefab>();

    override public AVehiclePrefab GetVehiclePrefab(int index) {
        return this.prefabList[index];
    }

    public override AVehiclePrefab GetRandomPrefab() {
        int randomIndex = Random.Range(0, this.prefabList.Count);

        return this.prefabList[randomIndex];
    }
}
