using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSensor : AbstractDevice
{
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

    protected byte[] sensorData = null;

    void Start()
    {
        this.sensorData = new byte[3 * this.pixelHeight * this.pixelWidth];

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

                pixelObj.name = index.ToString();
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

    public override byte[] CommandDevice(byte[] command) {
        RaycastHit rayData;
        Transform pixel;
        Material hitMat;

        int index = 0;
        for (int i = 0; i < this.pixelHeight; i++) {
            for (int j = 0; j < this.pixelWidth; j++) {
                pixel = this.lens.GetChild(index);

                bool isHit = Physics.Raycast(pixel.position, pixel.forward, out rayData, this.maxDistance);

                if (!isHit) {
                    this.AssignColorByte(this.sensorData, this.emptyColor, index);
                } else {
                    hitMat = rayData.transform.gameObject.GetComponent<MeshRenderer>().material;
                    this.AssignColorByte(this.sensorData, hitMat.color, index);
                }

                index++;
            }
        }

        return this.sensorData;
    }

    protected void AssignColorByte(byte[] sensorData, Color color, int index) {
        byte[] colorByte = this.GetColorByte(color);

        int byteIndex = (3 * index);

        sensorData[byteIndex] = colorByte[0];
        sensorData[byteIndex + 1] = colorByte[1];
        sensorData[byteIndex + 2] = colorByte[2];
    }

    protected byte[] GetColorByte(Color color) {
        return new byte[] {
            (byte)(int)(255 * color.r),
            (byte)(int)(255 * color.g),
            (byte)(int)(255 * color.b),
        };
    }
}
