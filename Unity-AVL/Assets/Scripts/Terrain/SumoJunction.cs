using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoJunction : MonoBehaviour
{
    protected string junctionId = null;
    protected List<Vector3> shape = new List<Vector3>();

    public void Init(JunctionInitData initData) {
        this.gameObject.name += "_" + initData.junctionId;

        this.junctionId = initData.junctionId;
        this.transform.position = new Vector3(
            (float)initData.position[0],
            0,
            (float)initData.position[1]
        );

        foreach (List<double> vertexPos in initData.shape) {
            Vector3 vertex = new Vector3(
                (float)vertexPos[0],
                0,
                (float)vertexPos[1]
            );

            this.shape.Add(vertex);
        }

    }
}
