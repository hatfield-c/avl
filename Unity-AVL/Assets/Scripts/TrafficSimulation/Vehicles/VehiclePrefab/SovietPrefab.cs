using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SovietPrefab : AVehiclePrefab
{
    [System.Serializable]
    public struct WheelData {
        public GameObject frontRight;
        public GameObject frontLeft;
        public GameObject backRight;
        public GameObject backLeft;
    }

    [System.Serializable]
    protected struct TextureData {
        public Texture2D carTexture;
        public Vector2Int pixelRedrawPos;
        public Vector2Int pixelRedrawSize;
    };

    [Header("Init Data")]
    [SerializeField] protected TextureData textureData = new TextureData();

    [Header("Internal References")]
    [SerializeField] protected List<Light> brakeLights = new List<Light>();
    [SerializeField] protected WheelData wheelData = new WheelData();
    [SerializeField] protected MeshRenderer meshRenderer = null;
    [SerializeField] protected AVehicleAnimator animator = null;

    protected Texture2D generatedTexture = null;

    public override void Init(VehicleState vehicleState) {
        this.InitCarTexture(vehicleState.GetColor());
    }

    public override void UpdateState(VehicleState state) {
        if (this.animator == null) {
            return;
        }

        this.animator.Animate(state);
    }

    public List<Light> GetBrakeLights() {
        return this.brakeLights;
    }

    public SovietPrefab.WheelData GetWheels() {
        return this.wheelData;
    }

    protected void InitCarTexture(Color color) {
        this.generatedTexture = new Texture2D(this.textureData.carTexture.width, this.textureData.carTexture.height);
        Graphics.CopyTexture(this.textureData.carTexture, this.generatedTexture);

        int numPixels = this.textureData.pixelRedrawSize.x * this.textureData.pixelRedrawSize.x;
        Color[] newColors = new Color[numPixels];

        for (int i = 0; i < numPixels; i++) {
            newColors[i] = new Color(color.r, color.g, color.b);
        }

        this.generatedTexture.SetPixels(
            this.textureData.pixelRedrawPos.x,
            this.textureData.pixelRedrawPos.y,
            this.textureData.pixelRedrawSize.x,
            this.textureData.pixelRedrawSize.x, 
            newColors
        );
        this.generatedTexture.Apply();
        this.meshRenderer.material.mainTexture = this.generatedTexture;
    }
}
