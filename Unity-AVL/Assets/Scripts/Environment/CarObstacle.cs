using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarObstacle : MonoBehaviour
{
    [SerializeField]
    protected MeshRenderer bodyRenderer = null;

    [SerializeField]
    protected MeshRenderer topRenderer = null;

    public void SetMaterial(Material material) {
        this.bodyRenderer.material = material;
        this.topRenderer.material = material;
    }
}
