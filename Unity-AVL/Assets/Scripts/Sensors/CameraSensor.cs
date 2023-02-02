using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSensor : AbstractDevice
{
    [Header("Sensor Parameters")]
    [SerializeField]
    protected Transform lens = null;

    [SerializeField]
    protected GameObject pixelPrefab = null;

    [SerializeField]
    protected int pixelWidth = 16;

    [SerializeField]
    protected int pixelHeight = 8;

    [SerializeField]
    protected float horzFov = 60;

    [SerializeField]
    protected float vertFov = 30;

    [SerializeField]
    protected float maxDistance = 500f;

    [SerializeField]
    protected float cameraWidth = 0.2f;

    [SerializeField]
    protected float cameraHeight = 0.2f;

    [SerializeField]
    protected Color emptyColor = new Color();

    protected MeshRenderer[,] rays = null;
    protected bool showingRays = false;

    void Start()
    {
        this.rays = new MeshRenderer[this.pixelHeight, this.pixelWidth];

        Vector3 horzAmount = this.lens.right * (this.cameraWidth / (float)this.pixelWidth);
        Vector3 vertAmount = this.lens.up * (this.cameraWidth / (float)this.pixelHeight);
        float horzOffset = this.cameraWidth / 2;
        float vertOffset = this.cameraHeight / 2;

        float horzAngle = this.horzFov / (float)this.pixelWidth;
        float vertAngle = this.vertFov / (float)this.pixelHeight;
        float horzAngleOffset = (this.horzFov / 2) - (horzAngle / 2);
        float vertAngleOffset = (this.vertFov / 2) - (vertAngle / 2);

        int index = 0;
        for (int i = 0; i < this.pixelHeight; i++) {
            for(int j = 0; j < this.pixelWidth; j++) {
                GameObject pixelObj = Instantiate(this.pixelPrefab);
                pixelObj.transform.position = this.lens.position;
                pixelObj.transform.eulerAngles = this.lens.eulerAngles;
                pixelObj.transform.parent = this.lens.transform;

                pixelObj.transform.position += -(vertAmount * i) + (this.lens.up * vertOffset);
                pixelObj.transform.position += (horzAmount * j) - (this.lens.right * horzOffset);

                pixelObj.transform.eulerAngles += (Vector3.right * vertAngle * i) - (Vector3.right * vertAngleOffset);
                pixelObj.transform.eulerAngles += (Vector3.up * horzAngle * j) - (Vector3.up * horzAngleOffset);

                pixelObj.name = index.ToString() + "_" + i.ToString() + "," + j.ToString();
                pixelObj.SetActive(false);

                this.rays[i, j] = pixelObj.transform.GetChild(0).GetComponent<MeshRenderer>();
                
                index++;
            }
        }

    }

    public void ShowRays(float rayDistance) {
        if (this.showingRays) {
            return;
        }

        this.showingRays = true;

        foreach (Transform child in this.lens.transform) {
            GameObject pixelObj = child.gameObject;

            pixelObj.transform.localScale = new Vector3(
                pixelObj.transform.localScale.x,
                pixelObj.transform.localScale.y,
                rayDistance
            );

            pixelObj.SetActive(true);
        }
        
    }

    public void UpdateRays(int[,,] pixels) {
        for(int i = 0; i < pixels.GetLength(0); i++) {
            for(int j = 0; j < pixels.GetLength(1); j++) {
                MeshRenderer ray = this.rays[i, j];

                ray.material.color = new Color(
                    pixels[i, j, 0] / 255f,
                    pixels[i, j, 1] / 255f,
                    pixels[i, j, 2] / 255f
                );
            }
        }
    }

    public override void ReadDevice(float[] empty, int[,,] memory) {
        RaycastHit rayData;
        Transform pixel;
        Material hitMat;

        int index = 0;
        for (int i = 0; i < this.pixelHeight; i++) {
            for (int j = 0; j < this.pixelWidth; j++) {
                pixel = this.lens.GetChild(index);

                bool isHit = Physics.Raycast(pixel.position, pixel.forward, out rayData, this.maxDistance);

                int r, g, b;

                if (!isHit) {
                    r = this.GetColor255(this.emptyColor.r);
                    g = this.GetColor255(this.emptyColor.g);
                    b = this.GetColor255(this.emptyColor.b);
                } else {
                    hitMat = rayData.transform.gameObject.GetComponent<MeshRenderer>().material;

                    r = this.GetColor255(hitMat.color.r);
                    g = this.GetColor255(hitMat.color.g);
                    b = this.GetColor255(hitMat.color.b);
                }

                memory[i, j, 0] = r;
                memory[i, j, 1] = g;
                memory[i, j, 2] = b;

                index++;
            }
        }

        if (this.showingRays) {
            this.UpdateRays(memory);
        }
    }

    protected int GetColor255(float intensity) {
        return (int)(255 * intensity);
    }

    public int GetPixelHeight() {
        return this.pixelHeight;
    }

    public int GetPixelWidth() {
        return this.pixelWidth;
    }

    public override void CommandDevice(float[] empty) { }
}
