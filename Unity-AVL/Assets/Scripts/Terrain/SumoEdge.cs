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

        this.transform.Translate(Vector3.right * (initData.width / 2));
    }
}
