using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SovietCarRepository", menuName = "ScriptableObjects/SovietCarRepository", order = 1)]
public class SovietCarRepository : AVehicleRepository
{
    public List<SovietPrefab> prefabList = new List<SovietPrefab>();
    protected SovietPrefab prefabBuffer;

    override public AVehiclePrefab GetVehiclePrefab(int index) {
        this.prefabBuffer = this.prefabList[index];

        if(this.prefabBuffer == null) {
            Debug.LogError("Prefab returned by Soviet Repository is null.");
        }

        return this.prefabList[index];
    }

    public override AVehiclePrefab GetRandomPrefab() {
        int randomIndex = Random.Range(0, this.prefabList.Count);
        this.prefabBuffer = this.prefabList[randomIndex];

        if (this.prefabBuffer == null) {
            Debug.LogError("Prefab returned by Soviet Repository is null.");
        }

        return this.prefabList[randomIndex];
    }
}
