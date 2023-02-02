using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    protected CameraSensor cameraSensor = null;

    [SerializeField]
    protected CameraVisualizer cameraVisualizer = null;

    [Header("Camera Debugging")]

    [SerializeField]
    protected float pixelWidth = 1f;

    [SerializeField]
    protected float pixelDistance = 8f;

    [SerializeField]
    protected bool showRays = false;

    [SerializeField]
    protected float rayLength = 5f;

    void Start()
    {
        this.cameraVisualizer.Init(this.cameraSensor, this.pixelDistance, this.pixelWidth, this.showRays, this.rayLength);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
