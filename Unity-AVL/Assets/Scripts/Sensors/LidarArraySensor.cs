using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarArraySensor : AbstractDevice
{
    [SerializeField]
    protected GameObject rayPrefab = null;

    [SerializeField]
    protected float maxDistance = 10;

    [SerializeField]
    protected List<Transform> lidarArray = new List<Transform>();

    protected MeshRenderer[] rays = null;
    protected bool showRays = false;

    protected Color closeColor = new Color(1, 0, 0);
    protected Color farColor = new Color(0, 1, 0);

    override public void ReadDevice(float[] memory, int[,,] empty) {
        RaycastHit rayData;
        Transform lidar;

        for (int i = 0; i < this.lidarArray.Count; i++) {
            lidar = this.lidarArray[i];

            bool isHit = Physics.Raycast(lidar.position, lidar.up, out rayData, this.maxDistance);

            if (isHit) {
                memory[i] = rayData.distance;
            } else {
                memory[i] = this.maxDistance;
            }
        }

        if (this.showRays) {
            this.UpdateRays(memory);
        }
    }

    protected void UpdateRays(float[] lidarVals) {
        for (int i = 0; i < this.lidarArray.Count; i++) {
            float dist = lidarVals[i];
            float lerp = dist / this.maxDistance;

            MeshRenderer ray = this.rays[i];

            Color rayColor = Color.Lerp(this.closeColor, this.farColor, lerp);

            ray.material.color = rayColor;
            ray.transform.parent.localScale = new Vector3(
                ray.transform.parent.localScale.x,
                ray.transform.parent.localScale.y,
                dist
            );
        }
    }

    public void CreateRays() {
        this.showRays = true;
        this.rays = new MeshRenderer[this.lidarArray.Count];

        GameObject rayContainer = new GameObject("RayContainer");
        rayContainer.transform.position = this.transform.position;
        rayContainer.transform.parent = this.transform;

        for (int i = 0; i < this.lidarArray.Count; i++) {
            Transform lidar = this.lidarArray[i];
            
            GameObject rayObj = Instantiate(this.rayPrefab);

            rayObj.transform.position = lidar.transform.position;
            rayObj.transform.eulerAngles = new Vector3(0, lidar.transform.eulerAngles.y, 0);
            rayObj.transform.parent = rayContainer.transform;

            MeshRenderer rayRenderer = rayObj.transform.GetChild(0).GetComponent<MeshRenderer>();
            this.rays[i] = rayRenderer;
        }
    }

    public override void CommandDevice(float[] empty) { }

    public List<Transform> GetLidarArray() {
        return this.lidarArray;
    }

    public int GetLidarCount() {
        return this.lidarArray.Count;
    }

}
