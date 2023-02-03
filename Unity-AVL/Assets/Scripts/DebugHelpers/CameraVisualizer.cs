using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVisualizer : MonoBehaviour
{
    [SerializeField]
    protected GameObject pixelPrefab = null;

    protected CameraSensor cameraSensor = null;
    protected float canvasDistance = 0f;
    protected float pixelWidth = 0f;
    protected bool showRays = false;
    protected float rayLength = 0f;

    protected float[] empty;
    protected int[,,] pixelVals;
    protected MeshRenderer[,] pixels;

    protected bool isInit = false;

    public void Init(
        CameraSensor cameraSensor, 
        float canvasDistance, 
        float pixelWidth,
        bool showRays,
        float rayLength
       ) {
        this.cameraSensor = cameraSensor;
        this.canvasDistance = canvasDistance;
        this.pixelWidth = pixelWidth;
        this.showRays = showRays;
        this.rayLength = rayLength;

        this.pixelVals = new int[this.cameraSensor.GetPixelHeight(), this.cameraSensor.GetPixelWidth(), 3];
        this.empty = new float[1];
        this.pixels = new MeshRenderer[this.cameraSensor.GetPixelHeight(), this.cameraSensor.GetPixelWidth()];

        this.CreatePixels();

        this.isInit = true;
    }

    void FixedUpdate() {
        this.cameraSensor.ReadDevice(this.empty, this.pixelVals);

        if (this.showRays) {
            this.cameraSensor.ShowRays(this.rayLength);
        }

        this.RenderPixels(this.pixelVals);
    }

    protected void CreatePixels() {
        float xOffset = (this.pixels.GetLength(1) * this.pixelWidth) / 2;
        float zOffset = (this.pixels.GetLength(0) * this.pixelWidth) / 2;

        xOffset -= this.pixelWidth / 2;
        zOffset -= this.pixelWidth / 2;

        for (int i = 0; i < this.pixels.GetLength(0); i++) {
            for (int j = 0; j < this.pixels.GetLength(1); j++) {
                float xPos = this.transform.position.x - (j * this.pixelWidth) + xOffset;
                float yPos = this.transform.position.y - this.canvasDistance;
                float zPos = this.transform.position.z + (i * this.pixelWidth) - zOffset;

                GameObject pixelObj = Instantiate(this.pixelPrefab);

                pixelObj.transform.position = new Vector3(xPos, yPos, zPos);
                pixelObj.transform.localScale = new Vector3(this.pixelWidth, this.pixelWidth, this.pixelWidth);
                pixelObj.transform.parent = this.transform;

                MeshRenderer pixelRenderer = pixelObj.GetComponent<MeshRenderer>();
                this.pixels[i, j] = pixelRenderer;
            }
        }
    }

    protected void RenderPixels(int[,,] pixelVals) {
        for(int i = 0; i < pixels.GetLength(0); i++) {
            for(int j = 0; j < pixels.GetLength(1); j++) {
                MeshRenderer pixel = this.pixels[i, j];
                
                pixel.material.color = new Color(
                    pixelVals[i, j, 0] / 255f,
                    pixelVals[i, j, 1] / 255f,
                    pixelVals[i, j, 2] / 255f
                );
            }
        }
    }
}
