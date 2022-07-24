using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoEdge : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected MeshRenderer meshRenderer = null;
    [SerializeField] protected MeshFilter meshFilter = null;

    public void Init(EdgeInitData initData, Dictionary<string, SumoJunction> junctionRepo) {
        this.meshRenderer.material = initData.material;

        SumoJunction toNode = junctionRepo[initData.toJunction];
        SumoJunction fromNode = junctionRepo[initData.fromJunction];

        Vector3 center = (toNode.transform.position + fromNode.transform.position) / 2;

        this.transform.localScale = new Vector3(
            initData.width,
            initData.thickness,
            initData.length
        );

        this.transform.position = center;
        this.transform.LookAt(toNode.transform);

        this.transform.Translate(Vector3.right * initData.laneWidth);

        this.BuildLines(initData);
    }

    protected void BuildLines(EdgeInitData initData) {

        int lineCount = (int)(initData.length / (initData.lineSpacing + initData.lineLength));

        for(int i = 0; i < lineCount; i++) {
            GameObject lineObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(lineObject.GetComponent<BoxCollider>());
            lineObject.name = "line";
            MeshRenderer meshRenderer = lineObject.GetComponent<MeshRenderer>();
            meshRenderer.material = initData.lineMaterial;

            lineObject.transform.localScale = new Vector3(initData.laneWidth / 10, initData.lineThickness, initData.lineLength);
            lineObject.transform.position = this.transform.position;
            lineObject.transform.rotation = this.transform.rotation;

            float forwardOffset = (initData.lineLength / 2) + (i * (initData.lineSpacing + initData.lineLength)) - (initData.length / 2);
            lineObject.transform.Translate(Vector3.forward * forwardOffset);

            lineObject.transform.parent = this.transform;
        }
    }
}
