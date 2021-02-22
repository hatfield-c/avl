using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class TerrainManager : MonoBehaviour
{
    [Header("Edges")]
    [SerializeField] protected GameObject edgePrefab = null;
    [SerializeField] protected GameObject edgeContainer = null;
    [SerializeField] protected float thickness = 0.1f;

    [Header("Junctions")]
    [SerializeField] protected GameObject junctionPrefab = null;
    [SerializeField] protected GameObject junctionContainer = null;

    [Header("Parameters")]
    [SerializeField] protected Material asphaltMaterial = null;
    [SerializeField] protected Material lineMaterial = null;
    [SerializeField] protected float lineLength = 2f;
    [SerializeField] protected float lineSpacing = 1f;
    [SerializeField] protected float linethickness = 0.15f;

    protected Dictionary<string, SumoJunction> junctionRepo = new Dictionary<string, SumoJunction>();
    protected Dictionary<string, SumoEdge> edgeRepo = new Dictionary<string, SumoEdge>();

    protected const string JUNC_INIT_ERR_MSG = "[Terrain Manager: Junction Init Error]";
    protected const string EDGE_INIT_ERR_MSG = "[Terrain Manager: Edge Init Error]";

    public void CreateJunctions(string rawData) {

        string[] dataPerVehicle = rawData.Split(TcpProtocol.DATA_DELIM);

        JunctionInitData initData;
        for (int i = 0; i < dataPerVehicle.Length; i++) {
            initData = JsonConvert.DeserializeObject<JunctionInitData>(dataPerVehicle[i]);

            if (initData.junctionId == null) {
                this.LogError(
                    TerrainManager.JUNC_INIT_ERR_MSG,
                    "Junction ID received from SUMO is null."
                );
                continue;
            }

            GameObject junctionObject = Instantiate(this.junctionPrefab);
            SumoJunction junction = junctionObject.GetComponent<SumoJunction>();

            initData.material = this.asphaltMaterial;

            junction.Init(initData);
            junction.transform.parent = this.junctionContainer.transform;

            if (this.junctionRepo.ContainsKey(initData.junctionId)) {
                this.LogError(
                    TerrainManager.JUNC_INIT_ERR_MSG,
                    "Fatal error! Trying to init junction with ID '" +
                    initData.junctionId +
                    "', but a junction with this ID already exists in the simulation!"
                );

                UnityListener.StopListening();
            }

            this.junctionRepo.Add(initData.junctionId, junction);
        }
    }

    public void CreateEdges(string rawData) {
        string[] dataPerVehicle = rawData.Split(TcpProtocol.DATA_DELIM);

        EdgeInitData initData;
        for (int i = 0; i < dataPerVehicle.Length; i++) {
            initData = JsonConvert.DeserializeObject<EdgeInitData>(dataPerVehicle[i]);

            if (initData.edgeId == null) {
                this.LogError(
                    TerrainManager.EDGE_INIT_ERR_MSG,
                    "Edge ID received from SUMO is null."
                );
                continue;
            }

            GameObject edgeObject = Instantiate(this.edgePrefab);
            SumoEdge edge = edgeObject.GetComponent<SumoEdge>();

            initData.material = this.asphaltMaterial;
            initData.lineMaterial = this.lineMaterial;
            initData.thickness = this.thickness;
            initData.lineLength = this.lineLength;
            initData.lineSpacing = this.lineSpacing;
            initData.lineThickness = this.linethickness;

            edge.Init(initData, this.junctionRepo);
            edge.transform.parent = this.edgeContainer.transform;

            if (this.edgeRepo.ContainsKey(initData.edgeId)) {
                this.LogError(
                    TerrainManager.EDGE_INIT_ERR_MSG,
                    "Fatal error! Trying to init edge with ID '" +
                    initData.edgeId +
                    "', but an edge with this ID already exists in the simulation!"
                );

                UnityListener.StopListening();
            }

            this.edgeRepo.Add(initData.edgeId, edge);
        }
    }

    protected void LogError(string type, string msg) {
        Debug.LogError(type + ": " + msg);
    }
}
