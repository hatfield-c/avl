using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSensor : AbstractDevice
{
    [Header("Render Display")]
    [SerializeField]
    protected Transform displayPosition = null;

    [SerializeField]
    protected Transform displayCamera = null;

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

    [Header("Visualize Camera Rays")]
    [SerializeField]
    protected bool visualizeCamera = false;

    [SerializeField]
    protected float renderDistance = 5f;

    void FixedUpdate() {
        this.displayCamera.position = this.displayPosition.position;
        this.displayCamera.rotation = this.displayPosition.rotation;
    }

    void Start()
    {
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

                if (this.visualizeCamera) {
                    
                    pixelObj.transform.localScale = new Vector3(
                        pixelObj.transform.localScale.x,
                        pixelObj.transform.localScale.y,
                        this.renderDistance
                    );

                    pixelObj.SetActive(true);
                }
                
                index++;
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
